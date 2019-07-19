using MongoDB.Bson;
using MongoDB.Driver;
using PoolGameServer.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolGameServer.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<TEntity> DbSet;

        protected BaseRepository(IMongoContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity obj)
        {
            _context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual async Task Update(TEntity obj,string id)
        {
            await DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), obj);
        }

        public virtual async Task Remove(string id)
        {
            await DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}