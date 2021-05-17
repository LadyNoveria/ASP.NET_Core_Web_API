using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public interface IRepository<T> where T: class
    {
        IList<T> GetAll();
        IList<T> GetByTimePeriod(TimeSpan time1, TimeSpan time2);
        void Create(T item);
    }
}
