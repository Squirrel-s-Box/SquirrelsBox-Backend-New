using Base.Security.JwtConfig;
using Microsoft.AspNetCore.Mvc;

namespace SquirrelsBox.Authentication.Domain.Interfaces
{
    public interface IAccessSessionService
    {
        Task<AccessTokenResponse> VerifyRefreshToken(string refreshToken, string code);
    }
}
