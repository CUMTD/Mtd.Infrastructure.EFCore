using Microsoft.EntityFrameworkCore;
using Mtd.Core.Repositories;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Mtd.Infrastructure.EFCore.Repositories
{
	public abstract class AsyncEFRepository<T> : EFRepository<T>, IAsyncReadable<T, IReadOnlyCollection<T>>, IAsyncWriteable<T, IReadOnlyCollection<T>> where T : class
	{
		protected AsyncEFRepository(DbContext context) : base(context) { }

		#region IAsyncReadable

		public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) => Query()
			.AllAsync(predicate, cancellationToken);

		public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) => Query()
			.AnyAsync(predicate, cancellationToken);

		public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) => Query()
			.FirstAsync(predicate, cancellationToken);

		public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) => Query()
			.FirstOrDefaultAsync(predicate, cancellationToken);

		public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken)
		{
			var result = await Query()
				.ToArrayAsync(cancellationToken)
				.ConfigureAwait(false);

			return result.ToImmutableArray();
		}

		public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) => Query()
			.SingleAsync(predicate, cancellationToken);

		public Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) => Query()
			.SingleOrDefaultAsync(predicate, cancellationToken);

		public async Task<IReadOnlyCollection<T>> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
		{
			var result = await Query()
				.Where(predicate)
				.ToArrayAsync(cancellationToken)
				.ConfigureAwait(false);

			return result.ToImmutableArray();
		}

		#endregion IAsyncReadable

		#region IEFAsyncWritable

		public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
		{
			var added = await _dbSet.AddAsync(entity, cancellationToken)
				.ConfigureAwait(false);
			return added.Entity;
		}

		public async Task<IReadOnlyCollection<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
		{
			await _dbSet.AddRangeAsync(entities, cancellationToken);
			return entities.ToImmutableArray();
		}

		public Task DeleteAsync(T entity, CancellationToken cancellationToken)
		{
			_dbSet.Remove(entity);
			return Task.CompletedTask;
		}

		public Task<int> CommitChangesAsync(CancellationToken cancellationToken) => _dbContext
			.SaveChangesAsync(cancellationToken);

		#endregion IEFAsyncWritable

		public ValueTask<T> AttachAsync(T entity)
		{
			var attached = _dbContext.Attach(entity);
			return ValueTask.FromResult(attached.Entity);
		}

		public IAsyncEnumerable<T> WhereAsync(Expression<Func<T, bool>> predicate) => Query()
			.Where(predicate)
			.AsAsyncEnumerable();

		public IAsyncEnumerable<T> GetAllAsync() => Query()
			.AsAsyncEnumerable();

	}
}
