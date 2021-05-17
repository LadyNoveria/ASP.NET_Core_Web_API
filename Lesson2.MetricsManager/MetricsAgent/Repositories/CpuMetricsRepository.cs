using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.Repositories;
using Dapper;

namespace MetricsAgent
{
    public interface ICpuMetricsRepository: IRepository<CpuMetric>
    {
    }
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private ConnectionProvider _connectionProvider;
        private SQLiteConnection _connection;
        public CpuMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
        }
        public void Create(CpuMetric item)
        {
            using var cmd = new SQLiteCommand(_connection);

            _connection.Execute("INSERT INTO cpumetrics(value, time)VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public IList<CpuMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<CpuMetric>("SELECT * FROM cpumetrics").ToList();
        }

        public IList<CpuMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM cpumetrics WHERE Time >= fromTime AND Time <= toTime";
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            return _connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE time BETWEEN @from AND @to",
                new
                {
                    from = fromTime,
                    to = toTime
                }).ToList();
        }
    }
}
