using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ScanWebAPI.Connectors
{
    public class DatabaseConnector
    {
        private string connectionString;

        private SqlConnection sqlConnection;
        public DatabaseConnector(string ServerName, string Database, string User_ID, string Password)
        {
            connectionString = $"Server={ServerName};Database={Database};User Id={User_ID};Password={Password};";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        public DatabaseConnector(string connectionString)
        {
            this.connectionString = connectionString;
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }
        ~DatabaseConnector()
        {
            sqlConnection.Close();
        }

        public List<Dictionary<string, object>> Select(string selectQuery)
        {
            var finresult = new List<Dictionary<string, object>>();

            using (SqlCommand command = new SqlCommand(selectQuery, sqlConnection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var result = new Dictionary<string, object>();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        result.Add(reader.GetName(i), reader[i]);
                    }
                    finresult.Add(result);
                }
            }
            return finresult;
        }

        public void Insert(string insertQuery, params dynamic[] prms)
        {
            //string sql = "INSERT INTO klant(klant_id,naam,voornaam) VALUES(@param1,@param2,@param3)";
            // Count the matches, which executes the query.  
            using (SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection))
            {
                for(var i = 0; i < prms.Count(); i++)
                {
                    cmd.Parameters.Add($"@param{i+1}", Helper.@SwitchType[prms[i].GetType()]).Value = prms[i];
                }
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            
        }

        public void Update(string updateQuery, params dynamic[] prms)
        {
            //command.CommandText = "UPDATE Student 
            //SET Address = @add, City = @cit Where FirstName = @fn and LastName = @add";

            using (SqlCommand cmd = new SqlCommand(updateQuery, sqlConnection))
            {
                for (var i = 0; i < prms.Count(); i++)
                {
                    cmd.Parameters.Add($"@param{i+1}", Helper.@SwitchType[prms[i].GetType()]).Value = prms[i];
                }
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }



        }

        

    }
}
