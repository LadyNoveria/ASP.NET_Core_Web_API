using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

namespace MetricsAgent.Repositories
{
    public interface IDotNetMetricsRepository : IRepository<DotNetMetric>
    {

    }
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private ConnectionProvider _connectionProvider;
        private SQLiteConnection _connection;
        public DotNetMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
        } 
        public void Create(DotNetMetric item)
        {
            using var cmd = new SQLiteCommand(_connection);

            _connection.Execute("INSERT INTO dotnetmetrics(value, time)VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public IList<DotNetMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<DotNetMetric>("SELECT * FROM dotnetmetrics").ToList();
        }
        public IList<DotNetMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            return _connection.Query<DotNetMetric>("SELECT * FROM dotnetmetrics WHERE time BETWEEN @from AND @to",
                new
                {
                    from = fromTime,
                    to = toTime
                }).ToList();
        }
    }
}
