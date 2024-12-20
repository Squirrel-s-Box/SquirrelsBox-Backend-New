﻿using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Security.Sha256M;
using Microsoft.Extensions.Options;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;

namespace SquirrelsBox.Storage.Services
{
    public class BoxService : IGenericServiceWithCascade<Box, BoxResponse>, IGenericReadService<Box, BoxResponse>
    {
        private readonly IGenericRepositoryWithCascade<Box> _repository;
        private readonly IGenericReadRepository<Box> _readRepository;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IOptions<JwtKeys> _jwtAccess;

        public BoxService(IGenericRepositoryWithCascade<Box> repository, IGenericReadRepository<Box> readRepository, IUnitOfWork<AppDbContext> unitOfWork, IOptions<JwtKeys> jwtAccess)
        {
            _repository = repository;
            _readRepository = readRepository;
            _unitOfWork = unitOfWork;
            _jwtAccess = jwtAccess;
        }

        public Task<BoxResponse> DeleteAsync(int id, string token = null)
        {
            throw new NotImplementedException();
        }

        public async Task<BoxResponse> DeleteCascadeAsync(int id, string token, bool cascade)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new BoxResponse("Box not found");

            try
            {
                var userCode = JwtTokenGenerator.GetUserCodeFromToken(token, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);
                await _repository.DeleteCascadeAsync(result, userCode, cascade);
                await _unitOfWork.CompleteAsync();

                return new BoxResponse(result);
            }
            catch (Exception e)
            {
                return new BoxResponse($"An error occurred while deleting the Box: {e.Message}");
            }
        }

        public async Task<BoxResponse> FindByCodeAsync(string value)
        {
            var result = await _repository.FindByCodeAsync(value);

            if (result == null)
                return new BoxResponse("Box not found");

            try
            {
                await _unitOfWork.CompleteAsync();

                return new BoxResponse(result);
            }
            catch (Exception e)
            {
                return new BoxResponse($"Box not found: {e.Message}");
            }
        }

        public async Task<BoxResponse> FindByIdAsync(int id)
        {
            try
            {
                var result = await _repository.FindByIdAsync(id);
                await _unitOfWork.CompleteAsync();

                return new BoxResponse(result);
            }
            catch (Exception e)
            {
                return new BoxResponse($"Box not found: {e.Message}");
            }
        }

        public Task<IEnumerable<BoxResponse>> ListAllByIdCodeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BoxResponse>> ListAllByUserCodeAsync(string userCode)
        {
            var results = await _readRepository.ListAllByUserCodeAsync(userCode);
            var response = results.Select(result => new BoxResponse(result));
            return response;
        }

        public async Task<BoxResponse> SaveAsync(Box model)
        {
            try
            {
                model.UserCodeLog = JwtTokenGenerator.GetUserCodeFromToken(model.UserCodeLog, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);

                model.Favourite = false;
                model.Active = true;

                model.CreationDate = DateTime.UtcNow;
                model.LastUpdateDate = null;

                await _repository.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return new BoxResponse(model);
            }
            catch (Exception e)
            {
                return new BoxResponse($"An error ocurred while saving the Box: {e.Message}");
            }
        }

        public async Task<BoxResponse> UpdateAsync(int id, Box model)
        {
            var result = await _repository.FindByIdAsync(id);
            if (result == null)
                return new BoxResponse("Box not found");

            try
            {
                var userCode = JwtTokenGenerator.GetUserCodeFromToken(model.UserCodeLog, _jwtAccess.Value.Key, _jwtAccess.Value.Issuer, _jwtAccess.Value.Audience);

                result.UserCodeLog = userCode;
                result.Name = model.Name;
                result.Favourite = model.Favourite;
                result.Active = model.Active;
                result.LastUpdateDate = DateTime.UtcNow;

                _repository.Update(result);
                await _unitOfWork.CompleteAsync();

                return new BoxResponse(result);
            }
            catch (Exception e)
            {
                return new BoxResponse($"An error occurred while updating the Box: {e.Message}");
            }
        }
    }
}
