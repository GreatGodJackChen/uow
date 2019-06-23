using System;
using System.Collections.Generic;
using System.Text;

namespace UnitOfWork
{
    class CurrentUnitOfWorkProvider
    {
        public IUnitOfWork Current
        {
            get;set;
        }
    }
}
