using System.Configuration;
using System.Data.SqlClient;

namespace CSharpTransaction
{
    public partial class Form1 : Form
    {
        public Globals globals { get; set; }
        public Form1()
        {
            if(globals == null)
                globals = new Globals();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // SELECT * FROM Products with (NOLOCK) => Kirli kodlarý görmek için kullanýlýr.Commit edilmeden önceki kayýtlarý da görmemizi saðlar.Tablo commit olmadýðý için kilitlendiðinde kullanýlýr.

            //DBCC USEROPTIONS

            //SET TRANSACTION ISOLATION LEVEL READ COMMITTED

            //string connStr = ConfigurationManager.AppSettings["ConnStr"];
            //string sql = "INSERT INTO Products(ProductName,CategoryID,UnitPrice,UnitsInStock) VALUES('OnurYRDGLN',2,89,20)";
            //string sql2 = "INSERT INTO Productss(ProductName,CategoryID,UnitPrice,UnitsInStock) VALUES('OnurLYRDGLN2',2,89,20)";
            //SqlConnection conn = new SqlConnection(connStr);
            //SqlCommand sqlCommand = new SqlCommand(sql, conn);

            //try
            //{
            //    conn.Open();
            //    sqlCommand.Connection = conn;
            //    sqlCommand.Transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            //    //ReadUncommited => baþkalarýnýn ve kendinizin attýðý insert'larý görmek için ReadUncommitted kullanýlýr.
            //    sqlCommand.ExecuteNonQuery(); //sql sorgusunu çalýþtýrýr.
            //    sqlCommand.CommandText = sql2;
            //    sqlCommand.ExecuteNonQuery();//sql2 sorgusunu çalýþtýrýr.
            //    sqlCommand.Transaction.Commit();
            //    conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    sqlCommand.Transaction.Rollback();
            //    conn.Close();
            //}

            //2.Yöntem

            string SqlType = Globals.ReadFromConfig("SqlType");
            Globals.SQL = SqlType == "MSSQL" ? Globals.SQLType.MSSQL : SqlType == "ORACLE" ? Globals.SQLType.Oracle : Globals.SQLType.OleDB;
            Globals.ConnectionString = Globals.ReadFromConfig("ConnStr");
            string sql = "INSERT INTO Products(ProductName,CategoryID,UnitPrice,UnitsInStock) VALUES('OnurYRDGLN',2,89,20)";
            string sql2 = "INSERT INTO Products(ProductName,CategoryID,UnitPrice,UnitsInStock) VALUES('OnurLYRDGLN2',2,89,20)";

            globals.ExecuteCommandQuery(sql2, Globals.SQLType.MSSQL);
            //SqlCommand sqlCommand = new SqlCommand(sql);
            //globals.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

            //try
            //{
            //    sqlCommand.Connection = globals.cnSQL;
            //    sqlCommand.Transaction = globals.Transaction;
            //    sqlCommand.ExecuteNonQuery();
            //    //ReadUncommited => baþkalarýnýn ve kendinizin attýðý insert'larý görmek için ReadUncommitted kullanýlýr.
            //    sqlCommand.CommandText = sql2;
            //    sqlCommand.ExecuteNonQuery();//sql2 sorgusunu çalýþtýrýr.
            //    globals.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    globals.RollbackTransaction();
            //}



        }
    }
}