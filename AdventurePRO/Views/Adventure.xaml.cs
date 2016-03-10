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
using System.ComponentModel;

namespace AdventurePRO.Views
{
    /// <summary>
    /// Логика взаимодействия для Adventure.xaml
    /// </summary>
    public partial class Adventure : UserControl, INotifyPropertyChanged
    {
        private bool open = false;

        public Adventure()
        {
            InitializeComponent();
        }

        public bool Open
        {
            get { return open; }
            set
            {
                open = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Open"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
