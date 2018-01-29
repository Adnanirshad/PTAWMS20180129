using PTAWMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for SQLDBAccess
/// </summary>
/// 
namespace PTAWMS.App_Start
{
    public class SQLDBAccess
    {
        private SqlConnection mobjConnection = new SqlConnection();
        private SqlConnection mobjTransConn = new SqlConnection();
        public SQLDBAccess()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public SqlConnection GetConnection()
        {
            string conn = string.Empty;
            using (HRMEntities db = new HRMEntities())
            {
                conn = db.Database.Connection.ConnectionString;
            }

            if (mobjConnection.State == ConnectionState.Open) mobjConnection.Close();
            mobjConnection.ConnectionString = conn;
            mobjConnection.Open();


            return mobjConnection;
        }
        public static string GetEncashmentDate()
        {
            return ConfigurationManager.ConnectionStrings["EncashmentDate"].ConnectionString;
        }
        public static DateTime GetEncashmentDeadLine()
        {
            return DateTime.ParseExact(ConfigurationManager.ConnectionStrings["EncashmentDate"].ConnectionString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public void PopulateCombo(DropDownList cbo, DataSet dSet, int Datasets_Table_Index, string _DisplayMember, string _ValueMember)
        {
            cbo.DataSource = dSet.Tables[Datasets_Table_Index];
            cbo.DataTextField = _DisplayMember;
            cbo.DataValueField = _ValueMember;
            cbo.DataBind();
        }
        public void PopulateList(ListBox cbo, DataSet dSet, string Datasets_Table_Name, string _DisplayMember, string _ValueMember)
        {
            cbo.DataSource = dSet.Tables[Datasets_Table_Name];
            cbo.DataTextField = _DisplayMember;
            cbo.DataValueField = _ValueMember;
            cbo.DataBind();

        }
        public void PopulateCombo(DropDownList cbo, DataSet dSet, string Datasets_Table_Name, string _DisplayMember, string _ValueMember)
        {
            cbo.DataSource = dSet.Tables[Datasets_Table_Name];
            cbo.DataTextField = _DisplayMember;
            cbo.DataValueField = _ValueMember;
            cbo.DataBind();
        }

        public void DisplayColumnsinGrid(GridView DataGrid, string[] ColumnName)
        {
            DataTable dTable = new DataTable();
            DataSet dSet = new DataSet();
            DataColumn dColumn;
            int I;

            dTable = dSet.Tables.Add("Table");
            for (I = 0; I <= ColumnName.GetUpperBound(0); I++)
            {
                dColumn = new DataColumn();
                dColumn.ColumnName = ColumnName[I];
                dTable.Columns.Add(dColumn);
            }
            DataGrid.DataSource = dSet;
            DataGrid.DataMember = "TABLE";
            DataGrid.DataBind();
        }

        public DataSet FillExcelDataSet(string filename)
        {
            OleDbConnection objConn = null;
            String sConnectionString = "";

            if (filename.EndsWith("xls"))
                sConnectionString = string.Format(ConfigurationManager.ConnectionStrings["xls"].ConnectionString, filename);
            else
                sConnectionString = string.Format(ConfigurationManager.ConnectionStrings["xlsx"].ConnectionString, filename);

            DataSet dSet = new DataSet();

            try
            {
                objConn = new OleDbConnection(sConnectionString);
                objConn.Open();
                DataTable dtSheetName = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtSheetName == null || dtSheetName.Rows.Count == 0)
                {
                    if (objConn.State == ConnectionState.Open)
                        objConn.Close();
                    throw new Exception("Unable to find sheet in the selected file.");
                }
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT DISTINCT * FROM [" + dtSheetName.Rows[0]["TABLE_NAME"].ToString() + "]", objConn);

                OleDbDataAdapter OleAdapter = new OleDbDataAdapter();
                OleAdapter.SelectCommand = objCmdSelect;
                OleAdapter.Fill(dSet);
                return dSet;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objConn.Close();
            }
            // dSet.Tables.Count

        }

        public DataSet FillDataset(string sqlQuery, string strTableName)
        {
            DataSet dSet = new DataSet();
            try
            {
                SqlDataAdapter dAdapter = new SqlDataAdapter();
                SqlCommand cmdSelect = new SqlCommand();
                if (mobjConnection.State == ConnectionState.Closed)
                {
                    GetConnection();
                }
                cmdSelect.Connection = mobjConnection;
                cmdSelect.CommandText = sqlQuery;
                dAdapter.SelectCommand = cmdSelect;
                if (strTableName == "")
                {
                    dAdapter.Fill(dSet);
                }
                else
                {
                    dAdapter.Fill(dSet, strTableName);
                }
                return dSet;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }

        }

        public void BindGrid(GridView DGrid, DataSet dset, string TableName)
        {

            DGrid.DataSource = dset.Tables[TableName].DefaultView;
            DGrid.DataBind();


        }

        public int GetDataSetRowIndex(DataSet dset, string strTable, int GridRowIndex)
        {
            int counter = -1;
            int i = 0;
            for (; i <= dset.Tables[strTable].Rows.Count - 1; i++)
            {
                if (dset.Tables[strTable].Rows[i].RowState != DataRowState.Deleted)
                {
                    counter = counter + 1;
                    if (counter == GridRowIndex)
                    {
                        return i;
                    }
                }
            }
            return i;
        }

        public SqlDataReader FillDataReader(string strQuery)
        {
            string eror = "nothing"; int val = 0;
            SqlCommand cmCommand = null;
            try
            {
                if (mobjConnection.State == ConnectionState.Closed)
                {
                    GetConnection();
                }
                cmCommand = new SqlCommand(strQuery, mobjConnection);
                return cmCommand.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public int ExecuteProcedure(string ProcName)
        {
            string eror = "nothing"; int val = 0;
            try
            {
                if (mobjConnection.State == ConnectionState.Closed)
                {
                    GetConnection();
                }
                SqlCommand cmCommand = new SqlCommand(ProcName, mobjConnection);
                cmCommand.CommandType = CommandType.StoredProcedure;
                return cmCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ExecuteQuery(string strSql, ref SqlTransaction TransObject)
        {
            SqlCommand cmd = null;
            bool flg = false;
            try
            {

                if (TransObject == null)
                    getTransaction(ref TransObject);
                cmd = TransObject.Connection.CreateCommand();
                cmd.Transaction = TransObject;
                cmd.CommandText = strSql;
                if (cmd.ExecuteNonQuery() > 0)
                    flg = true;

                return flg;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int ExecuteQueryReturnID(string strSql, ref SqlTransaction TransObject)
        {
            SqlCommand cmd = null;
            bool flg = false;
            try
            {

                if (TransObject == null)
                    getTransaction(ref TransObject);
                cmd = TransObject.Connection.CreateCommand();
                cmd.Transaction = TransObject;
                cmd.CommandText = strSql;
                return (int)(decimal)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw;
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool ExecuteQuery(string strSql)
        {
            SqlCommand cmd = new SqlCommand();
            bool flg = false;
            try
            {
                if (mobjConnection.State == ConnectionState.Closed)
                {
                    GetConnection();
                }
                cmd.Connection = mobjConnection;
                cmd.CommandText = strSql;
                if (cmd.ExecuteNonQuery() > 0)
                    flg = true;
                return flg;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public string GetMaxID(string ID, string RefID, string RefVal, string TableName)
        {
            string result = "", strSql;
            SqlDataReader reader = null;
            strSql = "SELECT isnull(MAX(cast(" + ID + " as INT)),0)+ 1 AS ID FROM " + TableName + "  with(nolock) WHERE " + RefID + " = '" + RefVal + "'";
            try
            {
                reader = FillDataReader(strSql);
                if (reader != null && reader.Read())
                {
                    result = reader["ID"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseDataReader(ref reader);
            }

        }
        public string GetMaxID(string ID, string TableName)
        {
            string result = "", strSql;
            SqlDataReader reader = null;

            strSql = "SELECT MAX(Cast(isnull(" + ID + ",0) as INT))+ 1 AS ID FROM " + TableName + "  with(nolock)";
            try
            {
                reader = FillDataReader(strSql);
                if (reader != null && reader.Read())
                {
                    result = reader["ID"].ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                reader.Close();
                reader.Dispose();
                CloseConnection();
            }

        }
        public string GetMaxSEQ(string SEQID)
        {
            string result = "", strSql;
            SqlDataReader reader = null;

            strSql = "SELECT " + SEQID + ".NEXTVAL AS ID FROM DUAL";
            try
            {
                reader = FillDataReader(strSql);
                if (reader != null && reader.Read())
                {
                    result = reader["ID"].ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                reader.Close();
                reader.Dispose();
                CloseConnection();
            }

        }
        public string GetLogTypeID(string LogName)
        {
            string result = "", strSql;
            SqlDataReader reader = null;
            strSql = "SELECT LOGTYPID FROM TBLLOG_TYPE WHERE LOGNAME = '" + LogName + "'";
            try
            {
                reader = FillDataReader(strSql);
                if (reader != null && reader.Read())
                {
                    result = reader["LOGTYPID"].ToString();

                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                reader.Close();
                reader.Dispose();
                CloseConnection();
            }

        }
        public void getTransaction(ref SqlTransaction TransObject)
        {
            if (TransObject == null)
            {
                if (mobjTransConn.State == ConnectionState.Open) mobjTransConn.Close();
                mobjTransConn.ConnectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
                mobjTransConn.Open();
                TransObject = mobjTransConn.BeginTransaction();
            }

        }

        public void Commit_Transaction(ref SqlTransaction TransObject)
        {
            try
            {
                TransObject.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TransObject = null;
                CloseConnection();
            }
        }

        public void Rollback_Transaction(ref SqlTransaction TransObject)
        {
            try
            {
                TransObject.Rollback();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TransObject = null;
                CloseConnection();
            }
        }

        public void CloseDataReader(ref SqlDataReader mobjReader)
        {
            mobjReader.Close();
            mobjReader.Dispose();
            CloseConnection();
        }
        public void CloseConnection()
        {
            if (mobjConnection.State == ConnectionState.Open) mobjConnection.Close();
        }
        public bool SendEmail(String From, String To, String message, String Subject)
        {
            bool retFlg = true;

            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress(To));
                if (To.Equals("chairman@pta.gov.pk"))
                    msg.CC.Add(new MailAddress("wasi@pta.gov.pk"));
                // msg.Cc = "ursalman@gmail.com";
                msg.From = new MailAddress(From, "E-Leave Portal");
                msg.Subject = Subject;
                msg.Body = message;
                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings.Get("mailserver"));
                //smtp.Send(msg);
                return retFlg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SendEmail(String From, String To, String BCC, String message, String Subject)
        {
            bool retFlg = true;

            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress(To));
                if (To.Equals("chairman@pta.gov.pk"))
                    msg.CC.Add(new MailAddress("wasi@pta.gov.pk"));
                msg.Bcc.Add(new MailAddress(BCC));
                // msg.Cc = "ursalman@gmail.com";
                msg.From = new MailAddress(From, "E-Leave Portal");
                msg.Subject = Subject;
                msg.Body = message;
                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings.Get("mailserver"));
                //smtp.Send(msg);
                return retFlg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}