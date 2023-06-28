// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfRepositorySample.Author;

/// <summary>Provides a simple API to persistence of the <see cref="EfRepositorySample.Author.IAuthorEntity"/>.</summary>
public interface IAuthorRepository : IRepository<IAuthorEntity, IAuthorIdentity>
{
}
