// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data
{
  using Microsoft.EntityFrameworkCore;

  /// <summary>Provides a simple API to persistence of an entity.</summary>
  public abstract class RepositoryBase<TEntityImpl, TEntity, TIdentity> : IRepository<TEntity, TIdentity>
    where TEntityImpl : EntityBase, TEntity
    where TEntity     : class, TIdentity
    where TIdentity   : class
  {
    /// <summary>Initializes a new instance of the <see cref="RepositoryBase{TEntity, TIdentity}"/> class.</summary>
    /// <param name="dbContext">An object that represents a session with the database and can be used to query and save instances of your entities.</param>
    protected RepositoryBase(DbContext dbContext)
    {
      DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>Gets an object that represents a session with the database and can be used to query and save instances of your entities.</summary>
    protected DbContext DbContext { get; }

    /// <summary>Gets an entity.</summary>
    /// <param name="identity">An object that represents an identity of an entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future.</returns>
    public async Task<TEntity?> GetAsync(TIdentity identity, CancellationToken cancellationToken)
    {
      var id = EntityBase.Create<TIdentity, TEntityImpl>(identity).Id;

      return await DbContext.Set<TEntityImpl>()
                            .AsNoTracking()
                            .Where(entity => entity.Id == id)
                            .SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>Adds an entity.</summary>
    /// <param name="entity">An object that represents an entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future.</returns>
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
      var dbEntity = EntityBase.Create<TEntity, TEntityImpl>(entity);
      var dbEntityEntry = DbContext.Entry(dbEntity);

      dbEntityEntry.State = EntityState.Added;

      foreach (var collectionEntry in dbEntityEntry.Collections)
      {
        if (collectionEntry.CurrentValue != null)
        {
          foreach (var collectionItemEntity in collectionEntry.CurrentValue)
          {
            DbContext.Entry(collectionItemEntity).State = EntityState.Unchanged;
          }
        }
      }

      await DbContext.SaveChangesAsync(cancellationToken);
      dbEntityEntry.State = EntityState.Detached;

      return dbEntity;
    }

    /// <summary>Updates an entity.</summary>
    /// <param name="entity">An object that represents an entity.</param>
    /// <param name="properties">An object that represents a collection of properties to update.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public async Task UpdateAsync(TEntity entity, IEnumerable<string> properties, CancellationToken cancellationToken)
    {
      var dbEntity = EntityBase.Create<TEntity, TEntityImpl>(entity);
      var dbEntityEntry = DbContext.Entry(dbEntity);

      dbEntityEntry.State = EntityState.Unchanged;

      var propertyHash = properties.ToHashSet(StringComparer.OrdinalIgnoreCase);

      foreach (var property in dbEntityEntry.Properties)
      {
        if (propertyHash.Contains(property.Metadata.Name))
        {
          property.IsModified = true;
        }
      }

      await DbContext.SaveChangesAsync(cancellationToken);
      dbEntityEntry.State = EntityState.Detached;
    }

    /// <summary>Deletes an entity.</summary>
    /// <param name="identity">An object that represents an identity of an entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public Task DeleteAsync(TIdentity identity, CancellationToken cancellationToken)
    {
      var id = EntityBase.Create<TIdentity, TEntityImpl>(identity).Id;

      return DbContext.Set<TEntityImpl>()
                      .Where(entity => entity.Id == id)
                      .ExecuteDeleteAsync(cancellationToken);
    }
  }
}
