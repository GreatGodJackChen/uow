using System;
using System.Collections.Generic;

namespace UnitOfWork.Web
{
    public interface ITypeFinder
    {
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);
    }
}