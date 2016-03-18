using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventurePRO.Model.APIs.ApiClients
{
    public partial class Hotelbeds
    {
        private static readonly XName HOTEL_IMAGES_AR = xmlns + "images";

        private static readonly XName HOTEL_IMAGE_EL = xmlns + "image";

        private static readonly XName IMAGE_TYPE_CODE_ATTR = "imageTypeCode";
        private static readonly XName IMAGE_PATH_ATTR = "path";

        private const string RES_IMAGE_TYPE_CODE = "RES";

        private static Uri[] parseHotelImages(XContainer container)
        {
            var x_images = elements(element(container, HOTEL_IMAGES_AR), HOTEL_IMAGE_EL);

            if (x_images == null)
            {
                return null;
            }

            var images = from x_image in x_images
                         where attribute(x_image, IMAGE_TYPE_CODE_ATTR) == RES_IMAGE_TYPE_CODE
                         select parseHotelImage(x_image);

            if(images != null)
            {
                return images.ToArray();
            }

            return null;
        }

        private static Uri parseHotelImage(XElement x_image)
        {
            var path = attribute(x_image, IMAGE_PATH_ATTR);

            if (path != null && path != default(string))
            {
                return new Uri(PATH_TO_IMAGES + path);
            }

            return null;
        }
    }
}

