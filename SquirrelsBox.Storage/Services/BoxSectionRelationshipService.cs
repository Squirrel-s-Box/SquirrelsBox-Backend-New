using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Services
{
    public class BoxSectionRelationshipService : IGenericServiceWithCascade<BoxSectionRelationship, BoxSectionRelationshipResponse>, IGenericReadService<BoxSectionRelationship, BoxSectionRelationshipResponse>
    {
        private readonly IGenericRepositoryWithCascade<BoxSectionRelationship> _repository;
        private readonly IGenericReadRepository<BoxSectionRelationship> _readRepository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public BoxSectionRelationshipService(IGenericRepositoryWithCascade<BoxSectionRelationship> repository, IGenericReadRepository<BoxSectionRelationship> readRepository, IUnitOfWork<AppDbContext> unitOfWork)
        {
            _repository = repository;
            _readRepository = readRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BoxSectionRelationshipResponse> DeleteAsync(int id)
        {
            throw new NotImplementedException("Use DeleteCascade method instead.");
        }

        public async Task<BoxSectionRelationshipResponse> DeleteCascadeAsync(int id, bool cascade)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new BoxSectionRelationshipResponse("Section not found");

            try
            {
                await _repository.DeleteCascadeAsync(result, cascade);
                await _unitOfWork.CompleteAsync();

                return new BoxSectionRelationshipResponse(result);
            }
            catch (Exception e)
            {
                return new BoxSectionRelationshipResponse($"An error occurred while deleting the Box: {e.Message}");
            }
        }

        public Task<BoxSectionRelationshipResponse> FindByCodeAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<BoxSectionRelationshipResponse> FindByIdAsync(int id)
        {
            try
            {
                var result = await _repository.FindByIdAsync(id);
                await _unitOfWork.CompleteAsync();

                return new BoxSectionRelationshipResponse(result);
            }
            catch (Exception e)
            {
                return new BoxSectionRelationshipResponse($"Session not found: {e.Message}");
            }
        }

        public async Task<IEnumerable<BoxSectionRelationshipResponse>> ListAllByIdCodeAsync(int id)
        {
            var results = await _readRepository.ListAllByIdAsync(id);
            var response = results.Select(result => new BoxSectionRelationshipResponse(result));
            return response;
        }

        public Task<IEnumerable<BoxSectionRelationshipResponse>> ListAllByUserCodeAsync(string userCode)
        {
            throw new NotImplementedException();
        }

        public async Task<BoxSectionRelationshipResponse> SaveAsync(BoxSectionRelationship model)
        {
            try
            {
                model.Section.CreationDate = DateTime.UtcNow;
                model.Section.LastUpdateDate = null;

                model.Section.Active = true;

                await _repository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return new BoxSectionRelationshipResponse(model);
            }
            catch (Exception e)
            {
                return new BoxSectionRelationshipResponse($"An error ocurred while saving the Section: {e.Message}");
            }
        }

        public async Task<BoxSectionRelationshipResponse> UpdateAsync(int id, BoxSectionRelationship model)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new BoxSectionRelationshipResponse("Section not found");

            try
            {
                if (model.BoxId != 0)
                {
                    // Update BoxId if provided
                    result.BoxId = model.BoxId;
                }

                if (model.Section != null)
                {
                    // Update Section properties if provided
                    result.Section.Name = model.Section.Name;
                    result.Section.Color = model.Section.Color;
                    result.Section.Active = model.Section.Active;
                    result.Section.LastUpdateDate = DateTime.UtcNow;
                }

                // Update the entity
                _repository.Update(result);
                await _unitOfWork.CompleteAsync();

                return new BoxSectionRelationshipResponse(result);
            }
            catch (Exception e)
            {
                return new BoxSectionRelationshipResponse($"An error occurred while updating the Section: {e.Message}");
            }
        }

    }
}
