using Microsoft.AspNetCore.Http;

public class SessionStateService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionStateService(IHttpContextAccessor accessor)
    {
        _httpContextAccessor = accessor;
    }

    public string GetValue(string key) =>
        _httpContextAccessor.HttpContext?.Session?.GetString(key);
}
