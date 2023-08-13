﻿// <auto-generated />
using System;
using DiscordApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiscordApi.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DiscordApi.LeagueApi.Models.PlayerGameStat", b =>
                {
                    b.Property<string>("MatchId")
                        .HasColumnType("text");

                    b.Property<string>("UserPuuid")
                        .HasColumnType("text");

                    b.HasKey("MatchId", "UserPuuid");

                    b.HasIndex("UserPuuid");

                    b.ToTable("PlayerGameStats");
                });

            modelBuilder.Entity("DiscordApi.LeagueApi.Models.User", b =>
                {
                    b.Property<string>("Puuid")
                        .HasColumnType("text");

                    b.Property<decimal?>("AuthorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("SummName")
                        .HasColumnType("text");

                    b.HasKey("Puuid");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DiscordApi.LeagueApi.Models.PlayerGameStat", b =>
                {
                    b.HasOne("DiscordApi.LeagueApi.Models.User", "User")
                        .WithMany("PlayerGameStats")
                        .HasForeignKey("UserPuuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DiscordApi.LeagueApi.Models.GameStat", "GameStat", b1 =>
                        {
                            b1.Property<string>("PlayerGameStatMatchId")
                                .HasColumnType("text");

                            b1.Property<string>("PlayerGameStatUserPuuid")
                                .HasColumnType("text");

                            b1.Property<string>("gameEndTimestamp")
                                .HasColumnType("text");

                            b1.Property<string>("gameStartTimestamp")
                                .HasColumnType("text");

                            b1.HasKey("PlayerGameStatMatchId", "PlayerGameStatUserPuuid");

                            b1.ToTable("PlayerGameStats");

                            b1.WithOwner()
                                .HasForeignKey("PlayerGameStatMatchId", "PlayerGameStatUserPuuid");
                        });

                    b.OwnsOne("LeagueApi.Models.PlayerStat", "PlayerStat", b1 =>
                        {
                            b1.Property<string>("PlayerGameStatMatchId")
                                .HasColumnType("text");

                            b1.Property<string>("PlayerGameStatUserPuuid")
                                .HasColumnType("text");

                            b1.Property<int?>("Assists")
                                .HasColumnType("integer");

                            b1.Property<string>("ChampionName")
                                .HasColumnType("text");

                            b1.Property<int?>("Deaths")
                                .HasColumnType("integer");

                            b1.Property<int?>("GoldEarned")
                                .HasColumnType("integer");

                            b1.Property<int?>("Kills")
                                .HasColumnType("integer");

                            b1.Property<int?>("NeutralMinionsKilled")
                                .HasColumnType("integer");

                            b1.Property<int?>("ParticipantId")
                                .HasColumnType("integer");

                            b1.Property<int?>("PentaKills")
                                .HasColumnType("integer");

                            b1.Property<int?>("QuadraKills")
                                .HasColumnType("integer");

                            b1.Property<int?>("Spell1Casts")
                                .HasColumnType("integer");

                            b1.Property<int?>("Spell4Casts")
                                .HasColumnType("integer");

                            b1.Property<string>("SummonerName")
                                .HasColumnType("text");

                            b1.Property<int?>("TimeCCingOthers")
                                .HasColumnType("integer");

                            b1.Property<int?>("TotalDamageDealtToChampions")
                                .HasColumnType("integer");

                            b1.Property<int?>("TotalMinionsKilled")
                                .HasColumnType("integer");

                            b1.Property<int?>("TotalTimeSpentDead")
                                .HasColumnType("integer");

                            b1.Property<int?>("VisionScore")
                                .HasColumnType("integer");

                            b1.Property<int?>("WardsPlaced")
                                .HasColumnType("integer");

                            b1.Property<bool>("Win")
                                .HasColumnType("boolean");

                            b1.HasKey("PlayerGameStatMatchId", "PlayerGameStatUserPuuid");

                            b1.ToTable("PlayerGameStats");

                            b1.WithOwner()
                                .HasForeignKey("PlayerGameStatMatchId", "PlayerGameStatUserPuuid");
                        });

                    b.Navigation("GameStat");

                    b.Navigation("PlayerStat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DiscordApi.LeagueApi.Models.User", b =>
                {
                    b.Navigation("PlayerGameStats");
                });
#pragma warning restore 612, 618
        }
    }
}
