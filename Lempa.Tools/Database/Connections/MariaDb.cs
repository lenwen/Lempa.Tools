using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Threading;

namespace Lempa.Tools.Database.Connections
{
    public class MariaDbConnectionData
    {
        public bool DataSet { get; set; }
        public string DbServer { get; set; }
        public string DbServerPort { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbUserPw { get; set; }

    }
    public class MariaDb
    {

        private MySql.Data.MySqlClient.MySqlConnection _conn;
        private MariaDbConnectionData ConData { get; set; }
        #region Settings

        public bool QueryConnectIsReadOnlyDb { get; set; }
        public int QueryRunTry { get; set; }
        public int QueryTimeOut { get; set; }
        public string QuerySql { get; set; }

        //  Query Return Information
        public DataTable ReturnDt { get; set; }
        public string ReturnString { get; set; }
        public long ReturnInt64 { get; set; }
        public bool QueryWasDone { get; set; }

        //  Query Debug information
        public List<string> ExtraStringList { get; set; }
        public int MySqlExceptionId { get; set; }
        public string MySqlExceptionIdText { get; set; }
        public string MySqlExceptionMessage { get; set; }

        #endregion
        public MariaDb(MariaDbConnectionData conData)
        {
            this.ConData = conData;

            this.QueryConnectIsReadOnlyDb = false;
            // this.QueryConnectToServer = QueryConnectToServerEnum.None;
            this.QueryRunTry = 5;
            this.QueryTimeOut = 60;
            this.QuerySql = null;
            this.ExtraStringList = new List<string>();
            this.QueryWasDone = false;

        }
        public void ClearReturnData()
        {
            this.ReturnDt = null;
            this.ReturnInt64 = 0;
            this.ReturnString = null;
            this.QuerySql = "";
            this.QueryWasDone = false;
            this.ExtraStringList = new List<string>();
            this.MySqlExceptionId = -1;
            this.MySqlExceptionIdText = "";
            this.MySqlExceptionMessage = "";

        }

        #region Open and close voids

        private void Open()
        {
            if (ConData.DataSet)
            {
                var connStr = string.Format("server={0};user={1};database={2};port={3};password={4};Encrypt=false", this.ConData.DbServer, this.ConData.DbUser, this.ConData.DbName, this.ConData.DbServerPort, this.ConData.DbUserPw);
                this._conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                
                this._conn.Open();
            }
            else
            {
                GenerateErrorMessage();
            }



        }
        private void Close()
        {
            this._conn.Close();
        }

        #endregion

        #region Error handling
        public void GenerateErrorMessage()
        {
            // http://dev.mysql.com/doc/connector-net/en/connector-net-programming-connecting-errors.html

            switch (MySqlExceptionId)
            {
                case 0:
                    this.MySqlExceptionIdText = "Cannot connect to server.  Contact administrator";
                    break;
                case 1045:
                    this.MySqlExceptionIdText = "Invalid username/password, please try again";
                    break;
                default:
                    this.MySqlExceptionIdText = "id: " + MySqlExceptionId.ToString();
                    break;
            }

        }
        #endregion


        #region Query Execute - Singel Query

