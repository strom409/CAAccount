using System.Threading.Tasks;

public interface IStorageService
{
    Task SetSessionItemAsync(string key, string value);
    Task<string> GetSessionItemAsync(string key);
}
