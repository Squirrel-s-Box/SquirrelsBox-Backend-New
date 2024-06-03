using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;

namespace SquirrelsBox.Authentication.Services
{
    public class UserDataService : IGenericService<UserData, UserDataResponse>
    {
        private readonly IGenericRepository<UserData> _repository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public UserDataService(IGenericRepository<UserData> repository, IUnitOfWork<AppDbContext> unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<UserDataResponse> DeleteAsync(int id)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new UserDataResponse("UserData not found");

            try
            {
                _repository.Delete(result);
                await _unitOfWork.CompleteAsync();

                return new UserDataResponse(result);
            }
            catch (Exception e)
            {
                return new UserDataResponse($"An error occurred while deleting the userData: {e.Message}");
            }
        }

        public async Task<UserDataResponse> FindByCodeAsync(string value)
        {
            try
            {
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

        public async Task<UserDataResponse> SaveAsync(UserData model)
        {
            try
            {
                model.Username = model.Username;
                model.Name = model.Name;
                model.Lastname = model.Name;
                model.Email = model.Name;
                model.Lastname = model.Name;
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
