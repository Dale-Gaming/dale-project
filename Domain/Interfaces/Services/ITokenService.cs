namespace Domain.Interfaces.Services
{
    public interface ITokenService : IDisposable
    {
        bool TokenValido(string token);
    }
}
