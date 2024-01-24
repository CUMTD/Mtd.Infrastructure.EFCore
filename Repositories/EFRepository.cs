using Microsoft.EntityFrameworkCore;

namespace Mtd.Infrastructure.EFCore.Repositories
{
	public abstract class EFRepository<T> : IDisposable, IAsyncDisposable where T : class
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<T> _dbSet;
		private bool _disposedValue;

		protected EFRepository(DbContext dbContext)
		{
			ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}

		protected IQueryable<T> Query() => _dbSet.AsQueryable();

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					_dbContext.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				_disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~EFRepository()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable

		#region IAsyncDisposable
		protected async Task DisposeAsync(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					await _dbContext.DisposeAsync().ConfigureAwait(false);
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				_disposedValue = true;
			}
		}

		public async ValueTask DisposeAsync()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			await DisposeAsync(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion IAsyncDisposable
	}
}
