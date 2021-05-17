using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.Repositories;

namespace MetricsAgent
{
    public interface ICpuMetricsRepository: IRepository<CpuMetric>
    {
    }
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private ConnectionProvider _connectionProvider;
        private SQLiteConnection _connection;
        public void Create(CpuMetric item)
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DROP TABLE IF EXISTS cpumetrics ";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE TABLE cpumetrics (id INTEGER PRIMARY KEY, value INT, time INT);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO cpumetrics(value, time)VALUES(@value, @time);";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<CpuMetric> GetAll()
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM cpumetrics";
            cmd.ExecuteNonQuery();
            var returnList = new List<CpuMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new CpuMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    }) ;
                }
            }
            return returnList;
        }

        public IList<CpuMetric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            _connectionProvider = new ConnectionProvider();
            _connection = _connectionProvider.CreateOpenedConnection();
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM cpumetrics WHERE Time >= fromTime AND Time <= toTime";
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            var returnList = new List<CpuMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new CpuMetric
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
