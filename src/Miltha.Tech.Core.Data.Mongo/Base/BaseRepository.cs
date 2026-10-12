using System.Linq.Expressions;
using Miltha.Tech.Core.Pagination;
using Miltha.Tech.Core.Data.Mongo.Documents;
using Miltha.Tech.Core.Data.Mongo.Extensions;
using MongoDB.Driver;

namespace Miltha.Tech.Core.Data.Mongo.Base
{
    public abstract class BaseRepository<TDocument> where TDocument : DocumentBase
    {
        protected readonly IMongoCollection<TDocument> _collection;

        protected BaseRepository(IMongoDatabase database, string? collectionName = null)
        {
            var name = string.IsNullOrWhiteSpace(collectionName)
                ? GetCollectionName(typeof(TDocument))
                : collectionName;

            _collection = database.GetCollection<TDocument>(name);
        }

        protected virtual FilterDefinition<TDocument> BaseFilter =>
            Builders<TDocument>.Filter.Eq(x => x.IsActive, true);

        public virtual async Task AddAsync(TDocument document)
        {
            PrepareForInsert(document);

            await _collection.InsertOneAsync(document);
        }

        public virtual async Task AddAsync(IEnumerable<TDocument> documents)
        {
            foreach (var doc in documents)
                PrepareForInsert(doc);

            await _collection.InsertManyAsync(documents);
        }

        public virtual async Task<long> CountAsync() =>
            await _collection.CountDocumentsAsync(BaseFilter);

        public virtual async Task<long> CountAsync(Expression<Func<TDocument, bool>> predicate) =>
            await _collection.CountDocumentsAsync(ComposeFilter(predicate));

        public virtual async Task DeleteAsync(Expression<Func<TDocument, bool>> predicate) =>
          await _collection.DeleteOneAsync(ComposeFilter(predicate));

        public virtual async Task DisableAsync(Guid id)
        {
            var filter = Builders<TDocument>.Filter.And(
                BaseFilter,
                Builders<TDocument>.Filter.Eq(x => x.Id, id));

            var update = Builders<TDocument>.Update.Set(x => x.IsActive, false);

            await _collection.UpdateOneAsync(filter, update);
        }

        public virtual async Task<TDocument?> GetAsync(Guid id) =>
            await _collection.Find(
                Builders<TDocument>.Filter.And(
                    BaseFilter,
                    Builders<TDocument>.Filter.Eq(x => x.Id, id)))
            .SingleOrDefaultAsync();

        public virtual async Task<TDocument?> GetAsync(Expression<Func<TDocument, bool>>? predicate = null) =>
            await _collection.Find(ComposeFilter(predicate))
            .SingleOrDefaultAsync();

        public virtual async Task ReplaceAsync(TDocument document)
        {
            var expected = document.ConcurrencyStamp;
            document.ConcurrencyStamp = Guid.NewGuid().ToString();

            var filter = Builders<TDocument>.Filter.And(
                BaseFilter,
                Builders<TDocument>.Filter.Eq(x => x.Id, document.Id),
                Builders<TDocument>.Filter.Eq(x => x.ConcurrencyStamp, expected)
            );

            var result = await _collection.ReplaceOneAsync(
                filter,
                document,
                new ReplaceOptions { IsUpsert = false });

            if (result.ModifiedCount == 0)
                throw new ConcurrencyConflictException(document.Id, expected);
        }

        public virtual async Task<IReadOnlyList<TDocument>> SelectAsync(PaginationFilter paginationFilter) =>
           await SelectInternalAsync(BaseFilter, paginationFilter);

        public virtual async Task<IReadOnlyList<TDocument>> SelectAsync(Expression<Func<TDocument, bool>> predicate, PaginationFilter paginationFilter) =>
            await SelectInternalAsync(ComposeFilter(predicate), paginationFilter);

        protected static string GetCollectionName(Type documentType) =>
            ((BsonCollectionAttribute)documentType
                .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .First()).CollectionName;

        protected static (int skip, int limit) Page(PaginationFilter paginationFilter, int maxPageSize = 200)
        {
            var limit = Math.Clamp(paginationFilter.PageSize, 1, maxPageSize);
            var page = Math.Max(1, paginationFilter.PageNumber);
            return ((page - 1) * limit, limit);
        }

        protected FilterDefinition<TDocument> ComposeFilter(Expression<Func<TDocument, bool>>? predicate = null)
        {
            return predicate == null
                ? BaseFilter
                : Builders<TDocument>.Filter.And(
                    BaseFilter,
                    Builders<TDocument>.Filter.Where(predicate));
        }

        protected virtual void PrepareForInsert(TDocument document)
        {
            document.CreatedOn = document.CreatedOn == default
                ? DateTime.UtcNow
                : document.CreatedOn;

            document.ConcurrencyStamp ??= Guid.NewGuid().ToString();
            document.IsActive = true;
        }

        protected virtual async Task<IReadOnlyList<TDocument>> SelectInternalAsync(FilterDefinition<TDocument> filter, PaginationFilter paginationFilter)
        {
            var (skip, limit) = Page(paginationFilter);

            return await _collection.Find(filter)
                .SortByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.Id)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }
    }
}