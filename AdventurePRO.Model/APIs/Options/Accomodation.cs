// Author: Kristina Enikeeva
// Date: 10.03.2016
// This file contains hotels search options class Accomodation

namespace AdventurePRO.Model.APIs.Options
{
    /// <summary>
    /// Describes an accomodation in single hotel room
    /// </summary>
    public class Accomodation
    {
        /// <summary>
        /// Rooms count in hotel room by default
        /// </summary>
        public const uint ROOMS_COUNT_DEFAULT = 1;

        private uint? rooms_count;

        /// <summary>
        /// Rooms count in hotel room
        /// </summary>
        public uint? RoomsCount
        {
            get { return rooms_count ?? ROOMS_COUNT_DEFAULT; }
            set { rooms_count = value; }
        }

        /// <summary>
        /// Room guests
        /// </summary>
        public Person[] Guests { get; set; }
    }
}
