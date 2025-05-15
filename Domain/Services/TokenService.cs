using Domain.Interfaces.Services;

namespace Domain.Services
{
    public class TokenService : ITokenService
    {
        public bool TokenValido(string token)
        {
            return true;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
