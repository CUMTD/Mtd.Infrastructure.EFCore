﻿using Microsoft.EntityFrameworkCore;
using Mtd.Core.Entities;
using Mtd.Core.Repositories;
using System.Security.Principal;

namespace Mtd.Infrastructure.EFCore.Repositories
{
	public abstract class AsyncEFIdentifiableRepository<T_Identity, T_Entity> : AsyncEFRepository<T_Entity>, IAsyncIdentifiable<T_Identity, T_Entity>
		where T_Identity : notnull, IComparable<T_Identity>
		where T_Entity : class, IIdentity<T_Identity>
	{
		protected AsyncEFIdentifiableRepository(DbContext context) : base(context)
		{
		}


		#region IAsyncIdentifiable

		public async Task<T_Entity> GetByIdentityAsync(T_Identity identity, CancellationToken cancellationToken)
		{
			var result = await GetByIdentityOrDefaultAsync(identity, cancellationToken).ConfigureAwait(false);
			return result ?? throw new InvalidOperationException($"{identity} not found.");
		}

		public async Task<T_Entity?> GetByIdentityOrDefaultAsync(T_Identity identity, CancellationToken cancellationToken)
		{
			var result = await _dbSet.FindAsync([identity], cancellationToken: cancellationToken).ConfigureAwait(false);
			return result;
		}

		#endregion IAsyncIdentifiable

	}
}
