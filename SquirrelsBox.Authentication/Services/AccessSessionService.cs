using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Security;
using Base.Security.JwtConfig;
using Base.Security.Sha256M;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Interfaces;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;
using SquirrelsBox.Authentication.Persistence.Repositories;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SquirrelsBox.Authentication.Services
{
    public class AccessSessionService : IGenericService<AccessSession, AccessSessionResponse>, IAccessSessionService
    {
        private readonly IGenericRepository<AccessSession> _repository;
        private readonly IAccessSessionRepository _accessSesionRepository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IOptions<Sha256Constantes> _encryptionSettings;
        private readonly IOptions<JwtKeys> _jwtAccess;

        public AccessSessionService(IGenericRepository<AccessSession> repository, IAccessSessionRepository accessSesionRepository, IUnitOfWork<AppDbContext> unitOfWork, IOptions<Sha256Constantes> encryptionSettings, IOptions<JwtKeys> jwtAccess)
        {
            _repository = repository;
            _accessSesionRepository = accessSesionRepository;
            _unitOfWork = unitOfWork;
            _encryptionSettings = encryptionSettings;
            _jwtAccess = jwtAccess;
        }

        public async Task<AccessSessionResponse> DeleteAsync(int id)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new AccessSessionResponse("Session not found");

            try
            {
                await _repository.DeleteAsync(result);
                await _unitOfWork.CompleteAsync();

                return new AccessSessionResponse(result);
            }
            catch (Exception e)
            {
                return new AccessSessionResponse($"An error occurred while deleting the Session: {e.Message}");
            }
        }

        public async Task<AccessSessionResponse> FindByCodeAsync(string value)
        {
            var result = await _repository.FindByCodeAsync(value);

            if (result == null)
                return new AccessSessionResponse("Session not found");

            try
            {
                await _unitOfWork.CompleteAsync();

                return new AccessSessionResponse(result);
            }
            catch (Exception e)
            {
                return new AccessSessionResponse($"Session not found: {e.Message}");
            }
        }

        public async Task<AccessSessionResponse> FindByIdAsync(int id)
        {
            try
            {
                var result = await _repository.FindByIdAsync(id);
                await _unitOfWork.CompleteAsync();

                return new AccessSessionResponse(result);
            }
            catch (Exception e)
            {
                return new AccessSessionResponse($"Session not found: {e.Message}");
            }
        }

        public async Task<AccessSessionResponse> LogIn(AccessSession model)
        {
            try
            {
                var result = await _accessSesionRepository.LogIn(model);
                var newRefreshToken = JwtTokenGenerator.CreateRefreshToken();
                var refreshTokenVerificationResult = await _accessSesionRepository.VerifyAndReplaceRefreshTokenAsync(result.RefreshToken, newRefreshToken);

                if (!refreshTokenVerificationResult)
                {
                    throw new InvalidOperationException("Invalid refresh token.");
                }

                JwtAccess jwtAccess = new JwtAccess
                {
                    UserCode = result.Code,
                    Role = UserRole.User
                };

                var newToken = JwtTokenGenerator.CreateToken(jwtAccess, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);
                await _unitOfWork.CompleteAsync();

                return new AccessSessionResponse(result, newToken);
            }
            catch (Exception e)
            {
                return new AccessSessionResponse($"Session not found: {e.Message}");
            }
        }

        public async Task<AccessSessionResponse> SaveAsync(AccessSession model)
        {
            try
            {
                //AccessSession verification;

                model.CreationDate = DateTime.UtcNow;
                model.LastUpdateDate = null;
                model.Code = Guid.NewGuid().ToString();
                model.Code = AESEncDec.AESEncryption(model.Code, _encryptionSettings.Value.Key, _encryptionSettings.Value.IV);
                //do
                //{
                //    model.Code = Guid.NewGuid().ToString();
                //    verification = await _repository.FindByCodeAsync(model.Code);
                //} while (verification != null);

                var refreshToken = JwtTokenGenerator.CreateRefreshToken();
                model.RefreshToken = refreshToken;

                await _repository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                JwtAccess jwtAccess = new JwtAccess
                {
                    UserCode = model.Code,
                    Role = UserRole.User
                };

                model.Code = AESEncDec.AESDecryption(model.Code, _encryptionSettings.Value.Key, _encryptionSettings.Value.IV);
                var token = JwtTokenGenerator.CreateToken(jwtAccess, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);

                return new AccessSessionResponse(model, token);
            }
            catch (Exception e)
            {
                return new AccessSessionResponse($"An error ocurred while saving the userData: {e.Message}");
            }
        }

        public async Task<AccessSessionResponse> UpdateAsync(int id, AccessSession model)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new AccessSessionResponse("Session not found");

            try
            {
                if (result.Attempt == 3 &&
                    (DateTime.UtcNow - result.LastUpdateDate) >= TimeSpan.FromMinutes(30))
                {
                    result.Attempt = 0;
                    result.LastUpdateDate = DateTime.UtcNow;
                }
                else if (result.Attempt < 3)
                {
                    result.Attempt += 1;
                    result.LastUpdateDate = DateTime.UtcNow;
                }
                else
                {
                    return new AccessSessionResponse("You have tried too much, wait 30 mins and try again else check your password");
                }

                _repository.Update(result);
                await _unitOfWork.CompleteAsync();

                return new AccessSessionResponse(result);
            }
            catch (Exception e)
            {
                return new AccessSessionResponse($"An error occurred while updating the user data: {e.Message}");
            }
        }

        public async Task<AccessTokenResponse> VerifyRefreshToken(string refreshToken, string code)
        {
            var newRefreshToken = JwtTokenGenerator.CreateRefreshToken();

            var refreshTokenVerificationResult = await _accessSesionRepository.VerifyAndReplaceRefreshTokenAsync(refreshToken, newRefreshToken);

            if (!refreshTokenVerificationResult)
            {
                throw new InvalidOperationException("Invalid refresh token.");
            }

            JwtAccess jwtAccess = new JwtAccess
            {
                UserCode = code,
                Role = UserRole.User
            };

            var newToken = JwtTokenGenerator.CreateToken(jwtAccess, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);
            await _unitOfWork.CompleteAsync();

            return new AccessTokenResponse(newToken, newRefreshToken);
        }
    }
}
