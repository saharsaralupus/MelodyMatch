using System.Threading.Tasks;

namespace Orquesta.WEB.Services
{
    public interface ILoginService
    {
        Task LoginAsync(string token);

        Task LogoutAsync();
    }
}