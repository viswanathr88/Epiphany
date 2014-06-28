﻿using Epiphany.Model.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epiphany.Model.Services
{
    class CachedAuthorService : IAuthorService
    {
        private readonly IDictionary<int, AuthorModel> cache;
        private readonly IMessenger messenger;
        private readonly IAuthorService service;

        public CachedAuthorService(IAuthorService authorService, IMessenger messenger)
        {
            this.service = authorService;
            this.messenger = messenger;
            this.cache = new Dictionary<int, AuthorModel>();
        }

        public async Task<AuthorModel> GetAuthorAsync(int id)
        {
            if (cache.ContainsKey(id))
            {
                return cache[id];
            }
            else
            {
                return await this.service.GetAuthorAsync(id);
            }
        }

        public async Task FlipFanshipAsync(AuthorModel author)
        {
            await this.service.FlipFanshipAsync(author);
        }
    }
}