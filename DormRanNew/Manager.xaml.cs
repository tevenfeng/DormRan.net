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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace DormRanNew
{
    /// <summary>
    /// Manager.xaml 的交互逻辑
    /// </summary>
    public partial class Manager : MetroWindow
    {
        public Manager()
        {
            InitializeComponent();
        }

        private void btnManageOfficer_Click(object sender, RoutedEventArgs e)
        {
            Editor officerEditor = new Editor(Management.人员);
            officerEditor.Owner = this;
            officerEditor.ShowDialog();
        }

        private void btnManageDorm_Click(object sender, RoutedEventArgs e)
        {
            Editor dormEditor = new Editor(Management.楼栋);
            dormEditor.Owner = this;
            dormEditor.ShowDialog();
        }

        private void btnQueryHistory_Click(object sender, RoutedEventArgs e)
        {
            Editor historyEditor = new Editor(Management.历史);
            historyEditor.Owner = this;
            historyEditor.ShowDialog();
        }

        private void btnCheckinHistory_Click(object sender, RoutedEventArgs e)
        {
            Editor checkinEditor = new Editor(Management.签到);
            checkinEditor.Owner = this;
            checkinEditor.ShowDialog();
        }
    }
}
