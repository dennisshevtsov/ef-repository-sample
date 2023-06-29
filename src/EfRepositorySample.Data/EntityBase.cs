// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using System.Reflection;

namespace EfRepositorySample.Data;

/// <summary>Represents an entity base.</summary>
public abstract class EntityBase : IUpdatable<object>
{
  /// <summary>Gets an object that represents an ID of an entity.</summary>
  public Guid Id { get; protected set; }

  /// <summary>Gets a collection of relation that this entity has.</summary>
  /// <param name="relations">An object that represents a collection of relations.</param>
  /// <returns>An object that represents a collection of relation that this entity has.</returns>
  public abstract IEnumerable<string> Relations(IEnumerable<string> relations);

  /// <summary>Updates this entity.</summary>
  /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
  public void Update(object newEntity)
  {
    ISet<string> updatingProperties = GetUpdatingProperties();
    ISet<string> updatedProperties  = updatingProperties;

    Update(newEntity, updatedProperties, updatingProperties);
  }

  /// <summary>Updates this entity.</summary>
  /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
  /// <param name="properties">An object that represents a collection of properties to update.</param>
  public void Update(object newEntity, IEnumerable<string> properties) =>
    Update(newEntity, properties, GetUpdatingProperties());

  protected virtual void Update(object newEntity, IEnumerable<string> updatedProperties, ISet<string> updatingProperties)
  {
    foreach (string property in updatedProperties)
    {
      if (updatingProperties.Contains(property))
      {
        Update(newEntity, property);
      }
    }
  }

  protected virtual void Update(object newEntity, string property)
  {
    PropertyInfo originalProperty = GetType().GetProperty(property)!;
    PropertyInfo newProperty      = newEntity.GetType().GetProperty(property)!;

    object? originalValue = originalProperty.GetValue(this);
    object? newValue      = newProperty.GetValue(newEntity);

    if (!object.Equals(originalValue, newValue))
    {
      originalProperty.SetValue(this, newValue);
    }
  }

  private ISet<string> GetUpdatingProperties() =>
    GetType().GetProperties()
             .Where(property => property.CanWrite)
             .Select(property => property.Name)
             .ToHashSet(StringComparer.OrdinalIgnoreCase);

  /// <summary>Creates a copy of an entity.</summary>
  /// <param name="entity">An object that represents an entity to copy.</param>
  /// <returns>An object that represents an instance of an entity copy.</returns>
  /// <exception cref="System.NotSupportedException">Throws if there is no such entity.</exception>
  public static T2 Create<T1, T2>(T1 entity) where T2 : EntityBase, T1
  {
    ArgumentNullException.ThrowIfNull(entity);

    if (entity.GetType() ==  typeof(T2))
    {
      return (T2)entity;
    }

    return (T2)typeof(T2).GetConstructor(new[] { typeof(T1) })!
                         .Invoke(new object[] { entity! });
  }
}
