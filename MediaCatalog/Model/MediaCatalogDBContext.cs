using Microsoft.EntityFrameworkCore;
using MediaCatalog.Model.DTO;

namespace MediaCatalog.Model
{
    public class MediaCatalogDBContext: DbContext
    {
        public MediaCatalogDBContext() : base() 
        { 
            Database.EnsureCreated(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=MediaCatalog.db");
        }

        public DbSet<TV_ProgramDTO> TVPrograms { get; set; }
        public DbSet<MediaFileDTO> MediaFiles { get; set; }
    }
}
