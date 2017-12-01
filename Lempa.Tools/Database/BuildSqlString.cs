using System;



namespace Lempa.Tools.Database
{
    public class BuildSqlString
    {
        private string _sqlColum;
        private string _sqlValue;
        private string _sqlUpdateString;
        private Lempa.Tools.Database.Security DbSecurity { get; set; }

        public BuildSqlString()
        {
            this.DbSecurity = new Security();
        }

        public void ClearAllSqlInformationInString()
        {
            this._sqlColum = "";
            this._sqlValue = "";
            this._sqlUpdateString = "";


        }
        public void Insert(string sqlColumeName, string sqlColumeValue)
        {
            sqlColumeName = sqlColumeName.ToLower();

            if (string.IsNullOrEmpty(this._sqlColum))
                this._sqlColum = sqlColumeName;
            else
                this._sqlColum = this._sqlColum + "," + sqlColumeName;

            if (String.IsNullOrEmpty(this._sqlValue))
                this._sqlValue = DbSecurity.Fnuttify(sqlColumeValue);
            else
                this._sqlValue = this._sqlValue + "," + DbSecurity.Fnuttify(sqlColumeValue);

        }

        public string ReturnInsertSqlString(string tablename)
        {
            string sql = string.Format("insert into {0} ({1}) values ({2});", tablename, this._sqlColum, this._sqlValue);
            return sql;
        }

        public void Update(string sqlColumeName, string sqlColumeValue)
        {
            sqlColumeName = sqlColumeName.ToLower();


            if (string.IsNullOrEmpty(this._sqlUpdateString))
                this._sqlUpdateString = string.Format("{0}={1}", sqlColumeName, DbSecurity.Fnuttify(sqlColumeValue));
            else
                this._sqlUpdateString = this._sqlUpdateString + string.Format(",{0}={1}", sqlColumeName, DbSecurity.Fnuttify(sqlColumeValue));

        }
        public string ReturnUpdateSqlString(string tablename, string whereSats)
        {
            return string.Format("update {0} SET {1} where {2};", tablename, this._sqlUpdateString, whereSats);
            //x sqlUpdateString = "";
            //x return sql;
        }
    }
}
