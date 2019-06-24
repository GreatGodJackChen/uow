using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        public UnitOfWork(IConfiguration configuration,IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            ActiveDbContexts = new Dictionary<string, DbContext>();
        }

        protected IDictionary<string, DbContext> ActiveDbContexts { get; }

        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }
        public void SaveChanges()
        {
            foreach (var activeDbContext in GetAllActiveDbContexts())
            {
                activeDbContext.SaveChanges();
            }
        }
        public TDbContext GetOrCreateDbContext<TDbContext>() where TDbContext:DbContext
        {
            var concreteDbContextType = typeof(TDbContext);

            var dbContextType = typeof(TDbContext).ToString();
            var dbContextKey = dbContextType;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out var dbContext))
            {
                dbContext = (TDbContext)_serviceProvider.GetService(typeof(TDbContext));

                //var tesdb= (TDbContext)_serviceProvider.GetService(typeof(TDbContext));
                //var c= tesdb.Set<Customer.ContactAddress>();
                //var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                //string[] arrStr = { connectionString }; 
                //dbContext = new DbContextFactory<TDbContext>().CreateDbContext(arrStr);
                var uowconn = _configuration["ConnectionStrings:DefaultConnection"];
                var uowOptions = new DbContextOptionsBuilder<UnitOfWorkDbContext>()
                    .UseSqlServer(uowconn)
                    .Options;
                dbContext =new UnitOfWorkDbContext(uowOptions);
               var cus= dbContext.Set<Customer.Customer>();
            }
            //反射出app对象

                //Assembly assembly = Assembly.GetExecutingAssembly();
                //var dbContext1 = (TDbContext)assembly.CreateInstance(concreteDbContextType.FullName);
                
            ActiveDbContexts[dbContextType] = dbContext;
            return (TDbContext)dbContext;
        }
    }

}
