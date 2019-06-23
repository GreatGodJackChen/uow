using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitOfWork
{
    public  static class UnitOfWorkExtensions
    {
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork)
            where TDbContext : DbContext
        {
            return (unitOfWork as UnitOfWork.UnitOfWork<TDbContext>).GetOrCreateDbContext<TDbContext>();
        }
    }
}
