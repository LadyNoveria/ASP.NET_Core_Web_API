using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.Repositories
{
    public interface IDotNetMetricsRepository : IRepository<DotNetMetric>
    {

    }
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private ConnectionProvider _connectionProvider;
        private SQLiteConnection _connection;
        public void Create(DotNetMetric item)
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DROP TABLE IF EXISTS dotnetmetrics ";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE dotnetmetrics (id INTEGER PRIMARY KEY, value INT, time INT);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO dotnetmetrics(value, time)VALUES(@value, @time);";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<DotNetMetric> GetAll()
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM dotnetmetrics";

            var returnList = new List<DotNetMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }
        public IList<DotNetMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM dotnetmetrics WHERE Time >= fromTime AND Time <= toTime";
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            var returnList = new List<DotNetMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }
    }
}
