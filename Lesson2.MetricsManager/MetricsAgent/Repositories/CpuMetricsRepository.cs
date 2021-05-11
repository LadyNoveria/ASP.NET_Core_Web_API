using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent
{
    public interface ICpuMetricsRepository: IRepository<CpuMetric>
    {
    }
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
        public CpuMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public void Create(CpuMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Execute("INSERT INTO cpumetrics(value, time)VALUES(@value, @time)",
                new {
                    value = item.Value,
                    time = item.Time
                });
        }

        public IList<CpuMetric> GetAll()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            return  connection.Query<CpuMetric>("SELECT * FROM cpumetrics").ToList();
        }

        public IList<CpuMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            return connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE time BETWEEN @from AND @to",
                new {
                    from = fromTime,
                    to = toTime
                }).ToList();
        }
    }
}
