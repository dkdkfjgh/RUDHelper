using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
namespace RUDHelper
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        static int cnt = 0;
        static int cnt2 = 0;
        SQLModule sqlClass = new SQLModule("Data.db");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBoxChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        private void Search()
        {
            if(cnt == 0)
            {
                cnt++;
                return;
            }

            string Input = TextBox.Text;
            sqlClass.ExecuteSQL(string.Format("select * from Data where Name like \"%{0}%\";", Input));
            var StrList = sqlClass.GetQueryResult();
            if (StrList.Count != 0)
            {
                Name.Text = StrList[0][0];
                RankLabel.Text = StrList[0][1];
                TipLabel.Text = StrList[0][2];
            }
            sqlClass.MakeResultEmpty();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Input = TextBox.Text;
            sqlClass.ExecuteSQL(string.Format("select * from Data where Name like \"%{0}%\";", Input));
            var StrList = sqlClass.GetQueryResult();
            if (StrList.Count > 1)
            {
                if (cnt2 < StrList.Count-1)
                {
                    cnt2++;
                }
                else
                {
                    cnt2 = 0;
                }
                Name.Text = StrList[cnt2][0];
                RankLabel.Text = StrList[cnt2][1];
                TipLabel.Text = StrList[cnt2][2];
            }
            sqlClass.MakeResultEmpty();

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
                    catch (Exception)
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
                MessageBox.Show(e.Message);
            }
        }
    }
}
