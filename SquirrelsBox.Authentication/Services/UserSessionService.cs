using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Interfaces;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;

namespace SquirrelsBox.Authentication.Services
{
    public class UserSessionService : IGenericService<UserSession, UserSessionResponse>
    {
        private readonly IGenericRepository<UserSession> _repository;
        private readonly IUserSessionRepository _userSessionService;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public UserSessionService(IGenericRepository<UserSession> repository, IUserSessionRepository userSessionService, IUnitOfWork<AppDbContext> unitOfWork)
        {
            _repository = repository;
            _userSessionService = userSessionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserSessionResponse> DeleteAsync(int id)
        {
            var result = await _userSessionService.GetUserSessionByUserIdAsync(id);
            if (result == null)
                return new UserSessionResponse("AccessSession not found");

            try
            {
                _repository.Delete(result);
                await _unitOfWork.CompleteAsync();

                return new UserSessionResponse(result);
            }
            catch (Exception e)
            {
                return new UserSessionResponse($"An error occurred while deleting the AccessSession: {e.Message}");
            }
        }

        public Task<UserSessionResponse> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<UserSessionResponse> FindByIdAsync(int id)
        {
            try
            {
                var result = await _repository.FindByIdAsync(id);
                await _unitOfWork.CompleteAsync();

                return new UserSessionResponse(result);
            }
            catch (Exception e)
            {
                return new UserSessionResponse($"AccessSession not found: {e.Message}");
            }
        }

        public async Task<UserSessionResponse> SaveAsync(UserSession model)
        {
            try
            {
                model.OldToken = model.Token;
                model.CreationDate = DateTime.UtcNow;
                model.LastUpdateDate = null;

                await _repository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return new UserSessionResponse(model);
            }
            catch (Exception e)
            {
                return new UserSessionResponse($"An error ocurred while saving the AccessSession: {e.Message}");
            }
        }

        public async Task<UserSessionResponse> UpdateAsync(int id, UserSession model)
        {
            var result = await _userSessionService.GetUserSessionByUserIdAndOldTokenAsync(id, model.OldToken);
            if (result == null)
                return new UserSessionResponse("AccessSession not found");

            try
            {
                result.OldToken = result.Token;
                result.Token = model.Token;
                result.LastUpdateDate = DateTime.UtcNow;

                _repository.Update(result);
                await _unitOfWork.CompleteAsync();

                return new UserSessionResponse(result);
            }
            catch (Exception e)
            {
                return new UserSessionResponse($"An error occurred while updating the AccessSession AccessSession: {e.Message}");
            }
        }
    }
}
