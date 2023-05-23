// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample
{
  /// <summary>Provides a simple API to persistence of an entity.</summary>
  public interface IRepository<TEntity, TIdentity>
  {
    /// <summary>Gets an entity.</summary>
    /// <param name="identity">An object that represents an identity of an entity.</param>
    /// <param name="relations">An object that represents a collection of relations to load.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future.</returns>
    public Task<TEntity?> GetAsync(TIdentity identity, IEnumerable<string> relations, CancellationToken cancellationToken);

    /// <summary>Adds an entity.</summary>
    /// <param name="entity">An object that represents an entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future.</returns>
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>Updates an entity.</summary>
    /// <param name="originalEntity">An object that represents an entity to update.</param>
    /// <param name="newEntity">An object that represents an entity from which the original entity should be updated.</param>
    /// <param name="properties">An object that represents a collection of properties to update.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public Task UpdateAsync(TEntity originalEntity, TEntity newEntity, IEnumerable<string> properties, CancellationToken cancellationToken);

    /// <summary>Deletes an entity.</summary>
    /// <param name="identity">An object that represents an identity of an entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public Task DeleteAsync(TIdentity identity, CancellationToken cancellationToken);
  }
}
