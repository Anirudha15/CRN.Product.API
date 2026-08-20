using ProductUser = CRN.Product.API.Entities.User;

namespace CRN.Product.API.Auth
{
    public interface IJwtService
    {
        string GenerateAccessToken(ProductUser user);

        string GenerateRefreshToken();
    }
}