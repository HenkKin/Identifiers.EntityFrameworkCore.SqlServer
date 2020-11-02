﻿// <auto-generated />
using DataAccessClientExample.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identifiers.EntityFrameworkCore.SqlServer.Example.Migrations.LongDatabase
{
    [DbContext(typeof(LongDbContext))]
    [Migration("20201102234705_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0-rc.2.20475.6");

            modelBuilder.Entity("DataAccessClientExample.DataLayer.ExampleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None)
                        .HasAnnotation("Identifier", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExampleEntities");
                });

            modelBuilder.Entity("DataAccessClientExample.DataLayer.ExampleEntityTranslation", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocaleId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TranslatedEntityId")
                        .HasColumnType("int");

                    b.Property<long?>("TranslatedEntityId1")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TranslatedEntityId1");

                    b.ToTable("ExampleEntityTranslation");
                });

            modelBuilder.Entity("DataAccessClientExample.DataLayer.ExampleEntityTranslation", b =>
                {
                    b.HasOne("DataAccessClientExample.DataLayer.ExampleEntity", "TranslatedEntity")
                        .WithMany("Translations")
                        .HasForeignKey("TranslatedEntityId1");

                    b.Navigation("TranslatedEntity");
                });

            modelBuilder.Entity("DataAccessClientExample.DataLayer.ExampleEntity", b =>
                {
                    b.Navigation("Translations");
                });
#pragma warning restore 612, 618
        }
    }
}
