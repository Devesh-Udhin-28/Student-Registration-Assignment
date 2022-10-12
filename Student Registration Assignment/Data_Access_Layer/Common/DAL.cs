using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Student_Registration_Assignment.Data_Access_Layer.Common
{
    public class DAL
    {
        public const string connectionString = @"server=localhost;database=StudentRegistration;uid=wbpoc;pwd=sql@tfs2008";

        public SqlConnection connection;

        public DAL()
        {
            this.connection = new SqlConnection(connectionString);
            OpenConnection();
        }

        public void OpenConnection()
        {
            try
            {
                if (this.connection.State == System.Data.ConnectionState.Open)
                {
                    this.connection.Close();
                }
                // without this, authentication works but update needs this to be able to work correctly !!!
                this.connection.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            if (this.connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                this.connection.Close();
                this.connection.Dispose();
            }
        }
    }
}