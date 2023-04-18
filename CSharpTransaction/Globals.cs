using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTransaction
{
    /* Dispose edebilmek için Component sınıfından kalıtım aldık. */
    public class Globals:Component
    {
        private string _DBName;
        private string _LogoURL;
        public string _WebSiteHeader;
        public enum SQLType
        {
            MSSQL = 0,
            Oracle =1,
            OleDB =2,
            SyBase = 3
        }
        public enum ISOLEVEL
        {
            Read_Uncommitted = 0,
            Read_Committed =1
        }
        public static SQLType SQL = SQLType.MSSQL;

        private static string mConnectionString = "";

        public SqlConnection cnSQL;
        public Oracle.ManagedDataAccess.Client.OracleConnection cnOrSQL;
        public OleDbConnection oleDBSQL;

        public static string ConnectionString
        {
            get
            {
                return mConnectionString;
            }
            set
            {
                mConnectionString = value;
            }
        }
        // ConnectionString
        [Browsable(true)]
        [Category("User Defined")]
        [Description("to change DB Name")]
        public string DBName
        {
            get
            {
                return _DBName;
            }
            set
            {
                _DBName = value;
            }
        }
        [Browsable(true)]
        [Category("User Defined")]
        [Description("Web Site Company Logo URL")]
        public string LogoURL
        {
            get
            {
                return _LogoURL;
            }
            set
            {
                _LogoURL = value;
            }
        }
        [Browsable(true)]
        [Category("User Defined")]
        [Description("Web Site Header")]
        public string WebSiteHeader
        {
            get
            {
                return _WebSiteHeader;
            }
            set
            {
                _WebSiteHeader = value;
            }
        }
        public static string ReadFromConfig(string ConnString)
        {
            string ConfigValue = ConfigurationManager.AppSettings[ConnString];
            return ConfigValue;
        }
        private SqlTransaction _Transaction = null;
        private bool _disposed = false;
        private Oracle.ManagedDataAccess.Client.OracleTransaction _OracleTransaction = null/* TODO Change to default(_) if this is not a reference type */;
        private System.Data.OleDb.OleDbTransaction _OleDBTransaction = null;  //

        public SqlTransaction Transaction
        {
            get
            {
                return _Transaction;
            }
            set
            {
                _Transaction = value;
            }
        } // Transaction
        public Oracle.ManagedDataAccess.Client.OracleTransaction OracleTransaction
        {
            get
            {
                return _OracleTransaction;
            }
            set
            {
                _OracleTransaction = value;
            }
        } // OracleTransaction
        public System.Data.OleDb.OleDbTransaction OLEDBTransaction
        {
            get
            {
                return _OleDBTransaction;
            }
            set
            {
                _OleDBTransaction = value;
            }
        } // OracleTransaction

        public bool BeginTransaction(IsolationLevel isolation_Level = IsolationLevel.ReadUncommitted)
        {
            try
            {
                if (SQL == SQLType.MSSQL)
                {
                    SqlConnection cn = new SqlConnection(ConnectionString);

                    cn.Open();
                    cnSQL = cn;


                    Transaction = cn.BeginTransaction(isolation_Level);

                    return true;
                }
                else if (SQL == SQLType.Oracle)
                {
                    Oracle.ManagedDataAccess.Client.OracleConnection cn = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString);
                    cn.Open();
                    cnOrSQL = cn;
                    OracleTransaction = cn.BeginTransaction(isolation_Level);
                    return true;
                }
                else if (SQL == SQLType.OleDB)
                {
                    System.Data.OleDb.OleDbConnection cn = new System.Data.OleDb.OleDbConnection(ConnectionString);
                    cn.Open();
                    OLEDBTransaction = cn.BeginTransaction();
                    return true;
                }
            }

            catch (Exception ex)
            {
                return false;
            }
            return false;
        } // BeginTransaction
        
        public bool CommitTransaction()
        {
            try
            {
                if (SQL == SQLType.MSSQL)
                {
                    if (Transaction == null)
                        throw new Exception("Programming error. Cannot commit transaction because the Sql Server Transaction object is nothing.");
                    Transaction.Commit();
                    Transaction = null;
                    cnSQL.Close();
                    this.Dispose();
                    return true;
                }
                else if (SQL == SQLType.Oracle)
                {
                    if (OracleTransaction == null)
                        throw new Exception("Programming error. Cannot commit transaction because the Oracle Transaction object is nothing.");
                    OracleTransaction.Commit();
                    OracleTransaction = null/* TODO Change to default(_) if this is not a reference type */;
                    cnOrSQL.Close();
                    this.Dispose();
                    return true;
                }
                else if (SQL == SQLType.OleDB)
                {
                    if (OLEDBTransaction == null)
                        throw new Exception("Programming error. Cannot commit transaction because the OleDB Transaction object is nothing.");
                    OLEDBTransaction.Commit();
                    OLEDBTransaction = null/* TODO Change to default(_) if this is not a reference type */;
                    oleDBSQL.Close();
                    this.Dispose();
                    return true;
                }
            }

            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public bool RollbackTransaction()
        {
            try
            {
                if (SQL == SQLType.MSSQL)
                {
                    if (Transaction == null)
                        throw new Exception("Programming error. Cannot commit transaction because the SQL Server Transaction object is nothing.");
                    Transaction.Rollback();
                    Transaction = null;
                    cnSQL.Close();
                    this.Dispose();
                    return true;
                }
                else if (SQL == SQLType.Oracle)
                {
                    if (OracleTransaction == null)
                        throw new Exception("Programming error. Cannot commit transaction because the Oracle Transaction object is nothing.");
                    OracleTransaction.Rollback();
                    OracleTransaction = null/* TODO Change to default(_) if this is not a reference type */;
                    cnOrSQL.Close();
                    this.Dispose();
                    return true;
                }
                else if (SQL == SQLType.OleDB)
                {
                    if (OLEDBTransaction == null)
                        throw new Exception("Programming error. Cannot commit transaction because the OLEDB Transaction object is nothing.");
                    OLEDBTransaction.Rollback();
                    OLEDBTransaction = null/* TODO Change to default(_) if this is not a reference type */;
                    this.Dispose();
                    return true;
                }
            }

            catch (Exception ex)
            {
                // ExceptionManager.Publish(ex)
                return false;
            }
            return false;
        } // RollbackTransaction

        public  bool ExecuteCommandQuery(string sql, SQLType sqlType)
        {
            IDbCommand dbCommand = null;
            BeginTransaction(IsolationLevel.ReadUncommitted);
            if (sqlType == SQLType.MSSQL)
            {
                SQL = SQLType.MSSQL;
                dbCommand = new SqlCommand(sql,cnSQL);
                dbCommand.Transaction = Transaction;
            }
            if (sqlType == SQLType.OleDB)
            {
                SQL = SQLType.OleDB;
                dbCommand = new OleDbCommand(sql,oleDBSQL);
                dbCommand.Transaction = OLEDBTransaction;
            }
            if (sqlType == SQLType.Oracle)
            {
                SQL = SQLType.Oracle;
                dbCommand = new OracleCommand(sql,cnOrSQL);
                dbCommand.Transaction = OracleTransaction;

            }
            
            try
            {
                dbCommand?.ExecuteNonQuery();
                CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                return false;
            }
        }

       
    }
}
