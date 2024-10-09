using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Security;
using Base.Security.Sha256M;
using Microsoft.Extensions.Options;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;

namespace SquirrelsBox.Authentication.Services
{
    public class UserDataService : IGenericService<UserData, UserDataResponse>
    {
        private readonly IGenericRepository<UserData> _repository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IOptions<JwtKeys> _jwtAccess;

        public UserDataService(IGenericRepository<UserData> repository, IUnitOfWork<AppDbContext> unitOfWork, IOptions<JwtKeys> jwtAccess)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _jwtAccess = jwtAccess;
        }

        public async Task<UserDataResponse> DeleteAsync(int id, string token = null)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new UserDataResponse("UserData not found");

            try
            {
                await _repository.DeleteAsync(result);
                await _unitOfWork.CompleteAsync();

                return new UserDataResponse(result);
            }
            catch (Exception e)
            {
                return new UserDataResponse($"An error occurred while deleting the userData: {e.Message}");
            }
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public async Task<UserDataResponse> FindByCodeAsync(string value)
        {
            try
            {
                value = JwtTokenGenerator.GetUserCodeFromToken(value, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);

                var result = await _repository.FindByCodeAsync(value);
                await _unitOfWork.CompleteAsync();

                return new UserDataResponse(result);
            }
            catch (Exception e)
            {
                return new UserDataResponse($"UserData not found: {e.Message}");
            }
        }

        public Task<UserDataResponse> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public async Task<UserDataResponse> SaveAsync(UserData model)
        {
            try
            {
                model.UserCode = JwtTokenGenerator.GetUserCodeFromToken(model.UserCode, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);
                model.CreationDate = DateTime.UtcNow;
                model.LastUpdateDate = null;

                await _repository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return new UserDataResponse(model);
            }
            catch (Exception e)
            {
                return new UserDataResponse($"An error ocurred while saving the userData: {e.Message}");
            }
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public async Task<UserDataResponse> UpdateAsync(int id, UserData model)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new UserDataResponse("UserData not found");

            try
            {
                result.LastUpdateDate = DateTime.UtcNow;

                _repository.Update(result);
                await _unitOfWork.CompleteAsync();

                return new UserDataResponse(result);
            }
            catch (Exception e)
            {
                return new UserDataResponse($"An error occurred while updating the user data: {e.Message}");
            }
        }
    }
}
