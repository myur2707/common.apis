using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BLL.Utilities;
using System.Collections;
using System.Configuration;
using Microsoft.VisualBasic;

namespace BLL.Utilities
{
    public class BLGeneralUtil
    {
        #region FOR WEB USE
        //Added by Ketan on 21/07/2011
        public struct UpdateTableInfo
        {
            public Boolean Status;
            public String ErrorMessage;
            public int TotalRowsAffected;
            public DataRow ErrorRow;
            public String ErrorSQLStatement;
        }

        public struct CreateTableInfo
        {
            public byte Exec_status;
            public string Message;
        }

        public enum UpdateWhereMode
        {
            KeyColumnsOnly,//PK columns only
            KeyAndModifiedColumns //PK and non-PK columns
        }

        public enum UpdateMethod
        {
            Update, //Fires an Update Statement
            DeleteAndInsert //Fires a Delete statement and then Inserts a new row while updating
        }

        public enum PeriodType
        {
            Attendance,
            Leave,
            Loan,
            General
        }

        public static UpdateTableInfo UpdateTable(ref IDbCommand comm, DataTable dt, UpdateWhereMode updateWhereMode)
        {
            UpdateTableInfo structUpdateTableInfo;
            try
            {
                #region Initial Validations
                if (comm == null)
                {
                    structUpdateTableInfo.ErrorMessage = "Command object is not initialized.Operation cancelled.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                if (comm.Connection == null)
                {
                    structUpdateTableInfo.ErrorMessage = "Database connection object is not set.Operation cancelled.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                if (comm.Connection.State != ConnectionState.Open)
                {
                    structUpdateTableInfo.ErrorMessage = "Database connection is not open.Operation cancelled.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                if (comm.Transaction == null)
                {
                    structUpdateTableInfo.ErrorMessage = "Transaction object is not initialized.Operation cancelled.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                #endregion //Initial Validations

                int TotalRowsSentForUpdate = 0;
                foreach (DataRow drow in dt.Rows)
                {
                    #region Logic for generating the query
                    String SQL = "";
                    Boolean IsChangeFound = false;
                    switch (drow.RowState)
                    {
                        case DataRowState.Added:
                            comm.Parameters.Clear();
                            IsChangeFound = true;
                            String ColumnNames = "";
                            String ParameterNames = "";
                            foreach (DataColumn dcol in dt.Columns)
                            {
                                if (dcol.Ordinal < dt.Columns.Count - 1)
                                {
                                    ColumnNames += dcol.ColumnName + ",";
                                    ParameterNames += "@" + dcol.ColumnName + ",";
                                }
                                else
                                {
                                    ColumnNames += dcol.ColumnName;
                                    ParameterNames += "@" + dcol.ColumnName;
                                }

                                IDataParameter param = DBObjectFactory.GetParameterObject();
                                param.ParameterName = "@" + dcol.ColumnName;
                                param.Value = drow[dcol, DataRowVersion.Current];
                                comm.Parameters.Add(param);
                            }
                            if (ColumnNames == "" || ParameterNames == "")
                                break;

                            SQL = "INSERT INTO " + dt.TableName +
                                    " ( " + ColumnNames + ") VALUES (" + ParameterNames + ")";

                            //MessageBox.Show(SQL, "INSERT");
                            break;
                        case DataRowState.Modified:
                            comm.Parameters.Clear();
                            IsChangeFound = true;
                            String SetColumnValues = "";
                            String WhereColumns = "";
                            if (updateWhereMode == UpdateWhereMode.KeyAndModifiedColumns)
                            {
                                #region KeyAndModifiedColumns
                                foreach (DataColumn dcol in dt.Columns)
                                {
                                    //Form the SET part for the columns whose Current Value != Original Value
                                    if (drow[dcol, DataRowVersion.Current].ToString() != drow[dcol, DataRowVersion.Original].ToString())
                                    {
                                        SetColumnValues += dcol.ColumnName + " = @" + dcol.ColumnName + ",";
                                        IDataParameter NewValueParam = DBObjectFactory.GetParameterObject();
                                        NewValueParam.ParameterName = "@" + dcol.ColumnName;
                                        NewValueParam.Value = drow[dcol, DataRowVersion.Current];
                                        comm.Parameters.Add(NewValueParam);

                                        if (drow[dcol, DataRowVersion.Original] != System.DBNull.Value)
                                        {
                                            WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + "1 AND ";
                                            IDataParameter OldValueParam = DBObjectFactory.GetParameterObject();
                                            OldValueParam.ParameterName = "@" + dcol.ColumnName + "1";
                                            OldValueParam.Value = drow[dcol, DataRowVersion.Original];
                                            comm.Parameters.Add(OldValueParam);
                                        }
                                        else
                                            WhereColumns += dcol.ColumnName + " IS NULL AND ";
                                    }
                                }
                                //Remove the last comma from SET string
                                if (SetColumnValues != "")
                                    SetColumnValues = SetColumnValues.Substring(0, SetColumnValues.Length - 1);
                                else
                                {
                                    IsChangeFound = false;
                                    break;
                                }

                                //Set the Primary Key Field
                                foreach (DataColumn dcol in dt.PrimaryKey)
                                {
                                    //Avoid including those PK fields in WHERE clause if they are modified as
                                    //this is already done in previous loop
                                    if (drow[dcol, DataRowVersion.Current] == drow[dcol, DataRowVersion.Original])
                                    {
                                        WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + " AND ";
                                        IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                        KeyValueParam.ParameterName = "@" + dcol.ColumnName;
                                        KeyValueParam.Value = drow[dcol, DataRowVersion.Original];
                                        comm.Parameters.Add(KeyValueParam);
                                    }
                                }
                                //Remove the last AND from WHERE string
                                if (WhereColumns != "")
                                    WhereColumns = WhereColumns.Substring(0, WhereColumns.Length - 4);
                                else
                                {
                                    IsChangeFound = false;
                                    break;
                                }
                                #endregion //KeyAndModifiedColumns
                            }
                            else if (updateWhereMode == UpdateWhereMode.KeyColumnsOnly)
                            {
                                #region KeyColumnsOnly
                                foreach (DataColumn dcol in dt.Columns)
                                {
                                    //Form the SET part for the columns whose Current Value != Original Value
                                    SetColumnValues += dcol.ColumnName + " = @" + dcol.ColumnName + ",";
                                    IDataParameter NewValueParam = DBObjectFactory.GetParameterObject();
                                    NewValueParam.ParameterName = "@" + dcol.ColumnName;
                                    NewValueParam.Value = drow[dcol, DataRowVersion.Current];
                                    comm.Parameters.Add(NewValueParam);
                                }
                                //Remove the last comma from SET string
                                if (SetColumnValues != "")
                                    SetColumnValues = SetColumnValues.Substring(0, SetColumnValues.Length - 1);
                                else
                                {
                                    IsChangeFound = false;
                                    break;
                                }

                                //Set the Primary Key Field
                                foreach (DataColumn dcol in dt.PrimaryKey)
                                {
                                    WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + "1 AND ";
                                    IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                    KeyValueParam.ParameterName = "@" + dcol.ColumnName + "1";
                                    KeyValueParam.Value = drow[dcol, DataRowVersion.Original];
                                    comm.Parameters.Add(KeyValueParam);
                                }
                                //Remove the last AND from WHERE string
                                if (WhereColumns != "")
                                    WhereColumns = WhereColumns.Substring(0, WhereColumns.Length - 4);
                                else
                                {
                                    IsChangeFound = false;
                                    break;
                                }
                                #endregion //KeyColumnsOnly
                            }
                            SQL = "UPDATE " + dt.TableName +
                                    " SET " + SetColumnValues + " WHERE " + WhereColumns;

                            //MessageBox.Show(SQL, "UPDATE");
                            break;
                        case DataRowState.Deleted:
                            comm.Parameters.Clear();
                            IsChangeFound = true;
                            String DeleteWhereColumns = "";

                            //Set the Primary Key Field
                            foreach (DataColumn dcol in dt.PrimaryKey)
                            {
                                DeleteWhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + " AND ";
                                IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                KeyValueParam.ParameterName = "@" + dcol.ColumnName;
                                KeyValueParam.Value = drow[dcol, DataRowVersion.Original];
                                comm.Parameters.Add(KeyValueParam);
                            }
                            if (DeleteWhereColumns != "")
                                //Remove the last AND from WHERE string
                                DeleteWhereColumns = DeleteWhereColumns.Substring(0, DeleteWhereColumns.Length - 4);
                            else
                            {
                                IsChangeFound = false;
                                break;
                            }

                            SQL = "DELETE FROM " + dt.TableName + " WHERE " + DeleteWhereColumns;

                            //MessageBox.Show(SQL, "DELETE");
                            break;
                    }
                    #endregion

                    #region Fire the query
                    if (IsChangeFound)
                    {
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = SQL;
                        if (comm.ExecuteNonQuery() != 1)
                        {
                            structUpdateTableInfo.ErrorMessage = "Database updation failed.";
                            structUpdateTableInfo.ErrorSQLStatement = SQL;
                            structUpdateTableInfo.ErrorRow = drow;
                            structUpdateTableInfo.Status = false;
                            structUpdateTableInfo.TotalRowsAffected = TotalRowsSentForUpdate;
                            return structUpdateTableInfo;
                        }
                        TotalRowsSentForUpdate++;
                        IsChangeFound = false;
                    }
                    #endregion
                }

                structUpdateTableInfo.ErrorMessage = "";
                structUpdateTableInfo.ErrorSQLStatement = "";
                structUpdateTableInfo.ErrorRow = null;
                structUpdateTableInfo.Status = true;
                structUpdateTableInfo.TotalRowsAffected = TotalRowsSentForUpdate;

                return structUpdateTableInfo;
            }
            catch (Exception e)
            {
                comm.Transaction.Rollback();
                throw (e);
            }
        }

        public static UpdateTableInfo UpdateTable(ref IDbCommand comm, DataTable dt, UpdateWhereMode updateWhereMode, UpdateMethod updateMethod)
        {
            UpdateTableInfo structUpdateTableInfo;
            try
            {
                #region Initial Validations
                if (comm == null)
                {
                    structUpdateTableInfo.ErrorMessage = "Command object is not initialized.Operation cancelled.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                if (comm.Connection == null)
                {
                    structUpdateTableInfo.ErrorMessage = "Database connection object is not set.Operation cancelled.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                if (comm.Connection.State != ConnectionState.Open)
                {
                    structUpdateTableInfo.ErrorMessage = "Database connection is not open.Operation cancelled.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                if (comm.Transaction == null)
                {
                    structUpdateTableInfo.ErrorMessage = "Transaction object is not initialized.Operation cancelled.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                if (dt == null)
                {
                    structUpdateTableInfo.ErrorMessage = "No Table to Update.";
                    structUpdateTableInfo.ErrorSQLStatement = "";
                    structUpdateTableInfo.ErrorRow = null;
                    structUpdateTableInfo.Status = false;
                    structUpdateTableInfo.TotalRowsAffected = 0;
                    return structUpdateTableInfo;
                }
                #endregion //Initial Validations

                int TotalRowsSentForUpdate = 0;


                foreach (DataRow drow in dt.Rows)
                {
                    String SQL = "";
                    String SQLDelete = "";
                    Boolean IsChangeFound = false;
                    String SetColumnValues = "";
                    String WhereColumns = "";

                    switch (drow.RowState)
                    {
                        #region Query Generation for Insert
                        case DataRowState.Added:
                            comm.Parameters.Clear();
                            IsChangeFound = true;
                            String ColumnNames = "";
                            String ParameterNames = "";


                            foreach (DataColumn dcol in dt.PrimaryKey)
                            {

                                WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + "1 AND ";
                                IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                KeyValueParam.ParameterName = "@" + dcol.ColumnName + "1";
                                KeyValueParam.Value = drow[dcol];//, DataRowVersion.Original];
                                comm.Parameters.Add(KeyValueParam);

                            }
                            //Remove the last AND from WHERE string
                            if (WhereColumns != "")
                                WhereColumns = WhereColumns.Substring(0, WhereColumns.Length - 4);
                            else
                            {
                                IsChangeFound = false;
                                break;
                            }

                            SQLDelete = "DELETE " + dt.TableName +
                                        " WHERE " + WhereColumns;

                            foreach (DataColumn dcol in dt.Columns)
                            {
                                if (dcol.Ordinal < dt.Columns.Count - 1)
                                {
                                    ColumnNames += dcol.ColumnName + ",";
                                    ParameterNames += "@" + dcol.ColumnName + ",";
                                }
                                else
                                {
                                    ColumnNames += dcol.ColumnName;
                                    ParameterNames += "@" + dcol.ColumnName;
                                }

                                IDataParameter param = DBObjectFactory.GetParameterObject();
                                param.ParameterName = "@" + dcol.ColumnName;
                                param.Value = drow[dcol, DataRowVersion.Current];
                                comm.Parameters.Add(param);
                            }
                            if (ColumnNames == "" || ParameterNames == "")
                                break;

                            SQL = "INSERT INTO " + dt.TableName +
                                    " ( " + ColumnNames + ") VALUES (" + ParameterNames + ")";

                            //MessageBox.Show(SQL, "INSERT");
                            break;
                        #endregion

                        #region Query Generation for Delete
                        case DataRowState.Deleted:
                            comm.Parameters.Clear();
                            IsChangeFound = true;
                            String DeleteWhereColumns = "";

                            //Set the Primary Key Field
                            foreach (DataColumn dcol in dt.PrimaryKey)
                            {
                                DeleteWhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + " AND ";
                                IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                KeyValueParam.ParameterName = "@" + dcol.ColumnName;
                                KeyValueParam.Value = drow[dcol, DataRowVersion.Original];
                                comm.Parameters.Add(KeyValueParam);
                            }
                            if (DeleteWhereColumns != "")
                                //Remove the last AND from WHERE string
                                DeleteWhereColumns = DeleteWhereColumns.Substring(0, DeleteWhereColumns.Length - 4);
                            else
                            {
                                IsChangeFound = false;
                                break;
                            }

                            SQL = "DELETE FROM " + dt.TableName + " WHERE " + DeleteWhereColumns;

                            //MessageBox.Show(SQL, "DELETE");
                            break;

                        #endregion

                        #region Query Generation for Update
                        case DataRowState.Modified:
                            comm.Parameters.Clear();
                            IsChangeFound = true;


                            if (updateMethod == UpdateMethod.Update)
                            {
                                ////When the Update method is Update

                                #region When Update Method is Update

                                if (updateWhereMode == UpdateWhereMode.KeyAndModifiedColumns)
                                {
                                    #region KeyAndModifiedColumns
                                    foreach (DataColumn dcol in dt.Columns)
                                    {
                                        //Form the SET part for the columns whose Current Value != Original Value
                                        if (drow[dcol, DataRowVersion.Current].ToString() != drow[dcol, DataRowVersion.Original].ToString())
                                        {
                                            SetColumnValues += dcol.ColumnName + " = @" + dcol.ColumnName + ",";
                                            IDataParameter NewValueParam = DBObjectFactory.GetParameterObject();
                                            NewValueParam.ParameterName = "@" + dcol.ColumnName;
                                            NewValueParam.Value = drow[dcol, DataRowVersion.Current];
                                            comm.Parameters.Add(NewValueParam);

                                            if (drow[dcol, DataRowVersion.Original] != System.DBNull.Value)
                                            {
                                                WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + "1 AND ";
                                                IDataParameter OldValueParam = DBObjectFactory.GetParameterObject();
                                                OldValueParam.ParameterName = "@" + dcol.ColumnName + "1";
                                                OldValueParam.Value = drow[dcol, DataRowVersion.Original];
                                                comm.Parameters.Add(OldValueParam);
                                            }
                                            else
                                                WhereColumns += dcol.ColumnName + " IS NULL AND ";
                                        }
                                    }
                                    //Remove the last comma from SET string
                                    if (SetColumnValues != "")
                                        SetColumnValues = SetColumnValues.Substring(0, SetColumnValues.Length - 1);
                                    else
                                    {
                                        IsChangeFound = false;
                                        break;
                                    }

                                    //Set the Primary Key Field
                                    foreach (DataColumn dcol in dt.PrimaryKey)
                                    {
                                        //Avoid including those PK fields in WHERE clause if they are modified as
                                        //this is already done in previous loop
                                        if (drow[dcol, DataRowVersion.Current] == drow[dcol, DataRowVersion.Original])
                                        {
                                            WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + " AND ";
                                            IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                            KeyValueParam.ParameterName = "@" + dcol.ColumnName;
                                            KeyValueParam.Value = drow[dcol, DataRowVersion.Original];
                                            comm.Parameters.Add(KeyValueParam);
                                        }
                                    }
                                    //Remove the last AND from WHERE string
                                    if (WhereColumns != "")
                                        WhereColumns = WhereColumns.Substring(0, WhereColumns.Length - 4);
                                    else
                                    {
                                        IsChangeFound = false;
                                        break;
                                    }
                                    #endregion //KeyAndModifiedColumns
                                }
                                else if (updateWhereMode == UpdateWhereMode.KeyColumnsOnly)
                                {
                                    #region KeyColumnsOnly
                                    foreach (DataColumn dcol in dt.Columns)
                                    {
                                        //Form the SET part for the columns whose Current Value != Original Value
                                        SetColumnValues += dcol.ColumnName + " = @" + dcol.ColumnName + ",";
                                        IDataParameter NewValueParam = DBObjectFactory.GetParameterObject();
                                        NewValueParam.ParameterName = "@" + dcol.ColumnName;
                                        NewValueParam.Value = drow[dcol, DataRowVersion.Current];
                                        comm.Parameters.Add(NewValueParam);
                                    }
                                    //Remove the last comma from SET string
                                    if (SetColumnValues != "")
                                        SetColumnValues = SetColumnValues.Substring(0, SetColumnValues.Length - 1);
                                    else
                                    {
                                        IsChangeFound = false;
                                        break;
                                    }

                                    //Set the Primary Key Field
                                    foreach (DataColumn dcol in dt.PrimaryKey)
                                    {
                                        WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + "1 AND ";
                                        IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                        KeyValueParam.ParameterName = "@" + dcol.ColumnName + "1";
                                        KeyValueParam.Value = drow[dcol, DataRowVersion.Original];
                                        comm.Parameters.Add(KeyValueParam);
                                    }
                                    //Remove the last AND from WHERE string
                                    if (WhereColumns != "")
                                        WhereColumns = WhereColumns.Substring(0, WhereColumns.Length - 4);
                                    else
                                    {
                                        IsChangeFound = false;
                                        break;
                                    }
                                    #endregion //KeyColumnsOnly
                                }
                                SQL = "UPDATE " + dt.TableName +
                                        " SET " + SetColumnValues + " WHERE " + WhereColumns;

                                #endregion
                            }
                            else if (updateMethod == UpdateMethod.DeleteAndInsert)
                            {
                                ////When the Update method is Delete and Insert

                                if (updateWhereMode == UpdateWhereMode.KeyAndModifiedColumns)
                                {
                                    #region KeyAndModifiedColumns
                                    foreach (DataColumn dcol in dt.Columns)
                                    {
                                        if (drow[dcol, DataRowVersion.Current].ToString() != drow[dcol, DataRowVersion.Original].ToString())
                                        {
                                            if (drow[dcol, DataRowVersion.Original] != System.DBNull.Value)
                                            {
                                                WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + "1 AND ";
                                                IDataParameter OldValueParam = DBObjectFactory.GetParameterObject();
                                                OldValueParam.ParameterName = "@" + dcol.ColumnName + "1";
                                                OldValueParam.Value = drow[dcol, DataRowVersion.Original];
                                                comm.Parameters.Add(OldValueParam);
                                            }
                                            else
                                                WhereColumns += dcol.ColumnName + " IS NULL AND ";

                                        }
                                    }
                                    //Set the Primary Key Field
                                    foreach (DataColumn dcol in dt.PrimaryKey)
                                    {
                                        //Avoid including those PK fields in WHERE clause if they are modified as
                                        //this is already done in previous loop
                                        if (drow[dcol, DataRowVersion.Current] == drow[dcol, DataRowVersion.Original])
                                        {
                                            WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + " AND ";
                                            IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                            KeyValueParam.ParameterName = "@" + dcol.ColumnName;
                                            KeyValueParam.Value = drow[dcol, DataRowVersion.Original];
                                            comm.Parameters.Add(KeyValueParam);
                                        }
                                    }
                                    //Remove the last AND from WHERE string
                                    if (WhereColumns != "")
                                        WhereColumns = WhereColumns.Substring(0, WhereColumns.Length - 4);
                                    else
                                    {
                                        IsChangeFound = false;
                                        break;
                                    }
                                    #region
                                }
                                else if (updateWhereMode == UpdateWhereMode.KeyColumnsOnly)
                                {
                                    //Set the Primary Key Field
                                    foreach (DataColumn dcol in dt.PrimaryKey)
                                    {

                                        WhereColumns += dcol.ColumnName + " = @" + dcol.ColumnName + "1 AND ";
                                        IDataParameter KeyValueParam = DBObjectFactory.GetParameterObject();
                                        KeyValueParam.ParameterName = "@" + dcol.ColumnName + "1";
                                        KeyValueParam.Value = drow[dcol, DataRowVersion.Original];
                                        comm.Parameters.Add(KeyValueParam);

                                    }
                                    //Remove the last AND from WHERE string
                                    if (WhereColumns != "")
                                        WhereColumns = WhereColumns.Substring(0, WhereColumns.Length - 4);
                                    else
                                    {
                                        IsChangeFound = false;
                                        break;
                                    }
                                }

                                SQLDelete = "DELETE " + dt.TableName +
                                        " WHERE " + WhereColumns;


                                //Build the Insert Query
                                String Columns = "";
                                foreach (DataColumn dcol in dt.Columns)
                                {
                                    //Form the SET part for the columns whose Current Value != Original Value
                                    Columns += dcol.ColumnName + ",";
                                    SetColumnValues += "@" + dcol.ColumnName + ",";
                                    IDataParameter NewValueParam = DBObjectFactory.GetParameterObject();
                                    NewValueParam.ParameterName = "@" + dcol.ColumnName;
                                    NewValueParam.Value = drow[dcol, DataRowVersion.Current];
                                    comm.Parameters.Add(NewValueParam);
                                }
                                ///To remove the last comma in the Set Statement
                                if (SetColumnValues != "")
                                    SetColumnValues = SetColumnValues.Substring(0, SetColumnValues.Length - 1);
                                else
                                {
                                    IsChangeFound = false;
                                    break;
                                }
                                if (Columns != "")
                                    Columns = Columns.Substring(0, Columns.Length - 1);
                                else
                                {
                                    IsChangeFound = false;
                                    break;
                                }


                                SQL = "INSERT INTO " + dt.TableName +
                                      " ( " + Columns + " ) VALUES ( " + SetColumnValues + " ) ";
                                    #endregion
                            }
                                    #endregion
                            break;
                    }

                    #region Fire the query
                    if (IsChangeFound)
                    {
                        if (updateMethod == UpdateMethod.DeleteAndInsert)
                        {
                            if (SQLDelete.Trim() != "")
                            {
                                comm.CommandType = CommandType.Text;
                                comm.CommandText = SQLDelete;
                                int SqlTest = comm.ExecuteNonQuery();
                                //if (comm.ExecuteNonQuery() != 1)
                                //System.Windows.Forms.MessageBox.Show(muck.ToString());
                                if (SqlTest != 1 && SqlTest != 0)
                                {

                                    structUpdateTableInfo.ErrorMessage = "Database updation failed.";
                                    structUpdateTableInfo.ErrorSQLStatement = SQL;
                                    structUpdateTableInfo.ErrorRow = drow;
                                    structUpdateTableInfo.Status = false;
                                    structUpdateTableInfo.TotalRowsAffected = TotalRowsSentForUpdate;
                                    return structUpdateTableInfo;
                                }
                            }

                        }

                        comm.CommandType = CommandType.Text;
                        comm.CommandText = SQL;
                        if (comm.ExecuteNonQuery() != 1)
                        {
                            structUpdateTableInfo.ErrorMessage = "Database updation failed.";
                            structUpdateTableInfo.ErrorSQLStatement = SQL;
                            structUpdateTableInfo.ErrorRow = drow;
                            structUpdateTableInfo.Status = false;
                            structUpdateTableInfo.TotalRowsAffected = TotalRowsSentForUpdate;
                            return structUpdateTableInfo;
                        }


                        TotalRowsSentForUpdate++;
                        IsChangeFound = false;
                    }

                    #endregion
                }
                        #endregion
                structUpdateTableInfo.ErrorMessage = "";
                structUpdateTableInfo.ErrorSQLStatement = "";
                structUpdateTableInfo.ErrorRow = null;
                structUpdateTableInfo.Status = true;
                structUpdateTableInfo.TotalRowsAffected = TotalRowsSentForUpdate;


                return structUpdateTableInfo;
            }
            catch (Exception e)
            {
                comm.Transaction.Rollback();
                throw (e);
            }


        }

        public static IDataParameter MakeParameter(String ParameterName, DbType ParameterType, Object ParameterValue)
        {
            IDataParameter parameter = DBObjectFactory.GetParameterObject();
            parameter.ParameterName = ParameterName;
            parameter.DbType = ParameterType;
            parameter.Value = ParameterValue;
            return parameter;
        }

        public static ArrayList GetDistinctColumnValues(DataTable dt, string ColumnName)
        {
            ArrayList UniqueValues = new ArrayList();
            string sortorder = ColumnName + " DESC";
            dt.DefaultView.Sort = sortorder;
            DataTable sorted_table = dt.DefaultView.ToTable();
            UniqueValues.Add(sorted_table.Rows[0][ColumnName].ToString());
            for (int i = 0; i < sorted_table.Rows.Count; i++)
            {
                if (sorted_table.Rows[i][ColumnName].ToString() == UniqueValues[UniqueValues.Count - 1].ToString())
                    continue;
                else
                    UniqueValues.Add(sorted_table.Rows[i][ColumnName].ToString());
            }
            return UniqueValues;
        }


        public static string return_ajax_string(string status, string message)
        {

            //return_ajax_string("false", "You have not completed the feedback, please fill the feedback to view your grades");
            return "{\"status\":\"" + status + "\",\"message\":\"" + message + "\"}";

        }

        public static string return_ajax_data(string status, string json_data)
        {
            return "{\"status\":\"" + status + "\",\"message\":" + json_data + "}";
        }

     
        public static Boolean IsNumeric(Object obj)
        {
            return Information.IsNumeric(obj);
        }
        #endregion
    }
}
