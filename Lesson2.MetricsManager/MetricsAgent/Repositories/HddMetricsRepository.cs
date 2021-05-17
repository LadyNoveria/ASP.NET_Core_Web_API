using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsAgent.Repositories;
using Dapper;

namespace MetricsAgent
{
    public interface IHddMetricsRepository : IRepository<HddMetric>
    {

    }
    public class HddMetricsRepository : IHddMetricsRepository 
    {
        private ConnectionProvider _connectionProvider;
        private SQLiteConnection _connection;
        public HddMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
        }
        public void Create(HddMetric item)
        {
            using var cmd = new SQLiteCommand(_connection);

            _connection.Execute("INSERT INTO hddmetrics(value, time)VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public IList<HddMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<HddMetric>("SELECT * FROM hddmetrics").ToList();
        }
        public IList<HddMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<HddMetric>("SELECT * FROM hddmetrics WHERE time BETWEEN @from AND @to",
                new
                {
                    from = fromTime,
                    to = toTime
                }).ToList();
        }
    }
}
