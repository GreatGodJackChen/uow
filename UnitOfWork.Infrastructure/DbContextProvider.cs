using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitOfWork
{
    public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : DbContext
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DbContextProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TDbContext GetDbContext()
        {
            //return UnitOfWorkExtensions.GetDbContext<TDbContext>();
            return _unitOfWork.GetOrCreateDbContext<TDbContext>();
        }
    }
}
