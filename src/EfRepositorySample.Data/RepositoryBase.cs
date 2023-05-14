// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace EfRepositorySample.Data
{
  /// <summary>Provides a simple API to persistence of an entity.</summary>
  public abstract class RepositoryBase<TEntity, TIdentity> : IRepository<TEntity, TIdentity>
  {
    private readonly DbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="RepositoryBase{TEntity, TIdentity}"/> class.</summary>
    /// <param name="dbContext">An object that represents a session with the database and can be used to query and save instances of your entities.</param>
    protected RepositoryBase(DbContext dbContext)
    {
      _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>Gets an entity.</summary>
    /// <param name="identity">An object that represents an identity of an entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future.</returns>
    public Task<TEntity> GetAsync(TIdentity identity, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    /// <summary>Adds an entity.</summary>
    /// <param name="entity">An object that represents an entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future.</returns>
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    /// <summary>Updates an entity.</summary>
    /// <param name="entity">An object that represents an entity.</param>
    /// <param name="properties">An object that represents a collection of properties to update.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public Task UpdateAsync(TEntity entity, IEnumerable<string> properties, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    /// <summary>Deletes an entity.</summary>
    /// <param name="identity">An object that represents an identity of an entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public Task DeleteAsync(TIdentity identity, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
