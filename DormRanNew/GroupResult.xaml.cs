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

namespace DormRanNew
{
    /// <summary>
    /// GroupResult.xaml 的交互逻辑
    /// </summary>
    public partial class GroupResult : Window
    {
        private ObservableCollection<officer> checkedOfficers;
        private ObservableCollection<officer> GroupResultsWomen = new ObservableCollection<officer>();
        private ObservableCollection<officer> GroupResultsMen = new ObservableCollection<officer>();
        private ObservableCollection<officer> GroupResultsOne = new ObservableCollection<officer>();
        private ObservableCollection<officer> GroupResultsTwo = new ObservableCollection<officer>();
        private ObservableCollection<officer> GroupResultsThree = new ObservableCollection<officer>();
        public GroupResult()
        {
            InitializeComponent();
        }

        public GroupResult(ObservableCollection<officer> checkedOfficer)
        {
            InitializeComponent();
            this.checkedOfficers = checkedOfficer;
        }

        private void checkedOfficer_Divided(object sender, SelectionChangedEventArgs e)
        {
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
            if (numberOfWomen <= (int)(numberOfPerson / 3))
            {
                this.GroupResultsOne = this.GroupResultsWomen;
                this.dataGridGroupOne.ItemsSource = null;
                this.dataGridGroupOne.ItemsSource = GroupResultsOne;

                int numberOfMen = numberOfPerson - numberOfWomen;
                Random r = new Random();
                int n = r.Next(0, numberOfMen);

                for(int i=0;i<(int)(numberOfMen/2);i++)
                {
                    int index = ((i + 1) * n) % numberOfMen;
                    GroupResultsTwo.Add(GroupResultsMen[index]);
                    GroupResultsMen[index].officer_id = "";
                }
                this.dataGridGroupTwo.ItemsSource = null;
                this.dataGridGroupTwo.ItemsSource = GroupResultsTwo;
            }
            else
            {
                int firstGroupWomen = (int)(numberOfPerson / 3);
                int numberOfMen = numberOfPerson - numberOfWomen;
                Random r = new Random();
                int n = r.Next(0, numberOfWomen);

                for (int i = 0; i < firstGroupWomen; i++)
                {
                    int index = ((i + 1) * n) % numberOfWomen;
                    GroupResultsOne.Add(GroupResultsWomen[index]);
                    GroupResultsWomen[index].officer_id = "";
                }
                this.dataGridGroupOne.ItemsSource = null;
                this.dataGridGroupOne.ItemsSource = GroupResultsOne;

                foreach(var tmp in GroupResultsWomen)
                {
                    if (!tmp.officer_id.Equals(""))
                    {
                        GroupResultsTwo.Add(tmp);
                    }
                }

                int m = r.Next(0, numberOfMen);
                for (int i = 0; i < firstGroupWomen-GroupResultsTwo.Count(); i++)
                {
                    int index = ((i + 1) * m) % numberOfMen;
                    GroupResultsTwo.Add(GroupResultsMen[index]);
                    GroupResultsMen[index].officer_id = "";
                }
                this.dataGridGroupTwo.ItemsSource = null;
                this.dataGridGroupTwo.ItemsSource = GroupResultsTwo;
            }
            foreach (var tmp in GroupResultsMen)
            {
                if (!tmp.officer_id.Equals(""))
                {
                    GroupResultsThree.Add(tmp);
                }
            }
            this.dataGridGroupThree.ItemsSource = null;
            this.dataGridGroupThree.ItemsSource = GroupResultsThree;
        }
    }
}
