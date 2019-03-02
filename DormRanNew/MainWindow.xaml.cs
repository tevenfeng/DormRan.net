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
using MahApps.Metro.Controls;

namespace DormRanNew
{
    class Result
    {
        public string dormName { get; set; }
        public string gender { get; set; }
        public string floorsOfDorm { get; set; }

        public Result(string dormName, string gender, string floorsOfDorm)
        {
            this.dormName = dormName;
            this.gender = gender;
            this.floorsOfDorm = floorsOfDorm;
        }
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region 变量

        /// <summary>
        /// 数据库连接
        /// </summary>
        private check_dorm_newEntities db;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.db = new check_dorm_newEntities();
        }

        private void btnStartSampling_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnStartSamplingGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDatabaseManagement_Click(object sender, RoutedEventArgs e)
        {
            Manager managerWindow = new Manager(this.db);
            managerWindow.Owner = this;
            managerWindow.ShowDialog();
        }
    }
}
