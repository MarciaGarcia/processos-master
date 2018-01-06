using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Caelum.Senado.Database;
using Caelum.Senado.Modelo;

namespace Caelum.Senado.Database.Migrations
{
    [DbContext(typeof(ProcessoContext))]
    partial class ProcessoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Caelum.Senado.Modelo.Interessado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("Interessados");
                });

            modelBuilder.Entity("Caelum.Senado.Modelo.Interesse", b =>
                {
                    b.Property<int>("InteressadoId");

                    b.Property<int>("ProcessoId");

                    b.HasKey("InteressadoId", "ProcessoId");

                    b.HasIndex("ProcessoId");

                    b.ToTable("Interesse");
                });

            modelBuilder.Entity("Caelum.Senado.Modelo.Processo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Ano");

                    b.Property<int>("Codigo");

                    b.Property<DateTime?>("DataUltimaAtualizacao");

                    b.Property<string>("Emenda");

                    b.Property<int>("Numero");

                    b.Property<int>("Origem");

                    b.Property<string>("Sigla")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Processos");
                });

            modelBuilder.Entity("Caelum.Senado.Modelo.Interesse", b =>
                {
                    b.HasOne("Caelum.Senado.Modelo.Interessado", "Interessado")
                        .WithMany("Interesses")
                        .HasForeignKey("InteressadoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Caelum.Senado.Modelo.Processo", "Processo")
                        .WithMany("Interesses")
                        .HasForeignKey("ProcessoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
