using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Data_Access_Layer.Common
{
    public class DatabaseCommand
    {
        public static DataTable GetData(string query)
        {
            DAL dal = new DAL();// connection has been opened
            DataTable dt = new DataTable();// variable to represent a table with data

            using (SqlCommand cmd = new SqlCommand(query, dal.connection))// represents a stored procedure to execute against a SQL server DB
            {
                cmd.CommandType = CommandType.Text;// gets or set a value indicating how the CommandText property is to be interpreted

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))// represents a set of data commands and a database connection used to fill the dataset
                {
                    sda.Fill(dt);
                }
            }

            dal.CloseConnection();

            return dt;

        }

        public static int UpdateAndInsertData(string query, List<SqlParameter> parameters)
        {
            int numberOfRowsAffected = 0;
            DAL dal = new DAL();

            using (SqlCommand cmd = new SqlCommand(query, dal.connection))
            {
                // take the command type as text
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                numberOfRowsAffected = cmd.ExecuteNonQuery();// executes a Transact-SQL statement against the connection and returns the number of rows affected
            }

            dal.CloseConnection();

            return numberOfRowsAffected;
        }

        public static DataTable GetDataWithConditions(string query, List<SqlParameter> parameters)
        {
            DAL dal = new DAL();

            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, dal.connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))// SqlDataAdapter is always used to fill a DataTable
                {
                    sda.Fill(dt);
                }
            }

            dal.CloseConnection();

            return dt;
        }

    }
}