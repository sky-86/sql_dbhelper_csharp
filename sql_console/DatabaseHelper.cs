using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Program
{
    class DatabaseHelper
    {
        private string _connectionString;
        private bool _isConnected = false;
        private SqlConnection _dbConnection;
        private SqlCommand _sqlCommand;
        public string ConnectionString
        {
            get { return _connectionString; }
        }//end prop
        public bool IsConnected
        {
            get { return GetCurrentConnectionStatus(); }
        }//end prop

        public DatabaseHelper(string connectionString, bool connectNow = true)
        {
            _connectionString = connectionString;

            if (connectNow)
            {
                Connect();
            }//end if

        }//end constructor

        public bool Connect()
        {
            try
            {
                _dbConnection = new SqlConnection(_connectionString);
                _isConnected = true;
            }
            catch
            {
                _isConnected = false;
            }//end try

            return _isConnected;
        }//end method

        public object[][] ExecuteReader(string sqlStatement)
        {
            SqlDataReader queryReturnData = null;
            object[][] returnData = null;

            try
            {
                if (IsConnected)
                {
                    _dbConnection.Open();
                    _sqlCommand = new SqlCommand(sqlStatement, _dbConnection);
                    queryReturnData = _sqlCommand.ExecuteReader();
                    returnData = ConvertDataReaderTo2DArray(queryReturnData);
                    _dbConnection.Close();
                }//end if
            }
            catch (SqlException)
            {
                throw new Exception("INVALID SQL Check -> " + sqlStatement);
            }//end try

            return returnData;
        }//end method

        public int ExecuteNonQuery(string sqlStatement)
        {
            int recordsAffected = -1;

            try
            {
                if (IsConnected)
                {
                    _dbConnection.Open();
                    _sqlCommand = new SqlCommand(sqlStatement, _dbConnection);
                    recordsAffected = _sqlCommand.ExecuteNonQuery();
                    _dbConnection.Close();
                }//end if
            }
            catch (SqlException)
            {
                throw new Exception("INVALID SQL Check -> " + sqlStatement);
            }//end try

            return recordsAffected;
        }//end method

        public int GetTableRecordCount(string tableName)
        {
            if (_isConnected == false)
            {
                throw new Exception("Error: not connected");
            }
            try
            {
                object[][] results = ExecuteReader("SELECT * FROM tblUsers");
                return results.Length;
            }
            catch
            {
                return -1;
            }
        }//end method

        public bool FlushTable(string tableName)
        {
            if (_isConnected == false)
            {
                throw new Exception("Error: not connected");
            }
            try
            {
                DeleteTable(tableName);
                AddTable(tableName);
                return true;
            }
            catch
            {
                return false;
            }
        }//end method

        public bool DeleteTable(string tableName)
        {
            if (_isConnected == false)
            {
                throw new Exception("Error: not connected");
            }
            try
            {
                ExecuteReader(String.Format("DROP TABLE {0}", tableName));
                return true;
            }
            catch
            {
                return false;
            }
        }//end method

        public bool AddTable(string tableName)
        {
            if (_isConnected == false)
            {
                throw new Exception("Error: not connected");
            }
            try
            {
                ExecuteReader(String.Format("CREATE TABLE {0}(id INT PRIMARY KEY IDENTITY (1,1), first VARCHAR(255), last VARCHAR(255))", tableName));
                return true;
            }
            catch
            {
                return false;
            }
        }//end method

        public bool Connect(string newConnectionString)
        {//overload to connect to new db
             try
            {
                _dbConnection = new SqlConnection(newConnectionString);
                _isConnected = true;
            }
            catch
            {
                _isConnected = false;
            }//end try

            return _isConnected;
        }//end method

        private object[][] ConvertDataReaderTo2DArray(SqlDataReader data)
        {
            object[,] returnData = null;
            List<object[]> lstRows = new List<object[]>();

            while (data.Read())
            {
                object[] newRow = new object[data.FieldCount];

                for (int fieldIndex = 0; fieldIndex < data.FieldCount; fieldIndex += 1)
                {
                    newRow[fieldIndex] = data[fieldIndex];
                }//end for

                lstRows.Add(newRow);
            }//end while

            return lstRows.ToArray();
        }//end method

        private bool GetCurrentConnectionStatus()
        {
            bool pastConnection = _dbConnection != null;
            bool currentlyConnected = false;

            if (pastConnection == true)
            {
                currentlyConnected = _dbConnection.State != System.Data.ConnectionState.Broken;
            }//end if

            _isConnected = currentlyConnected;

            return currentlyConnected;
        }//end method

    }
}//end class
