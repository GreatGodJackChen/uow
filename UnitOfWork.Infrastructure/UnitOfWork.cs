using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork,IActiveUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public UnitOfWork(TDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public TDbContext GetOrCreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            var concreteDbContextType = typeof(TDbContext);
            //反射出app对象
            Assembly assembly = Assembly.GetExecutingAssembly();
            var dbContext = (TDbContext)assembly.CreateInstance(concreteDbContextType.FullName);
            return dbContext;
        }
    }

}
