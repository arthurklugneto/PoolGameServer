namespace PoolServer.Services
{
    public interface ICryptographyService
    {
        string GenerateSHA256String(string inputString);
        string GenerateSHA512String(string inputString);        
    }
}