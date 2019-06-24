using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UnitOfWork
{
    public  class DbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext> where TDbContext : DbContext
    {
        //public TDbContext CreateDbContext(string[] args)
        //{
        //    var builder = new DbContextOptionsBuilder();
        //    //var contextOption = builder.UseSqlServer(args[0]).Options;

        //    public TDbContext tt=null;
        //    //var te = new UnitOfWorkDbContext(contextOption);
        //    //TDbContext dbContext = new TDbContext(contextOption);
        //    //var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

        //    //    SecondDbContextOptionsConfigurer.Configure(
        //    //        builder,
        //    //        configuration.GetConnectionString(MultipleDbContextEfCoreDemoConsts.SecondDbConnectionStringName)
        //    //    );

        //    return tt;
        //}
        public  TDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder();
            var contextOption = builder.UseSqlServer(args[0]).Options;
            var dbContext = new DbContext(contextOption);
            return (TDbContext)dbContext;
        }
    }
}