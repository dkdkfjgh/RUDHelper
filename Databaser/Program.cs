using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace Databaser
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLModule SQLClass = new SQLModule("Data.db");
        }
    }
    class SQLModule
    {
        private SQLiteConnection Conn = null;
        private string Query = null;
        private SQLiteDataReader rdr;
        private SQLiteCommand cmd = null;
        private List<string[]> dataList = new List<string[]>();

        public SQLModule(string FileName, bool mode = false)
        {
            if (mode == false)
            {
                Conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;mode=ro;", FileName)); //As read-only
            }
            else
            {
                Conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", FileName));
            }
            Conn.Open();
        }
        public List<string[]> GetQueryResult()
        {
            while (rdr.Read())
            {
                string[] tempRow = new string[rdr.FieldCount];
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    try
                    {
                        tempRow[i] = (string)rdr[i];
                    }
                    catch (Exception e)
                    {
                        return dataList;
                    }
                }
                dataList.Add(tempRow);
            }

            return dataList;

        }
        public void MakeResultEmpty()
        {
            dataList.Clear();
        }
        public void ExecuteSQL(string SQL)
        {
            try
            {
                Query = SQL;
                cmd = new SQLiteCommand(Query, Conn);
                rdr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {

            }
        }
    }
}
