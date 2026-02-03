using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

public class SessionTimeoutService : IDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    private Timer _timer;
    private CancellationTokenSource _cts;
    private DotNetObjectReference<SessionTimeoutService> _dotNetRef;
    private bool _disposed = false;

    // Events for UI notifications
    public event Action OnSessionTimeoutWarning;
    public event Action OnSessionTimeout;

    // Configuration
    public int WarningTimeSeconds { get; set; } = 60;
    public int TimeoutSeconds { get; set; } = 1200;
    public int CheckIntervalSeconds { get; set; } = 30;

    public SessionTimeoutService(IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("sessionTimeout.initialize", _dotNetRef, TimeoutSeconds * 1000);

            _cts = new CancellationTokenSource();
            StartPeriodicCheck();
        }
        catch (JSException ex)
        {
            Console.WriteLine($"JavaScript interop failed: {ex.Message}");
            StartPeriodicCheck();
        }
    }

    private void StartPeriodicCheck()
    {
        _timer = new Timer(async _ => await PeriodicCheckAsync(),
            null,
            TimeSpan.FromSeconds(CheckIntervalSeconds),
            TimeSpan.FromSeconds(CheckIntervalSeconds));
    }

    private async Task PeriodicCheckAsync()
    {
        if (_cts?.IsCancellationRequested == true || _disposed)
            return;

        try
        {
            await CheckSessionValidityAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in periodic check: {ex.Message}");
        }
    }

    [JSInvokable]
    public async Task UserIsActive()
    {
        if (_disposed || _cts?.IsCancellationRequested == true)
            return;

        try
        {
            await _jsRuntime.InvokeVoidAsync("sessionTimeout.reset");
        }
        catch (JSException ex)
        {
            Console.WriteLine($"JavaScript reset failed: {ex.Message}");
        }
    }

    [JSInvokable]
    public void ShowTimeoutWarning()
    {
        OnSessionTimeoutWarning?.Invoke();
    }

    [JSInvokable]
    public async Task TimeoutUser()
    {
        OnSessionTimeout?.Invoke();
        await LogoutUserAsync();
    }

    private async Task CheckSessionValidityAsync()
    {
        // Implement your session validation logic
        await Task.CompletedTask;
    }

    public async Task LogoutUserAsync()
    {
        if (_disposed) return;

        try
        {
            await ClearAuthenticationAsync();
            _navigationManager.NavigateTo("/logout", true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during logout: {ex.Message}");
            _navigationManager.NavigateTo("/logout?error=true", true);
        }
    }

    private async Task ClearAuthenticationAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("eval",
                @"localStorage.removeItem('authToken');
                  sessionStorage.removeItem('userSession');");
        }
        catch (JSException)
        {
            // Ignore
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _cts?.Cancel();
        _cts?.Dispose();
        _timer?.Dispose();
        _dotNetRef?.Dispose();

        OnSessionTimeoutWarning = null;
        OnSessionTimeout = null;
    }
}