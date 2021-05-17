using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.Repositories;
using Dapper;

namespace MetricsAgent
{
    public interface IRamMetricsRepository : IRepository<RamMetric>
    {

    }
    public class RamMetricsRepository: IRamMetricsRepository
    {
        private ConnectionProvider _connectionProvider;
        private SQLiteConnection _connection;

        public RamMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
        }

        public void Create(RamMetric item)
        {
            using var cmd = new SQLiteCommand(_connection);

            _connection.Execute("INSERT INTO rammetrics(value, time)VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public IList<RamMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<RamMetric>("SELECT * FROM rammetrics").ToList();
        }

        public IList<RamMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<RamMetric>("SELECT * FROM rammetrics WHERE time BETWEEN @from AND @to",
                new
                {
                    from = fromTime,
                    to = toTime
                }).ToList();
        }
    }
}
