using Microsoft.EntityFrameworkCore;
using Orquesta.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Orquesta.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Contratante> Contratantes { get; set; }
        public DbSet<Agrupacion> Agrupaciones { get; set; }
        public DbSet<Agrupacion_Genero> Agrupacion_Generos { get; set; }
        public DbSet<GeneroMusical> GeneroMusicales { get; set; }
        public DbSet<Integrante> Integrantes { get; set; }
        public DbSet<Representante> Representantes { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }
        public DbSet<Repertorio> Repertorios { get; set; }
        public DbSet<Calificacion_Agrupacion> Calificaciones_Agrupacion { get; set; }
        public DbSet<Calificacion_Contratante> Calificaciones_Contratante { get; set; }
        public DbSet<EstadoReserva> EstadoReservas { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);           
        }

    }
}
