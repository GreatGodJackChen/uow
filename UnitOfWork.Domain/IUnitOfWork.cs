using Microsoft.EntityFrameworkCore;

namespace UnitOfWork
{
    public interface IUnitOfWork:IActiveUnitOfWork
    {
        //int SaveAllChanges();
        TDbContext GetOrCreateDbContext<TDbContext>() where TDbContext : DbContext;
    }
}
