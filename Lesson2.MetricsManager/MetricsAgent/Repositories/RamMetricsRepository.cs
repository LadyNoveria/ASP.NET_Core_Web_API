using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.Repositories;
namespace MetricsAgent
{
    public interface IRamMetricsRepository : IRepository<RamMetric>
    {

    }
    public class RamMetricsRepository: IRamMetricsRepository
    {
        private ConnectionProvider _connectionProvider;
        private SQLiteConnection _connection;
        public void Create(RamMetric item)
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DROP TABLE IF EXISTS rammetrics ";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE rammetrics (id INTEGER PRIMARY KEY, value INT, time INT);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTOrammetrics(value, time)VALUES(@value, @time);";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<RamMetric> GetAll()
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM rammetrics";
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            var returnList = new List<RamMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }

        public IList<RamMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM rammetrics WHERE Time >= fromTime AND Time <= toTime";
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            var returnList = new List<RamMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetric
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
