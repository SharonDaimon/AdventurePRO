// Author: Kristina Enikeeva
// Date: 10.03.2016
// This file contains hotels search options class Accomodation

using System.ComponentModel;

namespace AdventurePRO.Model.APIs.Options
{
    /// <summary>
    /// Describes an accomodation in single hotel room
    /// </summary>
    public class Accomodation : INotifyPropertyChanged
    {
        /// <summary>
        /// Rooms count in hotel room by default
        /// </summary>
        public const uint ROOMS_COUNT_DEFAULT = 1;

        private uint? rooms_count;

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChange(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Rooms count in hotel room
        /// </summary>
        public uint? RoomsCount
        {
            get { return rooms_count ?? ROOMS_COUNT_DEFAULT; }
            set { rooms_count = value; }
        }

        private Person[] guests;

        /// <summary>
        /// Room guests
        /// </summary>
        public Person[] Guests { get{return guests;}
            set
            {
                guests = value;

                notifyPropertyChange("Guests");
            }
        }
    }
}
