using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Security.Sha256M;
using Microsoft.Extensions.Options;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Services
{
    public class SpecService : IGenericServiceWithMassive<Spec, ItemSpecRelationshipResponse>, IGenericReadService<Spec, ItemSpecRelationshipResponse>
    {
        private readonly IGenericRepositoryWithMassive<Spec> _repository;
        private readonly IGenericReadRepository<Spec> _readRepository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IOptions<JwtKeys> _jwtAccess;

        public SpecService(IGenericRepositoryWithMassive<Spec> repository, IGenericReadRepository<Spec> readRepository, IUnitOfWork<AppDbContext> unitOfWork, IOptions<JwtKeys> jwtAccess)
        {
            _repository = repository;
            _readRepository = readRepository;
            _unitOfWork = unitOfWork;
            _jwtAccess = jwtAccess;
        }

        public async Task<ItemSpecRelationshipResponse> DeleteteMassiveAsync(ICollection<int> ids, string token)
        {
            try
            {
                var userCode = JwtTokenGenerator.GetUserCodeFromToken(token, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);
                await _repository.DeleteteMassiveAsync(ids, userCode);
                await _unitOfWork.CompleteAsync();

                return new ItemSpecRelationshipResponse(ids);
            }
            catch (Exception e)
            {
                return new ItemSpecRelationshipResponse($"An error occurred while deleting the Spec: {e.Message}");
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

        public async Task<ItemSpecRelationshipResponse> SaveMassiveAsync(ICollection<Spec> modelList)
        {
            try
            {
                foreach (var model in modelList)
                {
                    var userCode = JwtTokenGenerator.GetUserCodeFromToken(model.UserCodeLog, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);
                    model.UserCodeLog = userCode;
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

        public async Task<ItemSpecRelationshipResponse> UpdateMassiveAsync(ICollection<Spec> modelList)
        {
            try
            {
                foreach (var model in modelList)
                {
                    var userCode = JwtTokenGenerator.GetUserCodeFromToken(model.UserCodeLog, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);
                    model.UserCodeLog = userCode;
                    model.LastUpdateDate = DateTime.UtcNow;
                }

                await _repository.UpdateMassiveAsync(modelList);

                await _unitOfWork.CompleteAsync();

                return new ItemSpecRelationshipResponse(modelList);
            }
            catch (Exception e)
            {
                return new ItemSpecRelationshipResponse($"An error occurred while updating the Spec  : {e.Message}");
            }
        }
    }
}
