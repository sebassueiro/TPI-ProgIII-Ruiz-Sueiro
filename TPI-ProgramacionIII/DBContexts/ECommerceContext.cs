using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using TPI_ProgramacionIII.Controllers;
using TPI_ProgramacionIII.Data.Entities;

namespace TPI_ProgramacionIII.DBContexts
{
    public class ECommerceContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineOfOrder> LinesOfOrder { get; set; }
        public DbSet<Product> Products { get; set; }

        //Este es el constructor
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasDiscriminator(u => u.UserType);

            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    LastName = "Roncoroni",
                    Name = "Tomas",
                    Email = "Troncoroni@gmail.com",
                    UserName = "tomiR",
                    Password = "123456",
                    Address = "San Martin 5550",
                    Id = 1
                },

                new Client
                {
                    LastName = "Diaz",
                    Name = "Nahuel",
                    Email = "NDiaz@gmail.com",
                    UserName = "diasnahuel",
                    Password = "123456",
                    Address = "Italia 2500",
                    Id = 3
                });

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    LastName = "Sueiro",
                    Name = "Sebastian",
                    Email = "seba@gmail.com",
                    UserName = "sebas",
                    Password = "123456",
                    Id = 4,
                    Role = "admin"
                });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 6,
                    Name = "Remera Roja",
                    Price = 12500,
                    Stock = 10,

                },
                new Product
                {
                    Id = 7,
                    Name = "Remera Negra",
                    Price = 13200,
                    Stock = 20,
                });

            //Relación entre Cliente y OrdenDeVenta(uno a muchos)
            modelBuilder.Entity<Client>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Client)
            .HasForeignKey(o => o.ClientId);

            // Relación entre OrdenDeVenta y LineaDeVenta (uno a muchos)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.LinesOfOrder)
                .WithOne(l => l.SaleOrder)
                .HasForeignKey(l => l.SaleOrderId);


            modelBuilder.Entity<LineOfOrder>()
                .HasOne(sol => sol.Product)
                .WithMany() //vacío porque no me interesa establecer esa relación
                .HasForeignKey(sol => sol.ProductId);



            base.OnModelCreating(modelBuilder);
        }
    }
}
