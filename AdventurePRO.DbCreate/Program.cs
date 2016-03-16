using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePRO.Model;

namespace AdventurePRO.DbCreate
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new AdventureContext();

            var adv = new Adventure
            {
                Weather = new Weather[] { new Weather { Date = DateTime.Now, Temperature = 0u, Unit = "°C" } },
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddDays(5)
            };

            context.Adventures.Add(adv);
            context.SaveChanges();
        }
    }
}
