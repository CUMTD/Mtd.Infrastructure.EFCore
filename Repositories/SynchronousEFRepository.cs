using Microsoft.EntityFrameworkCore;
using Mtd.Core.Entities;
using Mtd.Core.Repositories;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Mtd.Infrastructure.EFCore.Repositories
{
	public abstract class SynchronousEFRepository<T>(DbContext context) : EFRepository<T>(context), IReadable<T, IReadOnlyCollection<T>>, IWriteable<T, IReadOnlyCollection<T>> where T : class
	{

		#region IReadable
		public bool All(Expression<Func<T, bool>> predicate) => Query().All(predicate);
		public bool Any(Expression<Func<T, bool>> predicate) => Query().Any(predicate);

		public T First(Expression<Func<T, bool>> predicate) => Query().First(predicate);

		public T? FirstOrDefault(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) => Query().FirstOrDefault(predicate);

		public IReadOnlyCollection<T> GetAll() => Query().ToImmutableArray();

		public T Single(Expression<Func<T, bool>> predicate) => Query().Single(predicate);

		public T? SingleOrDefault(Expression<Func<T, bool>> predicate) => Query().SingleOrDefault(predicate);

		public IReadOnlyCollection<T> Where(Expression<Func<T, bool>> predicate) => Query().Where(predicate).ToImmutableArray();

		#endregion IReadable

		#region IWriteable
		public T Add(T entity) => _dbSet.Add(entity).Entity;

		public IReadOnlyCollection<T> AddRange(IEnumerable<T> entities)
		{
			_dbSet.AddRange(entities);
			return entities.ToImmutableArray();
		}

		public void Delete(T entity) => _dbSet.Remove(entity);

		public int CommitChanges() => _dbContext.SaveChanges();

		#endregion IWriteable

		public T Attach(T entity) => _dbContext.Attach(entity).Entity;
		public ITransaction CreateTransaction() => new EFCoreTransaction(_dbContext.Database.BeginTransaction());
	}
}
