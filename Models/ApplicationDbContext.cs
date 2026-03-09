using Microsoft.EntityFrameworkCore;

namespace ProyectoPyme.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Esencia> Esencias { get; set; }
        public DbSet<Carrito> Carrito { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapea las tablas a minúsculas exactas en MySQL
            modelBuilder.Entity<Productos>().ToTable("Productos");
            modelBuilder.Entity<Categoria>().ToTable("categorias");
            modelBuilder.Entity<Rol>().ToTable("roles");
            modelBuilder.Entity<Usuario>().ToTable("usuarios");   

            // Opcional: seed inicial para categorías
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { CategoriaId = 1, Nombre = "Diseñador", Descripcion = "Perfume de diseñador" },
                new Categoria { CategoriaId = 2, Nombre = "Dupe", Descripcion = "Perfume réplica" },
                new Categoria { CategoriaId = 3, Nombre = "Árabe", Descripcion = "Perfume árabe" }
            );

            modelBuilder.Entity<Esencia>().ToTable("esencias");

            modelBuilder.Entity<Esencia>().HasData(
                new Esencia { EsenciaId = 1, Nombre = "Eau de Toilette", Activo = true },
                new Esencia { EsenciaId = 2, Nombre = "Eau de Parfum", Activo = true },
                new Esencia { EsenciaId = 3, Nombre = "Parfum", Activo = true },
                new Esencia { EsenciaId = 4, Nombre = "Elixir", Activo = true }
            );



        }


    }
}
