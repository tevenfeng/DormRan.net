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
        private ObservableCollection<officer> GroupResultsWomen;
        private ObservableCollection<officer> GroupResultsMen;
        private ObservableCollection<officer> GroupResultsOne;
        private ObservableCollection<officer> GroupResultsTwo;
        private ObservableCollection<officer> GroupResultsThree;
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
            this.GroupResultsOne = new ObservableCollection<officer>();
            this.GroupResultsTwo = new ObservableCollection<officer>();
            this.GroupResultsThree = new ObservableCollection<officer>();
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
            if (numberOfWomen <= (int)(numberOfPerson / 3))
            {
               // this.GroupResultsOne = this.GroupResultsWomen;
                this.dataGridGroupOne.ItemsSource = null;
                this.dataGridGroupOne.ItemsSource = GroupResultsWomen;

                int numberOfMen = numberOfPerson - numberOfWomen;
                HashSet<int> hash = new HashSet<int>();
                int restNumber1 = (int)(numberOfMen / 2);
                while(restNumber1!=0)
                {
                    int index = r.Next(0, numberOfMen);
                    if(!hash.Contains(index))
                    {
                        GroupResultsTwo.Add(GroupResultsMen[index]);
                        hash.Add(index);
                        restNumber1--;
                    }              
                }
                this.dataGridGroupTwo.ItemsSource = null;
                this.dataGridGroupTwo.ItemsSource = GroupResultsTwo;

                for (int i = 0; i < numberOfMen; i++)
                {
                    if (!hash.Contains(i))
                    {
                        GroupResultsThree.Add(GroupResultsMen[i]);
                    }
                }
                this.dataGridGroupThree.ItemsSource = null;
                this.dataGridGroupThree.ItemsSource = GroupResultsThree;
            }
            else
            {
                int firstGroupNumber = (int)(numberOfPerson / 3);
                int secondGroupNumber = (int)(numberOfPerson / 3 +0.5);
                //int thirdGroupNumber = numberOfPerson - firstGroupNumber - secondGroupNumber;
                int numberOfMen = numberOfPerson - numberOfWomen;
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

                HashSet<int> hash2 = new HashSet<int>();
                int restNumber2 = secondGroupNumber - GroupResultsTwo.Count();
                while (restNumber2!=0)
                {
                    int index= r.Next(0, numberOfMen);
                    if (!hash2.Contains(index))
                    {
                        GroupResultsTwo.Add(GroupResultsMen[index]);
                        hash2.Add(index);
                        restNumber2--;
                    }      
                }
                this.dataGridGroupTwo.ItemsSource = null;
                this.dataGridGroupTwo.ItemsSource = GroupResultsTwo;

                for (int i = 0; i < numberOfMen ; i++)
                {
                    if (!hash2.Contains(i))
                    {
                        GroupResultsThree.Add(GroupResultsMen[i]);
                    }
                }
                this.dataGridGroupThree.ItemsSource = null;
                this.dataGridGroupThree.ItemsSource = GroupResultsThree;
            }
        }
    }
}
