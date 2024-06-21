using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Services
{
    public class SharedBoxService : IGenericService<SharedBox, SharedBoxResponse>, IGenericReadService<SharedBox, SharedBoxResponse>
    {
        private readonly IGenericRepository<SharedBox> _repository;
        private readonly IGenericReadRepository<SharedBox> _readRepository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public SharedBoxService(IGenericRepository<SharedBox> repository, IGenericReadRepository<SharedBox> readRepository, IUnitOfWork<AppDbContext> unitOfWork)
        {
            _repository = repository;
            _readRepository = readRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SharedBoxResponse> DeleteAsync(int id)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new SharedBoxResponse("SharedBox not found");

            try
            {
                await _repository.DeleteAsync(result);
                await _unitOfWork.CompleteAsync();

                return new SharedBoxResponse(result);
            }
            catch (Exception e)
            {
                return new SharedBoxResponse($"An error occurred while deleting the SharedBox: {e.Message}");
            }
        }

        public Task<SharedBoxResponse> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public Task<SharedBoxResponse> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SharedBoxResponse>> ListAllByIdCodeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SharedBoxResponse>> ListAllByUserCodeAsync(string userCode)
        {
            var results = await _readRepository.ListAllByUserCodeAsync(userCode);
            var response = results.Select(result => new SharedBoxResponse(result));
            return response;
        }

        public async Task<SharedBoxResponse> SaveAsync(SharedBox model)
        {
            try
            {
                model.State = true;
                model.CreationDate = DateTime.UtcNow;
                model.LastUpdateDate = null;

                await _repository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return new SharedBoxResponse(model);
            }
            catch (Exception e)
            {
                return new SharedBoxResponse($"An error ocurred while saving the SharedBox: {e.Message}");
            }
        }

        public async Task<SharedBoxResponse> UpdateAsync(int id, SharedBox model)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new SharedBoxResponse("SharedBox not found");

            try
            {
                result.LastUpdateDate = DateTime.UtcNow;

                _repository.Update(result);
                await _unitOfWork.CompleteAsync();

                return new SharedBoxResponse(result);
            }
            catch (Exception e)
            {
                return new SharedBoxResponse($"An error occurred while updating the SharedBox: {e.Message}");
            }
        }
    }
}
