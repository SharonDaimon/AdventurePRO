// Author: Kristina Enikeeva
// Дата: 08.03.2016
// This file contains hotelbeds locations logic

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AdventurePRO.Model.APIs.Hotelbeds
{
    public partial class ApiClient
    {
        /// <summary>
        /// Asks hotel-comtent-api for countries list
        /// </summary>
        /// <param name="country_codes">
        /// The country codes to request for. 
        /// Set null to get all available countries
        /// </param>
        /// <returns>The list of available countries codes</returns>
        public async Task<Country[]> GetCountriesAsync(params string[] country_codes)
        {
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("fields", "all");
            parameters.Add("language", LANG);

            if (country_codes != null && country_codes.Count() > 0)
            {
                parameters.Add("codes", string.Join(",", country_codes));
            }

            var x_countries = await WithFromTo("locations/countries", parameters, "countriesrs", "countries", "country");

            var countries = from xc in x_countries
                            select new Country() { Name = xc.Element("description").Value, Code = xc.Attribute("code").Value };

            return countries.ToArray();
        }

        /// <summary>
        /// Asks hotel-comtent-api for destinations list
        /// </summary>
        /// <param name="countries">
        /// The list of countries for the list of destinations is returned. 
        /// If it is null, method should return the list of destinations for or of available countries,
        /// but Country property in each of returned destination should be null
        /// </param>
        /// <returns>The list of available destinations</returns>
        public async Task<Destination[]> GetDestinationAsync(IEnumerable<Country> countries)
        {
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("fields", "code,countryCode,name");
            parameters.Add("language", LANG);

            if (countries != null && countries.Count() > 0)
            {
                parameters.Add("countryCodes", string.Join(",", countries.Select(c => c.Code)));
            }

            var x_destinations = await WithFromTo("locations/destinations", parameters, "destinationsrs", "destinations", "destination");

            //  To prevent NullReferenceException in linq to sql below
            if(countries == null)
            {
                countries = new Country[0];
            }

            var destinations = from xc in x_destinations
                            select new Destination()
                            {
                                Name = xc.Element("name").Value,
                                Code = xc.Attribute("code").Value,
                                Country = countries.FirstOrDefault(c => c.Code == xc.Attribute("countryCode").Value)
                            };

            return destinations.ToArray();
        }
    }
}
