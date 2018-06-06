using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace MicroMySQLSharp
{
    public static class MicroMySQLSharp
    {
        /// <summary>
        /// Initializes its static Instance.
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            if (_connstr != null) _connstr = null;
            _connstr = connectionString;
        }
        private static string _connstr;
        internal static string ConnectionString
        {
            get
            {
                if (_connstr == null || string.IsNullOrWhiteSpace(_connstr))
                    throw new Exception("MicroMySQLWrapper needs to be initialized.");
                return _connstr;
            }
        }

        /// <summary>
        /// Execute SQL query without responses, such as CREATE, UPDATE, INSERT, DELETE.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string s)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connstr))
                using (MySqlCommand cmd = new MySqlCommand(s, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// Execute SQL query and returns System.Data.DataRow object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<DataRow> GetDataRow(string query)
        {
            var res = new List<DataRow>();
            try
            {

                var dt = GetDataTable(query);
                foreach (DataRow dr in dt.Rows)
                {
                    res.Add(dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// Execute SQL query and returns System.Data.DataTable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string query)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connstr))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    conn.Open();
                    var dt = new DataTable();
                    dt.Clear();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
    }
}
