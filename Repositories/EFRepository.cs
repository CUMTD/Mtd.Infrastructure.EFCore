using Microsoft.EntityFrameworkCore;

namespace Mtd.Infrastructure.EFCore.Repositories
{
	public abstract class EFRepository<T> where T : class
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<T> _dbSet;

		protected EFRepository(DbContext dbContext)
		{
			ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}

		protected IQueryable<T> Query() => _dbSet.AsQueryable();
	}
}
