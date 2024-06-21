﻿using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Generic.Persistence.Repositories;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Services
{
    public class ASearchService : IGenericSearchService
    {
        private readonly IGenericSearchRepository _repository;

        public ASearchService(IGenericSearchRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> ListFinderAsync(string text, int type)
        {
            var results = await _repository.ListFinderAsync(text,type);
            return results;
        }
    }
}