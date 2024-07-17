using Base.Security.JwtConfig;
using Microsoft.AspNetCore.Mvc;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Domain.Interfaces
{
    public interface IAccessSessionService
    {
        Task<AccessTokenResponse> VerifyRefreshToken(string refreshToken, string code);
        Task<AccessSessionResponse> LogIn(AccessSession model);
    }
}
