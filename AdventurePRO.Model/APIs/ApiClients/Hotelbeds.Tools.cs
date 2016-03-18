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
                return default(IEnumerable<XElement>);
            }

            return el.Elements(name);
        }

        private static string value(XContainer container, XName element_name)
        {
            if(container == null)
            {
                return default(string);
            }
            return element_value(container.Element(element_name));
        }

        private static string element_value(XElement el)
        {
            if(el == null)
            {
                return default(string);
            }

            return el.Value;
        }

        private static XElement element(XContainer el, XName name)
        {
            if (el == null)
            {
                return default(XElement);
            }

            return el.Element(name);
        }

        private static string attribute(XElement el, XName attr_name)
        {
            if (el == null)
            {
                return default(string);
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
            if(v == null)
            {
                return default(uint);
            }

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
            if(v == null)
            {
                return default(float);
            }

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
