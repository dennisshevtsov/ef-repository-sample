// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Data
{
  /// <summary>Represents an entity base.</summary>
  public abstract class EntityBase
  {
    /// <summary>Gets an object that represents an ID of an entity.</summary>
    public Guid Id { get; protected set; }

    /// <summary>Creates a copy of an entity.</summary>
    /// <param name="entity">An object that represents an entity to copy.</param>
    /// <returns>An object that represents an instance of an entity copy.</returns>
    /// <exception cref="System.NotSupportedException">Throws if there is no such entity.</exception>
    public static T2 Create<T1, T2>(T1 entity) where T2 : EntityBase, T1
      => (T2)Activator.CreateInstance(typeof(T2), entity)!;
  }
}
