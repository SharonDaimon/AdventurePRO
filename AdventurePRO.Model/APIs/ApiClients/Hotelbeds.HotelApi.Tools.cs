using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using AdventurePRO.Model.APIs.Results;

namespace AdventurePRO.Model.APIs.ApiClients
{
    public partial class Hotelbeds
    {
        // XML response arrays names
        private static readonly XName HOTELS_AR = xmlns + "hotels";
        private static readonly XName ROOMS_AR = xmlns + "rooms";
        private static readonly XName RATES_AR = xmlns + "rates";

        // XML response elements names
        private static readonly XName AVAILABILITY_RS_EL = xmlns + "availabilityRS";
        private static readonly XName HOTEL_EL = xmlns + "hotel";
        private static readonly XName ROOM_EL = xmlns + "room";
        private static readonly XName RATE_EL = xmlns + "rate";

        // XML response atributes names
        private static readonly XName HOTEL_NAME_ATTR = "name";
        private static readonly XName HOTEL_CODE_ATTR = "code";
        private static readonly XName ROOM_NAME_ATTR = "name";
        private static readonly XName ROOM_CODE_ATTR = "code";
        private static readonly XName CURRENCY_ATTR = "currency";
        private static readonly XName ROOMS_COUNT_ATTR = "rooms";
        private static readonly XName ADULTS_NUMBER_ATTR = "adults";
        private static readonly XName CHILDREN_NUMBER_ATTR = "children";
        private static readonly XName NET_ATTR = "net";
        private static readonly XName RATE_KEY_ATTR = "rateKey";

        private async Task<IEnumerable<HotelRoom>> sendRequsetAndParseRespnonse_HotelApi
            (
                XDocument request_xml, IEnumerable<Hotel> hotels
            )
        {
            byte[] request_data = null;

            using (var stream = new MemoryStream())
            {
                request_xml.Save(stream);
                request_data = stream.ToArray();
            }

            byte[] response = await PostAsync(HOTEL_API, API_VERSION, HOTEL_API_HOTELS_METHOD, null, request_data);

            return parseResponse_HotelAPI(response, hotels);
        }

        private static IEnumerable<HotelRoom> parseResponse_HotelAPI(byte[] data, IEnumerable<Hotel> hotels)
        {
            XDocument x_response;

            using (var stream = new MemoryStream(data))
            {
                x_response = XDocument.Load(stream);
            }

            return parseResponse_HotelAPI(x_response, hotels);
        }

        private static IEnumerable<HotelRoom> parseResponse_HotelAPI(XContainer xroot, IEnumerable<Hotel> hotels)
        {
            return from x_hotel in elements(element(element(xroot, AVAILABILITY_RS_EL), HOTELS_AR), HOTEL_EL)
                   from room in parseHotel_HotelAPI(x_hotel, hotels)
                   select room;
        }

        private static IEnumerable<HotelRoom> parseHotel_HotelAPI(XElement x_hotel, IEnumerable<Hotel> hotels)
        {
            if (x_hotel == null)
            {
                return null;
            }

            if (hotels == null)
            {
                hotels = new Hotel[0];
            }

            string code = attribute(x_hotel, HOTEL_CODE_ATTR);
            var hotel = hotels.FirstOrDefault(h => h.Code == code);

            string currency = attribute(x_hotel, CURRENCY_ATTR);

            return from x_rooms in elements(element(x_hotel, ROOMS_AR), ROOM_EL)
                   from room in parseRoom(x_rooms, hotel, currency)
                   select room;
        }

        private static IEnumerable<HotelRoom> parseRoom(XElement x_room, Hotel hotel, string currency)
        {
            if (x_room == null)
            {
                return null;
            }

            string name = attribute(x_room, ROOM_NAME_ATTR);
            string code = attribute(x_room, ROOM_CODE_ATTR);

            return from x_rate in elements(element(x_room, RATES_AR), RATE_EL)
                   select parseRate(x_rate, hotel, name, code, currency);
        }

        private static HotelRoom parseRate(XElement x_rate, Hotel hotel, string room_name, string room_code, string currency)
        {
            if (x_rate == null)
            {
                return null;
            }

            return new HotelRoom
            {
                Hotel = hotel,
                Name = room_name,
                Key = attribute(x_rate, RATE_KEY_ATTR),
                Code = room_code,
                AdultsNumber = uint_attribute(x_rate, ADULTS_NUMBER_ATTR),
                ChildrenNumber = uint_attribute(x_rate, CHILDREN_NUMBER_ATTR),
                RoomsCount = uint_attribute(x_rate, ROOMS_COUNT_ATTR),
                Currency = currency,
                Cost = float_attribute(x_rate, NET_ATTR)
            };
        }

        private static uint uint_attribute(XElement el, XName attr_name)
        {
            return parse_uint(attribute(el, attr_name));
        }

        private static float float_attribute(XElement el, XName attr_name)
        {
            return parseFloat(attribute(el, attr_name));
        }

        private static IEnumerable<XElement> elements(XContainer el, XName name)
        {
            if (el == null)
            {
                return null;
            }

            return el.Elements(name);
        }

        private static XElement element(XContainer el, XName name)
        {
            if (el == null)
            {
                return null;
            }

            return el.Element(name);
        }

        private static string attribute(XElement el, XName attr_name)
        {
            if (el == null)
            {
                return null;
            }
            return attribute_val(el.Attribute(attr_name));
        }

        private static string attribute_val(XAttribute attr)
        {
            if (attr != null)
            {
                return attr.Value;
            }
            return default(string);
        }

        private static uint parse_uint(string v)
        {
            uint u;

            if (uint.TryParse(v, out u))
            {
                return u;
            }
            else
            {
                return default(uint);
            }
        }

        private static float parseFloat(string v)
        {
            float f;
            if (float.TryParse(v, out f))
            {
                return f;
            }
            else
            {
                return default(float);
            }
        }
    }
}
