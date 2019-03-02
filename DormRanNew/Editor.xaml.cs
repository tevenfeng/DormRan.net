using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public enum Area { 一区, 二区, 三区, 四区 };

    public class Dorm
    {

    }

    /// <summary>
    /// Editor.xaml 的交互逻辑
    /// </summary>
    public partial class Editor : MetroWindow
    {
        private bool isOfficerEditor;

        private check_dorm_newEntities db;

        public Editor(bool isManagingOfficer, check_dorm_newEntities db)
        {
            this.isOfficerEditor = isManagingOfficer;
            this.db = db;

            InitializeComponent();
        }

        private void editorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.isOfficerEditor)
            {
                this.Title = "人员管理";
                this.officeGrid.Visibility = Visibility.Visible;
                ObservableCollection<officer> officers = new ObservableCollection<officer>(this.db.officer.ToList());
                this.officeGrid.DataContext = officers;
            }
            else
            {
                this.Title = "楼栋管理";
                this.dormGrid.Visibility = Visibility.Visible;
                ObservableCollection<dorm> dorms = new ObservableCollection<dorm>(this.db.dorm.ToList());
                this.dormGrid.DataContext = dorms;
            }
        }

        private void btnAddNewRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemoveRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveRecords_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
