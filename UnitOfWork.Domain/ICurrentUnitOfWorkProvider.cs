using System;
using System.Collections.Generic;
using System.Text;

namespace UnitOfWork
{
    public interface ICurrentUnitOfWorkProvider
    {
        IUnitOfWork CurrentUnitOfWork { get; set; }
    }
}
