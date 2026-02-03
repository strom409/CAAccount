using Microsoft.JSInterop;
using System.Threading.Tasks;

public class StorageService : IStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public StorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetSessionItemAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorageHelper.setItem", key, value);
    }

    public async Task<string> GetSessionItemAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("sessionStorageHelper.getItem", key);
    }
}
