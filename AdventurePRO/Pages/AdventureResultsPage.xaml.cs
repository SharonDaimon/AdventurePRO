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
using System.Windows.Data;

namespace AdventurePRO.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdventureResultsPage.xaml
    /// </summary>
    public partial class AdventureResultsPage : Page
    {
        public AdventureResultsPage(Adventure adventure)
        {
            InitializeComponent();
            this.DataContext= adventure;
        }
    }   
}
