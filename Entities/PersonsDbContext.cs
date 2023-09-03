using System;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
	public class PersonsDbContext : DbContext
	{
        public PersonsDbContext(DbContextOptions options) : base(options)
        {

        }

		public DbSet<Person> Persons { get; set; }

		public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          //  modelBuilder.Entity<Country>().ToTable("Countries");

           // modelBuilder.Entity<Person>().ToTable

            //seed data to Countries table
            //read json files
            string countriesJson = System.IO.File.ReadAllText("countries.json");


            //deserailzed json data to list of countries
            List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            //read this data in foreach and add in hasdata method to seed data to db table

            foreach (Country item in countries)
            {
                modelBuilder.Entity<Country>().HasData(item);
            }

            //seed data for persons table

            //read json data
             string personsJson = System.IO.File.ReadAllText("persons.json");

            //desearized the json into list of perons
            List<Person>? people= System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person item in people)
            {
                modelBuilder.Entity<Person>().HasData(item);

            }

            
        }
    }
}

