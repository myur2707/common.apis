using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;

namespace BLL.Utilities
{
    public class DBObjectFactory
    {
        //private static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        private static string connectionString = ConfigurationManager.ConnectionStrings["campus_conciergeConnectionString"].ConnectionString;
        //private static string DataSourceType = ConfigurationManager.AppSettings.Get("CurrentDataBase");
        private static string Provider = ConfigurationManager.ConnectionStrings[0].ProviderName;

        // private static string connectionString = "Data Source=MPRADEEP\\SQL2005;Initial Catalog=SBSSNDPortal;Persist Security Info=True;User ID=sa;Password=m_pradeep2411";
        // private static string Provider = "System.Data.SqlClient";

        public static IDbConnection GetConnectionObject()
        {
            string DataSourceType;

            DataSourceType = ConfigurationManager.AppSettings.Get("CurrentDataBase");
            switch (Provider)
            {
                case "System.Data.SqlClient":
                    //return new OleDbConnection();
                    return new System.Data.SqlClient.SqlConnection();
                //case "ORACLE":
                //	return new OracleConnection();					
            }
            return null;
        }

        #region FOR WEB
        //Added by ketan on 21/07/2011

        public static IDbCommand GetCommandObject()
        {
            switch (Provider)
            {
                case "System.Data.SqlClient":
                    return new SqlCommand();
                //case "ORACLE":
                //	return new OracleCommand();
            }
            return null;
        }
        public static IDataParameter MakeParameter(String ParameterName, DbType ParameterType, Object ParameterValue)
        {
            IDataParameter parameter = DBObjectFactory.GetParameterObject();
            parameter.ParameterName = ParameterName;
            parameter.DbType = ParameterType;
            parameter.Value = ParameterValue;
            return parameter;
        }
        public static IDbDataAdapter GetDataAdapterObject(IDbCommand DBCommand)
        {
            switch (Provider)
            {
                case "System.Data.SqlClient":
                    return new SqlDataAdapter((SqlCommand)DBCommand);
                //case "ORACLE":
                //    return new OracleDataAdapter((OracleCommand)DBCommandObj);					
            }
            return null;
        }

        public static IDataParameter GetParameterObject()
        {
            switch (Provider)
            {
                case "System.Data.SqlClient":
                    return new SqlParameter();
                //case "ORACLE":
                //	return new OracleParameter();
            }
            return null;
        }

        #endregion
    }
}
