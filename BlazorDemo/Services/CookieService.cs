using Microsoft.JSInterop;
using System.Threading.Tasks;

public class CookieService
{
    private readonly IJSRuntime _jsRuntime;

    public CookieService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetCookieAsync(string key, string value, int? days = null)
    {
        var expires = days.HasValue ? days.Value : 7; // Default 7 days
        await _jsRuntime.InvokeVoidAsync("setCookie", key, value, expires);
    }

    public async Task<string> GetCookieAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("getCookie", key);
    }
}