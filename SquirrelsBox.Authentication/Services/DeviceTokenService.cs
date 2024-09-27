using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Interfaces;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;

namespace SquirrelsBox.Authentication.Services
{
    public class DeviceTokenService : IGenericService<DeviceSession, DeviceSessionResponse>
    {
        private readonly IGenericRepository<DeviceSession> _repository;
        private readonly IDeviceSessionRepository _deviceSessionRepository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public DeviceTokenService(IGenericRepository<DeviceSession> repository, IDeviceSessionRepository deviceToknRepository, IUnitOfWork<AppDbContext> unitOfWork)
        {
            _repository = repository;
            _deviceSessionRepository = deviceToknRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeviceSessionResponse> DeleteAsync(int id, string token = null)
        {
            var result = await _deviceSessionRepository.GetDeviceSessionByUserIdAsync(id);
            if (result == null)
                return new DeviceSessionResponse("AccessSession not found");

            try
            {
                await _repository.DeleteAsync(result);
                await _unitOfWork.CompleteAsync();

                return new DeviceSessionResponse(result);
            }
            catch (Exception e)
            {
                return new DeviceSessionResponse($"An error occurred while deleting the Device Token: {e.Message}");
            }
        }

        public Task<DeviceSessionResponse> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<DeviceSessionResponse> FindByIdAsync(int id)
        {
            try
            {
                var result = await _repository.FindByIdAsync(id);
                await _unitOfWork.CompleteAsync();

                return new DeviceSessionResponse(result);
            }
            catch (Exception e)
            {
                return new DeviceSessionResponse($"Device Token not found: {e.Message}");
            }
        }

        public async Task<DeviceSessionResponse> SaveAsync(DeviceSession model)
        {
            try
            {
                model.CreationDate = DateTime.UtcNow;
                model.LastUpdateDate = null;

                await _repository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return new DeviceSessionResponse(model);
            }
            catch (Exception e)
            {
                return new DeviceSessionResponse($"An error ocurred while saving the Device Token: {e.Message}");
            }
        }

        public async Task<DeviceSessionResponse> UpdateAsync(int id, DeviceSession model)
        {
            var result = await _deviceSessionRepository.GetDeviceSessionByUserIdAsync(id);
            if (result == null)
                return new DeviceSessionResponse("Device Token not found");

            try
            {
                result.Token = model.Token;
                result.LastUpdateDate = DateTime.UtcNow;

                _repository.Update(result);
                await _unitOfWork.CompleteAsync();

                return new DeviceSessionResponse(result);
            }
            catch (Exception e)
            {
                return new DeviceSessionResponse($"An error occurred while updating the Device Token: {e.Message}");
            }
        }
    }
}
