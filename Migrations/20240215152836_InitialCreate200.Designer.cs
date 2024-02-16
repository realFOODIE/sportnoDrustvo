﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using sportnoDrustvo.Classes;

#nullable disable

namespace sportnoDrustvo.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240215152836_InitialCreate200")]
    partial class InitialCreate200
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Clan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Priimek")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clani");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Obvestilo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ClanId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Obvescanje")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TerminId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClanId");

                    b.HasIndex("TerminId");

                    b.ToTable("Obvestila");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Rekvizit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ClanId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DatumIzposoje")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("JeNaVoljo")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClanId");

                    b.ToTable("Rekviziti");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Rezervacija", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClanId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatumRezervacije")
                        .HasColumnType("TEXT");

                    b.Property<int>("TerminId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClanId");

                    b.HasIndex("TerminId");

                    b.ToTable("Rezervacije");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Termin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatumTermina")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImeEkipe")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxUdelezencev")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Termini");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Trener", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Specializacija")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Trenerji");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Obvestilo", b =>
                {
                    b.HasOne("sportnoDrustvo.Classes.Models+Clan", null)
                        .WithMany("Obvestila")
                        .HasForeignKey("ClanId");

                    b.HasOne("sportnoDrustvo.Classes.Models+Termin", "Termin")
                        .WithMany()
                        .HasForeignKey("TerminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Termin");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Rekvizit", b =>
                {
                    b.HasOne("sportnoDrustvo.Classes.Models+Clan", "Clan")
                        .WithMany()
                        .HasForeignKey("ClanId");

                    b.Navigation("Clan");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Rezervacija", b =>
                {
                    b.HasOne("sportnoDrustvo.Classes.Models+Clan", "Clan")
                        .WithMany()
                        .HasForeignKey("ClanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("sportnoDrustvo.Classes.Models+Termin", "Termin")
                        .WithMany()
                        .HasForeignKey("TerminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clan");

                    b.Navigation("Termin");
                });

            modelBuilder.Entity("sportnoDrustvo.Classes.Models+Clan", b =>
                {
                    b.Navigation("Obvestila");
                });
#pragma warning restore 612, 618
        }
    }
}
