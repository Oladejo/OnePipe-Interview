using MongoDB.Bson;
using MongoDB.Driver;
using OnePipe.Core.DatabaseConnection;
using OnePipe.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpePipe.Data.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoCollection<TEntity> Context;

        public Repository(IOnePipeDatabaseSetting settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Context = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }
            await Context.InsertOneAsync(entity);
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            if (!entities.Any())
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }
            await Context.InsertManyAsync(entities);
        }

        public void Delete(string id)
        {
            var objectId = new ObjectId(id);
            Context.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            var all = Context.FindSync(Builders<TEntity>.Filter.Empty);
            return await all.ToListAsync();
        }

        public async Task<TEntity> GetAsync(string id)
        {
            var objectId = new ObjectId(id);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            return await Context.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public void Update(TEntity obj)
        {
            Context.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj), obj);
        }
    }
}
