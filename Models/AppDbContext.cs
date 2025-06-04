using Microsoft.EntityFrameworkCore;


namespace WebCriptomonedas.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Transaction> Transactions => Set<Transaction>();


        //sirve para que las migraciones funcionen correctamente y se creen las tablas en la base de datos
        //solucionamos el problema de la precision de los decimales
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Siempre llamá al base primero

            // Configurar precisión para los decimales
            modelBuilder.Entity<Transaction>()
                .Property(t => t.CryptoAmount)
                .HasPrecision(18, 8); // Ideal para criptomonedas

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Money)
                .HasPrecision(18, 2); // Ideal para dinero tradicional
        }

    }
}
