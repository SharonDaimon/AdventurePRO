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
using AdventurePRO.Model.Logics;

namespace AdventurePRO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AdventureOptions options;
        private AdventureApiContext context;

        public MainWindow()
        {
            InitializeComponent();

            // Some logic to show results
            initExchangeConverter();

            options = new AdventureOptions();

            Options.DataContext = options;
        }

        private void initExchangeConverter()
        {
            StaticCurrencyConverter.Converter = new converter();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if(context == null)
            {
                context = new AdventureApiContext { Options = options };
            }

            MainContent.Navigate(new AdventureResultsPage(context.AdventureResults));
        }

        private class converter : ICurencyConverter
        {
            private const string EUR = "EUR";

            Dictionary<string, float> rates;

            public converter()
            {
                init();
            }

            private async void init()
            {
                rates = await new Model.APIs.ApiClients.Fixer().GetRatesAsync();
            }

            public float this[string from, string to]
            {
                get
                {
                    if(rates != null
                        && (rates.ContainsKey(from) || from == EUR) 
                        && rates.ContainsKey(to) || to == EUR)
                    {
                        return (from == EUR ? 1 : rates[from]) / (to == EUR ? 1 : rates[to]);
                    }
                    throw new NotSupportedException("Don't have given rate(s)");
                }
            }

            public float Convert(float cost, string from, string to)
            {
                return cost * this[from, to];
            }
        }
    }
}
