using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVDTestApp.Logging
{
    interface ILogger
    {
        void Info(string message, string infoType, object caller, string methood);
        Task InfoAsync(string message, string infoType, object caller, string methood);
    }
}
