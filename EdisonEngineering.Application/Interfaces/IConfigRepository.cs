public interface IConfigRepository
{
    Task<Dictionary<string, string>> GetAllAsync();
}