using ControlFilteredResults.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ControlFilteredResults.Data
{
    public class FileListContext : DbContext
    {
        public FileListContext()
            : base()
        {
        }

        public FileListContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<FileList> FileList { get; set; }

        public DbSet<File> File { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileList>()
                .Ignore(nameof(Models.FileList.Files));
            modelBuilder.Entity<File>()
                .HasOne<FileList>()
                .WithMany(fileList => fileList.FileNames)
                .HasForeignKey(fileList => fileList.FileListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
