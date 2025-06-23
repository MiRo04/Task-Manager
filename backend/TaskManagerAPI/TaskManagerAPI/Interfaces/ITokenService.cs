using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Users user);
    }
}
