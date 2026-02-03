using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

public class UserSessionService : IAsyncDisposable
{

    private readonly NavigationManager _navigationManager;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly IJSRuntime _jsRuntime;
    private bool _disposed = false;
    private IJSObjectReference? _jsModule;
    public string  GlobalUserGroupall { get; set; }
    public bool IsLoading { get; private set; }
    public bool IsInitialized { get; private set; }
    public string Usernamel { get; private set; } = string.Empty;
    public string GlobalUserGroup { get; private set; } = string.Empty;
    public string UnitNamel { get; private set; } = string.Empty;
    public int UnitId { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(Usernamel);
    //public UserSessionService(NavigationManager navigationManager)
    //{
    
    //    _navigationManager = navigationManager;
    //}
    public event Func<Task>? OnUserDataChanged;

    private readonly IStorageService _storage;

    private readonly CookieService _cookieService;
    public UserSessionService(CookieService cookieService)
    {
        _cookieService = cookieService;
    }
    public UserSessionService(IStorageService storage)
    {
        _storage = storage;
    }


    public UserSessionService(ProtectedLocalStorage protectedLocalStorage, IJSRuntime jsRuntime)
    {
        _protectedLocalStorage = protectedLocalStorage;
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (IsInitialized) return;

        IsLoading = true;
        try
        {
            // Load JavaScript module for storage events
            _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/storageEvents.js");

            // Load user data from storage
            await LoadUserFromStorageAsync();

            // Setup storage listener
            if (_jsModule != null)
            {
                var dotNetRef = DotNetObjectReference.Create(this);
                await _jsModule.InvokeVoidAsync("registerStorageListener", dotNetRef);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing UserSessionService: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoginAsync(string username, string unitname, int unitid, string globalUserGroup)
    {
        IsLoading = true;
        try
        {
            Usernamel = username;
            UnitNamel = unitname;
            UnitId = unitid;
            GlobalUserGroup = globalUserGroup;

            // Save to local storage
            await _protectedLocalStorage.SetAsync("Username", username);
            await _protectedLocalStorage.SetAsync("UserGroup", globalUserGroup);
            await _protectedLocalStorage.SetAsync("UnitName", unitname);
            await _protectedLocalStorage.SetAsync("UnitId", unitid);

            IsInitialized = true;
            await NotifyUserDataChangedAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LogoutAsync(NavigationManager navManager)
    {
        IsLoading = true;
        try
        {
            Usernamel = string.Empty;
            UnitNamel = string.Empty;
            GlobalUserGroup = string.Empty;
            UnitId = 0;

            // Remove from local storage
            await _protectedLocalStorage.DeleteAsync("Username");
            await _protectedLocalStorage.DeleteAsync("UserGroup");
            await _protectedLocalStorage.DeleteAsync("UnitName");
            await _protectedLocalStorage.DeleteAsync("UnitId");

            IsInitialized = false;
            await NotifyUserDataChangedAsync();
            navManager.NavigateTo("/login", true); // true = force reload

        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadUserFromStorageAsync()
    {
        try
        {
            var userResult = await _protectedLocalStorage.GetAsync<string>("Username");
            var groupResult = await _protectedLocalStorage.GetAsync<string>("UserGroup");
            var unitNameResult = await _protectedLocalStorage.GetAsync<string>("UnitName");
            var unitIdResult = await _protectedLocalStorage.GetAsync<int>("UnitId");

            Usernamel = userResult.Success ? userResult.Value ?? string.Empty : string.Empty;
            GlobalUserGroup = groupResult.Success ? groupResult.Value ?? string.Empty : string.Empty;
            UnitNamel = unitNameResult.Success ? unitNameResult.Value ?? string.Empty : string.Empty;
            UnitId = unitIdResult.Success ? unitIdResult.Value : 0;

            IsInitialized = !string.IsNullOrEmpty(Usernamel);
            await NotifyUserDataChangedAsync();
           // await SetupCrossTabSync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user from storage: {ex.Message}");
            Usernamel = string.Empty;
            GlobalUserGroup = string.Empty;
            UnitNamel = string.Empty;
            UnitId = 0;
            IsInitialized = false;
        }

    }

    public async Task LoadUserFromStorageAsyncpage()
    {
        try
        {
            // Direct JS interop without separate CookieService
            GlobalUserGroupall = await _jsRuntime.InvokeAsync<string>("getCookie", "userGrouppriv");


            if (string.IsNullOrEmpty(GlobalUserGroupall))
            {
                GlobalUserGroupall = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userGrouppriv");

            }
            if (!string.IsNullOrEmpty(GlobalUserGroupall))
            {
                await _jsRuntime.InvokeVoidAsync("setCookie", "userGrouppriv", GlobalUserGroupall, 7);
            }


        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from cookie: {ex.Message}");
        }
    
       
    }
    public async Task SaveUserGroupAsync(string userGroup)
    {
        try
        {
            GlobalUserGroupall = userGroup;

            // Save to BOTH cookie and localStorage for redundancy
            await _jsRuntime.InvokeVoidAsync("setCookie", "userGrouppriv", userGroup, 7); 
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userGrouppriv", userGroup);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to cookie: {ex.Message}");
        }
    }

        private async Task SetupCrossTabSync()
    {
        // Listen for storage changes (works across tabs)
        await _jsModule.InvokeVoidAsync("eval", @"
            window.addEventListener('storage', function(e) {
                if (e.key === 'userSession') {
                    // Trigger reload when session changes in another tab
                    window.location.reload();
                }
            });
        ");
    }

    [JSInvokable]



    public async Task OnStorageChangedAsync(string key)
    {
        Console.WriteLine($"Storage changed detected: {key}");
        await LoadUserFromStorageAsync();
    }

    private async Task NotifyUserDataChangedAsync()
    {
        if (OnUserDataChanged != null)
        {
            await OnUserDataChanged.Invoke();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            _disposed = true;

            if (_jsModule != null)
            {
                try
                {
                    await _jsModule.InvokeVoidAsync("unregisterStorageListener");
                    await _jsModule.DisposeAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during JS module disposal: {ex.Message}");
                }
            }
        }
    }
}