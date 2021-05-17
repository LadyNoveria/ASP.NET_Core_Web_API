using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.Repositories
{
    public interface IConnectionProvider
    {
        SQLiteConnection CreateOpenedConnection();
    }
}
