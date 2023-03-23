using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiScaffolding01;

namespace ApiScaffolding01.Data
{
    public class PersonContext : DbContext
    {
        public PersonContext (DbContextOptions<PersonContext> options)
            : base(options)
        {
        }

        public DbSet<ApiScaffolding01.Person> Person { get; set; } = default!;
    }
}
