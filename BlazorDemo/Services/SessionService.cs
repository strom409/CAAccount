// Services/SessionService.cs
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class SessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserName
    {
        get => _httpContextAccessor.HttpContext.Session.GetString("UserName");
        set => _httpContextAccessor.HttpContext.Session.SetString("UserName", value);
    }

    public string UnitName
    {
        get => _httpContextAccessor.HttpContext.Session.GetString("UnitName");
        set => _httpContextAccessor.HttpContext.Session.SetString("UnitName", value);
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(UserName);

    public async Task Login(string userName, string unitName)
    {
        UserName = userName;
        UnitName = unitName;
        await _httpContextAccessor.HttpContext.Session.CommitAsync();
    }

    public async Task Logout()
    {
        _httpContextAccessor.HttpContext.Session.Remove("UserName");
        _httpContextAccessor.HttpContext.Session.Remove("UnitName");
        await _httpContextAccessor.HttpContext.Session.CommitAsync();
    }
}