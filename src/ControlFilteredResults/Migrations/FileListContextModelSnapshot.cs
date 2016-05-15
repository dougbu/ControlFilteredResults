using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ControlFilteredResults.Data;

namespace ControlFilteredResults.Migrations
{
    [DbContext(typeof(FileListContext))]
    partial class FileListContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20801")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ControlFilteredResults.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FileListId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("FileListId");

                    b.ToTable("File");
                });

            modelBuilder.Entity("ControlFilteredResults.Models.FileList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("FileList");
                });

            modelBuilder.Entity("ControlFilteredResults.Models.File", b =>
                {
                    b.HasOne("ControlFilteredResults.Models.FileList")
                        .WithMany()
                        .HasForeignKey("FileListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
