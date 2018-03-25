﻿// <auto-generated />
using ElectoralСalculator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ElectoralСalculator.Data.Migrations
{
    [DbContext(typeof(ElectoralСalculatorContext))]
    partial class ElectoralСalculatorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("ElectoralСalculator.Data.Models.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Party")
                        .IsRequired();

                    b.Property<bool>("ValidVote");

                    b.Property<int>("VoterId");

                    b.Property<bool>("VotingRights");

                    b.HasKey("Id");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("ElectoralСalculator.Data.Models.Voter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("Forename")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Pesel")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Voters");
                });
#pragma warning restore 612, 618
        }
    }
}
