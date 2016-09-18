using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public interface ILoginService
    {
        Task<LoginResult> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
    }
}
