// Author: Anastasia Mukalled
// Date: 13.03.2016
// This file contains Adventure database context

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventurePRO.Model
{
    /// <summary>
    /// The database context
    /// </summary>
    public class AdventureContext : DbContext
    {
        /// <summary>
        /// Adventures
        /// </summary>
        public DbSet<Adventure> Adventures { get; set; }
    }
}
