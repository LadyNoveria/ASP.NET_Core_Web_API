using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;

namespace MetricsAgent
{
    public interface IHddMetricsRepository : IRepository<HddMetric>
    {

    }
        public class HddMetricsRepository : IHddMetricsRepository 
    {
            private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100";
            public void Create(HddMetric item)
            {
                using var connection = new SQLiteConnection(ConnectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(connection);

                cmd.CommandText = "DROP TABLE IF EXISTS hddmetrics ";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE hddmetrics (id INTEGER PRIMARY KEY, value INT, time INT);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO hddmetrics(value, time)VALUES(@value, @time);";
                cmd.Parameters.AddWithValue("@value", item.Value);
                cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

            public IList<HddMetric> GetAll()
            {
                using var connection = new SQLiteConnection(ConnectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(connection);
                cmd.CommandText = "SELECT * FROM hddmetrics";

                var returnList = new List<HddMetric>();
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnList.Add(new HddMetric
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
