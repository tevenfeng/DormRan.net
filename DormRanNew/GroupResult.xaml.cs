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
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;

namespace DormRanNew
{
    /// <summary>
    /// GroupResult.xaml 的交互逻辑
    /// </summary>
    public partial class GroupResult : MetroWindow
    {
        private ObservableCollection<officer> checkedOfficers;
        private ObservableCollection<officer> GroupResultsWomen;
        private ObservableCollection<officer> GroupResultsMen;
        public static ObservableCollection<officer> GroupResultsOne;
        public static ObservableCollection<officer> GroupResultsTwo;
        public GroupResult()
        {
            InitializeComponent();
        }

        public GroupResult(ObservableCollection<officer> checkedOfficer)
        {
            InitializeComponent();
            this.checkedOfficers = checkedOfficer;
            this.GroupResultsWomen= new ObservableCollection<officer>();
            this.GroupResultsMen = new ObservableCollection<officer>();
            GroupResultsOne = new ObservableCollection<officer>();
            GroupResultsTwo = new ObservableCollection<officer>();
            checkedOfficer_Divided();
        }

        private void checkedOfficer_Divided()
        {
            Random r = new Random();
            int numberOfPerson = this.checkedOfficers.Count();
            int numberOfWomen = 0;
            foreach (var tmpOfficer in checkedOfficers)
            {
                if(tmpOfficer.officer_gender.Equals("女"))
                {
                    numberOfWomen++;
                    this.GroupResultsWomen.Add(tmpOfficer);
                }
                else
                {
                    this.GroupResultsMen.Add(tmpOfficer);
                }
            }
            if (numberOfWomen <= (int)(numberOfPerson / 2))
            {
                GroupResultsOne = this.GroupResultsWomen;
                this.dataGridGroupOne.ItemsSource = null;
                this.dataGridGroupOne.ItemsSource = GroupResultsOne;

                GroupResultsTwo = this.GroupResultsMen;
                this.dataGridGroupTwo.ItemsSource = null;
                this.dataGridGroupTwo.ItemsSource = GroupResultsTwo;
            }
            else
            {
                int firstGroupNumber = (int)(numberOfPerson / 2);
                HashSet<int> hash1 = new HashSet<int>();
                while(firstGroupNumber!=0)
                {
                    int index = r.Next(0, numberOfWomen);
                    if (!hash1.Contains(index))
                    {
                        GroupResultsOne.Add(GroupResultsWomen[index]);
                        hash1.Add(index);
                        firstGroupNumber--;
                    }
                }
                this.dataGridGroupOne.ItemsSource = null;
                this.dataGridGroupOne.ItemsSource = GroupResultsOne;

                for(int i=0;i< numberOfWomen; i++)
                {
                    if(!hash1.Contains(i))
                    {
                        GroupResultsTwo.Add(GroupResultsWomen[i]);
                    }
                }

                foreach(var tmp in GroupResultsMen)
                {
                    GroupResultsTwo.Add(tmp);
                }

                this.dataGridGroupTwo.ItemsSource = null;
                this.dataGridGroupTwo.ItemsSource = GroupResultsTwo;
            }
        }
    }
}
