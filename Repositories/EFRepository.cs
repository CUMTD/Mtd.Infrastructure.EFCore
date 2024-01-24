using Microsoft.EntityFrameworkCore;

namespace Mtd.Infrastructure.EFCore.Repositories
{
	public abstract class EFRepository<T> : IDisposable, IAsyncDisposable where T : class
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<T> _dbSet;
		private bool _disposedValue = false;

		protected EFRepository(DbContext dbContext)
		{
			ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}

		protected IQueryable<T> Query() => _dbSet.AsQueryable();

		#region IDisposable

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public async ValueTask DisposeAsync()
		{
			await DisposeAsyncCore().ConfigureAwait(false);

			Dispose(disposing: false);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}

				_disposedValue = true;
			}
		}

		protected virtual async ValueTask DisposeAsyncCore()
		{
			if (!_disposedValue)
			{
				await _dbContext.DisposeAsync().ConfigureAwait(false);
				_disposedValue = true;
			}
		}

		#endregion IDisposable
	}
}
