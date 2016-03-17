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
            throw new NotImplementedException();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if(context == null)
            {
                context = new AdventureApiContext { Options = options };
            }

            MainContent.Navigate(new AdventureResultsPage(context.AdventureResults));
        }
    }
}
