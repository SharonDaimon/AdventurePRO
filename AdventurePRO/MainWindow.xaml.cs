// Author: Ekaterina Kuznetsova
// Date: 06.03.2016
// This file contains MainWindow class

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
using AdventurePRO.Model;
using AdventurePRO.Pages;

namespace AdventurePRO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Some logic to show results
            initExchangeConverter();

            MainContent.Navigate(new AdventureResultsPage(createAdventureList()));
        }

        private void initExchangeConverter()
        {
            StaticCurrencyConverter.Converter = new CurrencyConverter();
        }

        private IEnumerable<Adventure> createAdventureList()
        {
            List<Adventure> adventures = new List<Adventure>();

            Person[] persons1 = new Person[]
            {
                new Person() {Name = "Катя", Age = 17, Gender = Gender.Female},
                new Person() {Name = "Лёша", Age = 18, Gender = Gender.Male }
            };

            Occupancy[] oc1H1 = new Occupancy[]
            {
                new Occupancy() { CheckIn = new DateTime(2016, 03, 21), CheckOut = new DateTime(2016, 03, 24), Cost = 12560, Currency="р", Guests = persons1 }
            };

            Hotel[] hotels1 = new Hotel[]
            {
                new Hotel() { Stars = 3, Occupancies = oc1H1 },
            };

            Taxi[] taxis1 = new Taxi[]
            {
                new Taxi() { When = new DateTime(2016, 03, 21), Passengers = persons1, Cost = 490, Currency="р"}
            };

            Ticket[] tickets1 = new Ticket[]
            {
                new Ticket() { Departure = new DateTime(2016, 3, 21, 1, 13, 0), Arrival = new DateTime(2016, 3, 21, 4, 26, 0), Cost = 3026, Currency = "р", Owner = persons1[0] },
                new Ticket() { Departure = new DateTime(2016, 3, 21, 1, 13, 0), Arrival = new DateTime(2016, 3, 21, 4, 26, 0), Cost = 3026, Currency = "р", Owner = persons1[1] }
            };

            AttractionTicket[] attraction_tickets1 = new AttractionTicket[]
            {
                new AttractionTicket() { Cost = 250, Currency = "р", Owner = persons1[0] },
                new AttractionTicket() { Cost = 250, Currency = "р", Owner = persons1[1] }
            };

            Attraction[] attractions1 = new Attraction[]
            {
                new Attraction()
                {
                    Name = "VK super photo",
                    Tickets = attraction_tickets1,
                    Photos = new Uri[]
                {
                    new Uri("http://cs631821.vk.me/v631821420/4325/sdGI0E-IE9Y.jpg"),
                    new Uri("http://38.media.tumblr.com/6dc3291c402a7756104100e6791f7c1c/tumblr_inline_ntkfxwTCXr1t76aks_500.gif"),
                    new Uri("https://media1.giphy.com/media/8z3YZdZ5vAz28/200_s.gif")
                },
                    Description = @"Lorem ipsum dolor sit amet, feugiat moderatius pri at. Etiam nihil propriae sed cu, qui suscipiantur necessitatibus at. Alterum pertinax torquatos in vim, iudico causae id vis. Nam iudico posidonium ex. Et graeco offendit per, nihil efficiantur duo at.
In quo sumo velit mediocritatem, eam eius utroque ne. Vis cibo singulis no, paulo dignissim has ne, eu utroque theophrastus comprehensam mel. Erat porro eu usu. Ne cum duis complectitur. His ex ullum facilisi deterruisset. Solet explicari conceptam cum ut, ea admodum copiosae tincidunt sea, an errem zril salutandi mea.
Vel omnis eruditi no, mel ne viris blandit pertinacia, te eam indoctum volutpat signiferumque. Cu eam possim mediocrem sententiae, nam ut mucius ",
                    Rating = 3.5f,
                    Site="https://vk.com"
                }

            };

            Weather[] weather = new Weather[]
            {
                new Weather() { Date = new DateTime(2016, 3, 21), Temperature = 18, Unit="C" },
                new Weather() { Date = new DateTime(2016, 3, 22), Temperature = 20, Unit="C" },
                new Weather() { Date = new DateTime(2016, 3, 23), Temperature = 21, Unit="C" },
                new Weather() { Date = new DateTime(2016, 3, 24), Temperature = 16, Unit="C" }
            };

            Adventure a1 = new Adventure()
            {
                Currency = "р",
                StartDate = new DateTime(2016, 3, 20),
                FinishDate = new DateTime(2016, 3, 25),
                Persons = persons1,
                Hotels = hotels1,
                Taxis = taxis1,
                Tickets = tickets1,
                Attractions = attractions1,
                Weather = weather
            };



            adventures.Add(a1);

            return adventures;
        }

        private class CurrencyConverter : ICurencyConverter
        {
            public float this[string from, string to]
            {
                get
                {
                    return 1;
                }
            }

            public float Convert(float cost, string from, string to)
            {
                return cost;
            }
        }
    }
}
