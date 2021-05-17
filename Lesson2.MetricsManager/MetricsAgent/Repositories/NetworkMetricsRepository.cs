using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using System.Linq;

namespace MetricsAgent.Repositories
{
    public interface INetworkMetricsRepository : IRepository<NetworkMetric>
    {

    }
    public class NetworkMetricsRepository:INetworkMetricsRepository
    {
        private ConnectionProvider _connectionProvider;
        private SQLiteConnection _connection;
        public NetworkMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
        }
        public void Create(NetworkMetric item)
        {
            using var cmd = new SQLiteCommand(_connection);

            _connection.Execute("INSERT INTO networkmetrics(value, time)VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public IList<NetworkMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<NetworkMetric>("SELECT * FROM networkmetrics").ToList();
        }
        public IList<NetworkMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<NetworkMetric>("SELECT * FROM networkmetrics WHERE time BETWEEN @from AND @to",
                new
                {
                    from = fromTime,
                    to = toTime
                }).ToList();
        }
    }
}
