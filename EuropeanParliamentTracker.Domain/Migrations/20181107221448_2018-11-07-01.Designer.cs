﻿// <auto-generated />
using System;
using EuropeanParliamentTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EuropeanParliamentTracker.Domain.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20181107221448_2018-11-07-01")]
    partial class _2018110701
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EuropeanParliamentTracker.Domain.Entities.PoliticalGroup", b =>
                {
                    b.Property<Guid>("PoliticalGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("PoliticalGroupId");

                    b.ToTable("PoliticalGroups");
                });

            modelBuilder.Entity("EuropeanParliamentTracker.Domain.Entities.Vote", b =>
                {
                    b.Property<Guid>("VoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Name");

                    b.HasKey("VoteId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("EuropeanParliamentTracker.Domain.Entities.VoteResult", b =>
                {
                    b.Property<Guid>("VoteResultId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ParliamentarianId");

                    b.Property<Guid>("VoteId");

                    b.Property<int>("VoteType");

                    b.HasKey("VoteResultId");

                    b.HasIndex("ParliamentarianId");

                    b.HasIndex("VoteId");

                    b.ToTable("VoteResults");
                });

            modelBuilder.Entity("EuropeanParliamentTracker.Domain.Models.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("CountryId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("EuropeanParliamentTracker.Domain.Models.NationalParty", b =>
                {
                    b.Property<Guid>("NationalPartyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("NationalPartyId");

                    b.ToTable("NationalParties");
                });

            modelBuilder.Entity("EuropeanParliamentTracker.Domain.Models.Parliamentarian", b =>
                {
                    b.Property<Guid>("ParliamentarianId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CountryId");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<Guid>("NationalPartyId");

                    b.Property<string>("OfficalId");

                    b.HasKey("ParliamentarianId");

                    b.HasIndex("CountryId");

                    b.HasIndex("NationalPartyId");

                    b.ToTable("Parliamentarians");
                });

            modelBuilder.Entity("EuropeanParliamentTracker.Domain.Entities.VoteResult", b =>
                {
                    b.HasOne("EuropeanParliamentTracker.Domain.Models.Parliamentarian", "Parliamentarian")
                        .WithMany()
                        .HasForeignKey("ParliamentarianId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EuropeanParliamentTracker.Domain.Entities.Vote", "Vote")
                        .WithMany()
                        .HasForeignKey("VoteId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EuropeanParliamentTracker.Domain.Models.Parliamentarian", b =>
                {
                    b.HasOne("EuropeanParliamentTracker.Domain.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EuropeanParliamentTracker.Domain.Models.NationalParty", "NationlParty")
                        .WithMany()
                        .HasForeignKey("NationalPartyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
