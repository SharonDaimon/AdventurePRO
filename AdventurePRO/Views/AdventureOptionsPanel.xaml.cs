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
using AdventurePRO.Model.Logics;
using AdventurePRO.Model.APIs.Options;
using AdventurePRO.Model.APIs.Results;

namespace AdventurePRO.Views
{
    /// <summary>
    /// Логика взаимодействия для AdventureOptionsPanel.xaml
    /// </summary>
    public partial class AdventureOptionsPanel : UserControl
    {
        public AdventureOptionsPanel()
        {
            InitializeComponent();
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            var a = (e.OriginalSource as Button).DataContext as AdventureOptions;

            a.AddPerson(new Model.Person());
        }

        private void RemovePerson_Click(object sender, RoutedEventArgs e)
        {
            var a = this.DataContext as AdventureOptions;

            var p = (e.OriginalSource as Button).DataContext as Model.Person;

            a.RemovePerson(p);
        }

        private void Hotel_Checked(object sender, RoutedEventArgs e)
        {
            var hotels = (this.DataContext as AdventureOptions).Hotel;

            var selected = (e.OriginalSource as CheckBox).DataContext as Model.Hotel;

            (this.DataContext as AdventureOptions).Hotel = selected;
        }

        private void Attraction_Checked(object sender, RoutedEventArgs e)
        {
            var attractions = (this.DataContext as AdventureOptions).Attractions;

            var selected = (e.OriginalSource as CheckBox).DataContext as Model.Attraction;

            if (attractions == null)
            {
                attractions = new Model.Attraction[0] { };
            }

            (this.DataContext as AdventureOptions).Attractions = attractions
                .Concat(new Model.Attraction[1] { selected }).ToArray();
        }


    }
}
