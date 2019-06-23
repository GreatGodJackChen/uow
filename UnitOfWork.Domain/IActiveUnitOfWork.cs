using System;
using System.Collections.Generic;
using System.Text;

namespace UnitOfWork
{
    public interface IActiveUnitOfWork
    {
        int SaveChanges();
    }
}