        public void ExecuteQuerySelect()
        {
            var count = 0;

            while (true)
            {
                count = count + 1;
                try
                {
                    this.Open();
                    var cmd = this._conn.CreateCommand();
                    cmd.CommandText = this.QuerySql;

                    cmd.CommandTimeout = this.QueryTimeOut;

                    MySql.Data.MySqlClient.MySqlDataReader msdr = cmd.ExecuteReader();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    for (int i = 0; i < msdr.VisibleFieldCount; i++)
                        // ReSharper disable once AssignNullToNotNullAttribute
                        dt.Columns.Add(msdr.GetName(i), type: msdr.GetFieldType(i));
                    while (msdr.Read())
                    {
                        object[] cols = new object[msdr.VisibleFieldCount];
                        msdr.GetValues(cols);
                        dt.Rows.Add(cols);
                    }
                    msdr.Close();
                    this.ReturnDt = dt;
                    this.Close();
                    this.QueryWasDone = true;
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    this.ExtraStringList.Add("<br>Catch (MySql.Data.MySqlClient.MySqlException ex)");
                    this.QueryWasDone = false;
                    this.MySqlExceptionId = ex.Number;
                    this.MySqlExceptionMessage = ex.Message;
                    this.GenerateErrorMessage();
                    if ((ex.Number == 0) && ex.Message.StartsWith("Timeout expired."))
                        break;

                }

                catch (Exception e)
                {
                    this.QueryWasDone = false;
                    this.ExtraStringList.Add("<br>Catch (Exeption e)");
                    this.ExtraStringList.Add("<br>Execption e");
                    this.ExtraStringList.Add("<br>e.Message: <br>" + e.Message);
                    this.ExtraStringList.Add("<br>e.InneException: <br>" + e.InnerException);
                    this.ExtraStringList.Add("<br>e.Data: <br>" + e.Data);
                    this.ExtraStringList.Add("<br>e.Source: <br>" + e.Source);
                }

                if (this.QueryWasDone) break;

                if (count >= this.QueryRunTry)
                {

                    break;
                }

                Thread.Sleep(5000);

            }


        }

        public void ExecuteQueryUpdate()
        {
            var count = 0;

            while (true)
            {
                count = count + 1;
                try
                {
                    this.Open();
                    var cmd = this._conn.CreateCommand();
                    cmd.CommandText = this.QuerySql;
                    cmd.CommandTimeout = this.QueryTimeOut;
                    cmd.ExecuteNonQuery();
                    this.Close();
                    this.QueryWasDone = true;
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    this.ExtraStringList.Add("<br>Catch (MySql.Data.MySqlClient.MySqlException ex)");
                    this.QueryWasDone = false;
                    this.MySqlExceptionId = ex.Number;
                    this.MySqlExceptionMessage = ex.Message;
                    this.GenerateErrorMessage();

                }

                catch (Exception e)
                {
                    this.QueryWasDone = false;
                    this.ExtraStringList.Add("<br>Catch (Exeption e)");
                    this.ExtraStringList.Add("<br>Execption e");
                    this.ExtraStringList.Add("<br>e.Message: <br>" + e.Message);
                    this.ExtraStringList.Add("<br>e.InneException: <br>" + e.InnerException);
                    this.ExtraStringList.Add("<br>e.Data: <br>" + e.Data);
                    this.ExtraStringList.Add("<br>e.Source: <br>" + e.Source);
                }

                if (this.QueryWasDone) break;

                if (count >= this.QueryRunTry)
                {

                    break;
                }

                Thread.Sleep(5000);
            }
        }

        public void ExecuteQueryInsertReturnRowIdInt64(bool ReturnRowIdInt64 = true)
        {
            var count = 0;

            while (true)
            {
                count = count + 1;
                try
                {
                    this.Open();
                    var cmd = this._conn.CreateCommand();
                    cmd.CommandText = this.QuerySql;
                    cmd.CommandTimeout = this.QueryTimeOut;
                    cmd.ExecuteNonQuery();

                    if (ReturnRowIdInt64)
                        this.ReturnInt64 = cmd.LastInsertedId;

                    this.Close();
                    this.QueryWasDone = true;
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    this.ExtraStringList.Add("<br>Catch (MySql.Data.MySqlClient.MySqlException ex)");
                    this.QueryWasDone = false;
                    this.MySqlExceptionId = ex.Number;
                    this.MySqlExceptionMessage = ex.Message;
                    this.GenerateErrorMessage();

                }

                catch (Exception e)
                {
                    this.QueryWasDone = false;
                    this.ExtraStringList.Add("<br>Catch (Exeption e)");
                    this.ExtraStringList.Add("<br>Execption e");
                    this.ExtraStringList.Add("<br>e.Message: <br>" + e.Message);
                    this.ExtraStringList.Add("<br>e.InneException: <br>" + e.InnerException);
                    this.ExtraStringList.Add("<br>e.Data: <br>" + e.Data);
                    this.ExtraStringList.Add("<br>e.Source: <br>" + e.Source);
                }

                if (this.QueryWasDone) break;

                if (count >= this.QueryRunTry)
                {

                    break;
                }

                Thread.Sleep(5000);
            }


        }
        #endregion
    }
}
