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
            // SELECT * FROM Products with (NOLOCK) => Kirli kodlar� g�rmek i�in kullan�l�r.Commit edilmeden �nceki kay�tlar� da g�rmemizi sa�lar.Tablo commit olmad��� i�in kilitlendi�inde kullan�l�r.

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
            //    //ReadUncommited => ba�kalar�n�n ve kendinizin att��� insert'lar� g�rmek i�in ReadUncommitted kullan�l�r.
            //    sqlCommand.ExecuteNonQuery(); //sql sorgusunu �al��t�r�r.
            //    sqlCommand.CommandText = sql2;
            //    sqlCommand.ExecuteNonQuery();//sql2 sorgusunu �al��t�r�r.
            //    sqlCommand.Transaction.Commit();
            //    conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    sqlCommand.Transaction.Rollback();
            //    conn.Close();
            //}

            //2.Y�ntem

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
            //    //ReadUncommited => ba�kalar�n�n ve kendinizin att��� insert'lar� g�rmek i�in ReadUncommitted kullan�l�r.
            //    sqlCommand.CommandText = sql2;
            //    sqlCommand.ExecuteNonQuery();//sql2 sorgusunu �al��t�r�r.
            //    globals.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    globals.RollbackTransaction();
            //}



        }
    }
}