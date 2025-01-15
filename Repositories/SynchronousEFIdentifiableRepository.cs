using Microsoft.EntityFrameworkCore;
using Mtd.Core.Entities;
using Mtd.Core.Repositories;

namespace Mtd.Infrastructure.EFCore.Repositories
{
	public abstract class SynchronousEFIdentifiableRepository<T_Identity, T_Entity>(DbContext context) : SynchronousEFRepository<T_Entity>(context), IIdentifiable<T_Identity, T_Entity>
		where T_Identity : IComparable<T_Identity>
		where T_Entity : class, IIdentity<T_Identity>
	{
		public T_Entity GetByIdentity(T_Identity identity) => _dbSet
			.Find(identity) ?? throw new InvalidOperationException($"{identity} not found.");
		public T_Entity? GetByIdentityOrDefault(T_Identity identity) => _dbSet
			.Find(identity);
	}
}
