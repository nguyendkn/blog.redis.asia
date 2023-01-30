﻿using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Humanizer;
using MongoDB.Bson;
using MongoDB.Driver;
using Package.Mongo.Abstracts;
using Package.Mongo.Attributes;

namespace Package.Mongo;

public class MongoSet<TEntity> : IQueryable<TEntity>, IMongoSet<TEntity>
    where TEntity : MongoDocument
{
    private readonly IMongoDatabase _database;
    private readonly string _collectionName;
    private readonly IMongoCollection<TEntity> _collection;

    public MongoSet(IMongoDatabase database)
    {
        _database = database;
        _collectionName = typeof(TEntity).Name.Pluralize();
        _collection = database.GetCollection<TEntity>(_collectionName);
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        => _collection.AsQueryable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _collection.AsQueryable().GetEnumerator();

    Type IQueryable.ElementType
        => _collection.AsQueryable().ElementType;

    Expression IQueryable.Expression
        => _collection.AsQueryable().Expression;

    IQueryProvider IQueryable.Provider
        => _collection.AsQueryable().Provider;

    public IMongoCollection<TEntity> Collection(string connectionString, string database)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        var client = new MongoClient(settings);
        return client.GetDatabase(database).GetCollection<TEntity>(_collectionName);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _collection.InsertManyAsync(entities);
    }

    public async Task<bool> UpdateAsync(object id, TEntity entity)
    {
        var record = await _collection.ReplaceOneAsync(x => x!.Id.Equals(id), entity);
        return record.IsAcknowledged;
    }

    public async Task<bool> UpdateAsync(List<TEntity> entities)
    {
        try
        {
            foreach (var entity in entities)
            {
                var id = entity.Id;
                var record = await _collection.ReplaceOneAsync(x => x!.Id.Equals(id), entity);
                return record.IsAcknowledged;
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> RemoveAsync(object id)
    {
        var deleteResult = await _collection.DeleteOneAsync(x => x!.Id.Equals(id));
        return deleteResult.IsAcknowledged;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
    {
        var document = await _collection.FindAsync(expression);
        return await document.FirstOrDefaultAsync();
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CountOptions? options = null)
    {
        return await _collection.CountDocumentsAsync(expression, options);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        return (await CountAsync(expression) > 0);
    }

    public async Task<TEntity> FindAsync(object key)
    {
        if (key.Equals(Guid.Empty)) return default!;
        var document = await _collection.FindAsync(x => x!.Id.Equals(key));
        return await document.FirstOrDefaultAsync();
    }
    
    public async Task<List<TEntity>> ToListAsync()
    {
        var document = await Where(x => true).ToListAsync();
        return document;
    }

    public IFindFluent<TEntity, TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        return Where(expression, string.Empty);
    }

    public IFindFluent<TEntity, TEntity> Where(Expression<Func<TEntity, bool>> expression, string? keyword)
    {
        var builder = Builders<TEntity>.Filter;
        var text = Builders<TEntity>.Filter.Text(keyword ?? string.Empty);
        var where = Builders<TEntity>.Filter.Where(expression);
        if (string.IsNullOrEmpty(keyword)) return _collection.Find(where);
        var filters = builder.And(text, where);
        var fluent = _collection.Find(filters);
        return fluent;
    }

    public IAggregateFluent<BsonDocument> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        var targetProperty = typeof(TProperty).Name;
        var targetCollection = targetProperty.Pluralize();
        var localFieldProperty = typeof(TEntity).GetProperties()
            .FirstOrDefault(x =>
                x.GetCustomAttribute<MongoLocalFieldAttribute>() != null);
        var localField = localFieldProperty?.GetCustomAttribute<MongoLocalFieldAttribute>()?.LocalField;
        var foreignField = typeof(TProperty).GetProperties()
            .FirstOrDefault(x =>
                x.GetCustomAttribute<MongoKeyAttribute>() != null)?.Name;
        return _collection.Aggregate().Lookup(targetCollection, localField, foreignField, localFieldProperty?.Name);
    }
}