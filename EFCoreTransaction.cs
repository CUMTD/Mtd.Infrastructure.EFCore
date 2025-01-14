using Microsoft.EntityFrameworkCore.Storage;
using Mtd.Core.Entities;

namespace Mtd.Infrastructure.EFCore
{
	public class EFCoreTransaction : ITransaction, IDisposable
	{
		private readonly IDbContextTransaction _transaction;
		private bool _disposed = false;

		public EFCoreTransaction(IDbContextTransaction transaction)
		{
			ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));
			_transaction = transaction;
		}

		public Guid TransactionId => _transaction.TransactionId;

		public bool SupportsSavePoints => _transaction.SupportsSavepoints;

		public Task CommitAsync(CancellationToken cancellationToken) => _transaction.CommitAsync(cancellationToken);
		public async Task<string> CreateSavePointAsync(string savePointName, CancellationToken cancellationToken)
		{
			await _transaction.CreateSavepointAsync(savePointName, cancellationToken).ConfigureAwait(false);
			return savePointName;
		}

		public Task ReleaseSavePointAsync(string savePointName, CancellationToken cancellationToken) => _transaction.ReleaseSavepointAsync(savePointName, cancellationToken);
		public Task RollbackAsync(CancellationToken cancellationToken) => _transaction.RollbackAsync(cancellationToken);
		public async Task<string> RollbackToSavePointAsync(string savePointName, CancellationToken cancellationToken)
		{
			await _transaction.RollbackToSavepointAsync(savePointName, cancellationToken).ConfigureAwait(false);
			return savePointName;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_transaction.Dispose();
				}

				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
