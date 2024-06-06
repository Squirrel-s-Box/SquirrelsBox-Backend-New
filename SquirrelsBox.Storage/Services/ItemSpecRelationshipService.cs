using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Services
{
    public class ItemSpecRelationshipService : IGenericServiceWithMassive<Spec, ItemSpecRelationshipResponse>, IGenericReadService<Spec, ItemSpecRelationshipResponse>
    {
        private readonly IGenericRepositoryWithMassive<Spec> _repository;
        private readonly IGenericReadRepository<Spec> _readRepository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public ItemSpecRelationshipService(IGenericRepositoryWithMassive<Spec> repository, IGenericReadRepository<Spec> readRepository, IUnitOfWork<AppDbContext> unitOfWork)
        {
            _repository = repository;
            _readRepository = readRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ItemSpecRelationshipResponse> DeleteAsync(int id)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new ItemSpecRelationshipResponse("Spec not found");

            try
            {
                await _repository.DeleteAsync(result);
                await _unitOfWork.CompleteAsync();

                return new ItemSpecRelationshipResponse(result);
            }
            catch (Exception e)
            {
                return new ItemSpecRelationshipResponse($"An error occurred while deleting the Spec: {e.Message}");
            }
        }

        public Task<ItemSpecRelationshipResponse> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemSpecRelationshipResponse> FindByIdAsync(int id)
        {
            try
            {
                var result = await _repository.FindByIdAsync(id);
                await _unitOfWork.CompleteAsync();

                return new ItemSpecRelationshipResponse(result);
            }
            catch (Exception e)
            {
                return new ItemSpecRelationshipResponse($"Spec not found: {e.Message}");
            }
        }

        public async Task<IEnumerable<ItemSpecRelationshipResponse>> ListAllByIdCodeAsync(int id)
        {
            var results = await _readRepository.ListAllByIdAsync(id);
            var response = results.Select(result => new ItemSpecRelationshipResponse(result));
            return response;
        }

        public Task<IEnumerable<ItemSpecRelationshipResponse>> ListAllByUserCodeAsync(string userCode)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemSpecRelationshipResponse> SaveAsync(Spec model)
        {
            throw new NotImplementedException("Use SaveMassiveAsync method instead.");
        }

        public async Task<ItemSpecRelationshipResponse> SaveMassiveAsync(ICollection<Spec> modelList)
        {
            try
            {
                foreach (var model in modelList)
                {
                    model.Active = true;
                    model.CreationDate = DateTime.UtcNow;
                    model.LastUpdateDate = null;
                }
                await _repository.AddMassiveAsync(modelList);
                await _unitOfWork.CompleteAsync();
                return new ItemSpecRelationshipResponse(modelList);
            }
            catch (Exception e)
            {
                return new ItemSpecRelationshipResponse($"An error occurred while saving the Specs: {e.Message}");
            }
        }

        public async Task<ItemSpecRelationshipResponse> UpdateAsync(int id, Spec model)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new ItemSpecRelationshipResponse("Spec not found");

            try
            {
                var lastItemId = result.ItemId;

                result.HeaderName = model.HeaderName;
                result.Value = model.Value;
                result.ValueType = model.ValueType;
                result.Active = model.Active;
                result.LastUpdateDate = DateTime.UtcNow;

                _repository.Update(result);

                result.ItemId = lastItemId;

                await _unitOfWork.CompleteAsync();

                return new ItemSpecRelationshipResponse(result);
            }
            catch (Exception e)
            {
                return new ItemSpecRelationshipResponse($"An error occurred while updating the Spec  : {e.Message}");
            }
        }
    }
}
