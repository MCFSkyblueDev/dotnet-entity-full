using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Services;

namespace Core.Interfaces.Factories
{
    public interface IAuthFactory
    {
        IAuthService CreateAuthService(string provider);
    }
}