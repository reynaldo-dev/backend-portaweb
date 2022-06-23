using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PARCIAL_3_DPWA.Models
{
    public partial class railwayContext : DbContext
    {
        public railwayContext()
        {
        }

        public railwayContext(DbContextOptions<railwayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Certificacion> Certificacions { get; set; } = null!;
        public virtual DbSet<CertificacionByUsuario> CertificacionByUsuarios { get; set; } = null!;
        public virtual DbSet<ExperienciaByUsuario> ExperienciaByUsuarios { get; set; } = null!;
        public virtual DbSet<GradoAcademicoByUsuario> GradoAcademicoByUsuarios { get; set; } = null!;
        public virtual DbSet<Red> Reds { get; set; } = null!;
        public virtual DbSet<RedByUser> RedByUsers { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=containers-us-west-58.railway.app:7224;Database=railway;Username=postgres;Password=8xjhArQOUYh8lDI0Ru7O");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("timescaledb");

            modelBuilder.Entity<Certificacion>(entity =>
            {
                entity.HasKey(e => e.Id_certificacion)
                    .HasName("certificacion_pkey");

                entity.ToTable("certificacion");

                entity.Property(e => e.Id_certificacion).HasColumnName("id_certificacion");

                entity.Property(e => e.Descripcion).HasColumnName("descripcion");

                entity.Property(e => e.Institucion).HasColumnName("institucion");

                entity.Property(e => e.Link).HasColumnName("_link");

                entity.Property(e => e.Nombre).HasColumnName("nombre");

                entity.Property(e => e.Obtivos).HasColumnName("obtivos");
            });

            modelBuilder.Entity<CertificacionByUsuario>(entity =>
            {
                entity.HasKey(e => e.Id_certificacion_by_Usuario)
                    .HasName("certificacion_by_usuario_pkey");

                entity.ToTable("certificacion_by_usuario");

                entity.Property(e => e.Id_certificacion_by_Usuario).HasColumnName("id_certificacion_by_usuario");

                entity.Property(e => e.Id_certificacion).HasColumnName("id_certificacion");

                entity.Property(e => e.Id_usuario).HasColumnName("id_usuario");

            });

            modelBuilder.Entity<ExperienciaByUsuario>(entity =>
            {
                entity.HasKey(e => e.IdExperienciaByUsuario)
                    .HasName("experiencia_by_usuario_pkey");

                entity.ToTable("experiencia_by_usuario");

                entity.Property(e => e.IdExperienciaByUsuario).HasColumnName("id_experiencia_by_usuario");

                entity.Property(e => e.Id_usuario).HasColumnName("id_usuario");

                entity.Property(e => e.Nombre_proyecto).HasColumnName("nombre_proyecto");

                entity.Property(e => e.Responsabilidades).HasColumnName("responsabilidades");

                entity.Property(e => e.Resumen).HasColumnName("resumen");

                entity.Property(e => e.Rol).HasColumnName("rol");

                entity.Property(e => e.Tecnologias).HasColumnName("tecnologias");

            });

            modelBuilder.Entity<GradoAcademicoByUsuario>(entity =>
            {
                entity.HasKey(e => e.Id_grado_academico_by_usuario)
                    .HasName("grado_academico_by_usuario_pkey");

                entity.ToTable("grado_academico_by_usuario");

                entity.Property(e => e.Id_grado_academico_by_usuario).HasColumnName("id_grado_academico_by_usuario");

                entity.Property(e => e.Id_usuario).HasColumnName("id_usuario");

                entity.Property(e => e.Objetivos).HasColumnName("objetivos");

                entity.Property(e => e.Profesion).HasColumnName("profesion");

                entity.Property(e => e.Universidad).HasColumnName("universidad");

            });

            modelBuilder.Entity<Red>(entity =>
            {
                entity.HasKey(e => e.Id_red)
                    .HasName("red_pkey");

                entity.ToTable("red");

                entity.Property(e => e.Id_red).HasColumnName("id_red");

                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });

            modelBuilder.Entity<RedByUser>(entity =>
            {
                entity.HasKey(e => e.Id_red_by_user)
                    .HasName("red_by_user_pkey");

                entity.ToTable("red_by_user");

                entity.Property(e => e.Id_red_by_user).HasColumnName("id_red_by_user");

                entity.Property(e => e.Accesslink).HasColumnName("accesslink");

                entity.Property(e => e.Id_red).HasColumnName("id_red");

                entity.Property(e => e.Id_usuario).HasColumnName("id_usuario");


            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id_usuario)
                    .HasName("usuario_pkey");

                entity.ToTable("usuario");

                entity.HasIndex(e => e.Correo, "usuario_correo_key")
                    .IsUnique();

                entity.HasIndex(e => e.U_name, "usuario_u_name_key")
                    .IsUnique();

                entity.Property(e => e.Id_usuario).HasColumnName("id_usuario");

                entity.Property(e => e.Apellidos).HasColumnName("apellidos");

                entity.Property(e => e.Correo).HasColumnName("correo");

                entity.Property(e => e.Intro).HasColumnName("intro");

                entity.Property(e => e.Nombres).HasColumnName("nombres");

                entity.Property(e => e.U_name)
                    .HasMaxLength(100)
                    .HasColumnName("u_name");

                entity.Property(e => e.UrlFoto).HasColumnName("urlfoto");
            });

            modelBuilder.HasSequence("chunk_constraint_name", "_timescaledb_catalog");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
