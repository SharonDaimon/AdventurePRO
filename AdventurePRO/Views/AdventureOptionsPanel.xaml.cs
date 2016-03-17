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

        private void AddGuest_Click(object sender, RoutedEventArgs e)
        {
            var a = (e.OriginalSource as Button).DataContext as Accomodation;
            var guests = a.Guests;

            if(guests == null)
            {
                guests = new Model.Person[0] { };
            }

            a.Guests = guests.Concat(new Model.Person[1] { new Model.Person() }).ToArray();
        }

        private void AddAccomodation_Click(object sender, RoutedEventArgs e)
        {
            var accomodations = (this.DataContext as AdventureOptions).Accomodations;

            if(accomodations == null)
            {
                accomodations = new Accomodation[0] { };
            }

            (this.DataContext as AdventureOptions).Accomodations = accomodations
                .Concat(new Accomodation[1] { new Accomodation() }).ToArray();
        }

        private void Hotel_Checked(object sender, RoutedEventArgs e)
        {
            var hotels = (this.DataContext as AdventureOptions).Hotels;

            var selected = (e.OriginalSource as CheckBox).DataContext as Model.Hotel;

            if(hotels == null)
            {
                hotels = new Model.Hotel[0] { };
            }

            (this.DataContext as AdventureOptions).Hotels = hotels
                .Concat(new Model.Hotel[1] { selected }).ToArray();
        }

        private void Attraction_Checked(object sender, RoutedEventArgs e)
        {
            var attractions = (this.DataContext as AdventureOptions).Attractions;

            var selected = (e.OriginalSource as CheckBox).DataContext as Model.Attraction;

            if(attractions == null)
            {
                attractions = new Model.Attraction[0] { };
            }

            (this.DataContext as AdventureOptions).Attractions = attractions
                .Concat(new Model.Attraction[1] { selected }).ToArray();
        }
    }
}
    