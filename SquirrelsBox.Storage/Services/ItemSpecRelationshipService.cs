using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Services
{
    public class ItemSpecRelationshipService : IGenericService<ItemSpecRelationship, ItemSpecRelationshipResponse>, IGenericReadService<ItemSpecRelationship, ItemSpecRelationshipResponse>
    {
        private readonly IGenericRepository<ItemSpecRelationship> _repository;
        private readonly IGenericReadRepository<ItemSpecRelationship> _readRepository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public ItemSpecRelationshipService(IGenericRepository<ItemSpecRelationship> repository, IGenericReadRepository<ItemSpecRelationship> readRepository, IUnitOfWork<AppDbContext> unitOfWork)
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
                _repository.Delete(result);
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

        public async Task<ItemSpecRelationshipResponse> SaveAsync(ItemSpecRelationship model)
        {
            try
            {
                model.Spec.Active = true;

                model.Spec.CreationDate = DateTime.UtcNow;
                model.Spec.LastUpdateDate = null;

                await _repository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return new ItemSpecRelationshipResponse(model);
            }
            catch (Exception e)
            {
                return new ItemSpecRelationshipResponse($"An error ocurred while saving the Spec: {e.Message}");
            }
        }

        public async Task<ItemSpecRelationshipResponse> UpdateAsync(int id, ItemSpecRelationship model)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new ItemSpecRelationshipResponse("Spec not found");

            try
            {
                if (model.ItemId != 0)
                {
                    //It works as th enew Box Id
                    result.Spec.Id = model.ItemId;
                    _repository.Update(result);

                    return new ItemSpecRelationshipResponse(result);
                }
                else
                {
                    var lastItemId = result.ItemId;
                    var lastSpecId = result.SpecId;

                    result.ItemId = 0;
                    result.SpecId = 0;
                    result.Spec.HeaderName = model.Spec.HeaderName;
                    result.Spec.Value = model.Spec.Value;
                    result.Spec.ValueType = model.Spec.ValueType;
                    result.Spec.Active = model.Spec.Active;
                    result.Spec.LastUpdateDate = DateTime.UtcNow;

                    _repository.Update(result);

                    result.ItemId = lastItemId;
                    result.SpecId = lastSpecId;

                    await _unitOfWork.CompleteAsync();

                    return new ItemSpecRelationshipResponse(result);
                }
            }
            catch (Exception e)
            {
                return new ItemSpecRelationshipResponse($"An error occurred while updating the Spec  : {e.Message}");
            }
        }
    }
}
