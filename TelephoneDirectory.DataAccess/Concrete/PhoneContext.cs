using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Entities.Concrete;
using TelephoneDirectory.Entities.DTOs;

namespace TelephoneDirectory.DataAccess.Concrete
{
    public class PhoneContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TelephoneDirectory;Trusted_Connection=true");
        }



        public DbSet<Person> Persons { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    }
}
