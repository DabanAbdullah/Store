﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using Store.Models;


namespace Store.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> catagories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Applicationuser> ApplicationUsers { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }    
        public DbSet<OrderDetail> OrderDetails { get; set; }    
        
        public DbSet<Company> Companies { get; set; }

        public DbSet<Shoppingcart> Shoopingcart { get; set; }


        public DbSet<productimage> ProductImage { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //write this when you have identity
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CatName = "Action", DisplayOrder = 1 },
                new Category { CategoryID = 2, CatName = "Drama", DisplayOrder = 2 }
                );

            modelBuilder.Entity<Company>().HasData(
               new Company
               {
                   Id = 1,
                   Name = "siemens",
                   State = "Bavaria",
                   City = "Munich",
                   StreetAdress = "Maria strasse 1",
                   PostalCode = "90321",
                   Phonenumber = "12333333"
               },
               new Company
               {
                   Id = 2,
                   Name = "T&T",
                   State = "Bavaria",
                   City = "Erlangen",
                   StreetAdress = "Leipzig Strasse 12",
                   PostalCode = "90111",
                   Phonenumber = "123438y333333"
               },
               new Company
               {
                   Id = 3,
                   Name = "AirBNB",
                   State = "Bavaria",
                   City = "Nuremberg",
                   StreetAdress = "Eberdshardhof",
                   PostalCode = "90431",
                   Phonenumber = "123647333333"
               }

               );


            modelBuilder.Entity<Product>().HasData(
                 new Product
                 {
                     Id = 1,
                     Title = "Fortune of Time",
                     Author = "Billy Spark",
                     Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                     ISBN = "SWD9999001",
                     ListPrice = 99,
                     Price = 90,
                     Price50 = 85,
                     CatID = 1,
                    // imageurl = "",

                 },
                 new Product
                 {
                     Id = 2,
                     Title = "Dark Skies",
                     Author = "Nancy Hoover",
                     Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                     ISBN = "CAW777777701",
                     ListPrice = 40,
                     Price = 30,
                     Price50 = 25,
                     CatID = 1,
                  //   imageurl = "",

                 },
                 new Product
                 {
                     Id = 3,
                     Title = "Vanish in the Sunset",
                     Author = "Julian Button",
                     Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                     ISBN = "RITO5555501",
                     ListPrice = 55,
                     Price = 50,
                     Price50 = 40,
                     CatID = 1,
                   //  imageurl = "",

                 },
                 new Product
                 {
                     Id = 4,
                     Title = "Cotton Candy",
                     Author = "Abby Muscles",
                     Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                     ISBN = "WS3333333301",
                     ListPrice = 70,
                     Price = 65,
                     Price50 = 60,
                     CatID = 2,
                   //  imageurl = "",
                 },
                 new Product
                 {
                     Id = 5,
                     Title = "Rock in the Ocean",
                     Author = "Ron Parker",
                     Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                     ISBN = "SOTJ1111111101",
                     ListPrice = 30,
                     Price = 27,
                     Price50 = 25,
                     CatID = 2,
                   //  imageurl = "",

                 },
                 new Product
                 {
                     Id = 6,
                     Title = "Leaves and Wonders",
                     Author = "Laura Phantom",
                     Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                     ISBN = "FOT000000001",
                     ListPrice = 25,
                     Price = 23,
                     Price50 = 22,
                     CatID = 2,
                  //   imageurl = "",

                 });



        }
    }
}
