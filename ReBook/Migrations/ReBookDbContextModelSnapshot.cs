﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReBook.Repository;

#nullable disable

namespace ReBook.Migrations
{
    [DbContext(typeof(ReBookDbContext))]
    partial class ReBookDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.Property<int>("AuthorsId")
                        .HasColumnType("int");

                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.HasKey("AuthorsId", "BooksId");

                    b.HasIndex("BooksId");

                    b.ToTable("AuthorBook");
                });

            modelBuilder.Entity("BookTag", b =>
                {
                    b.Property<int>("BooksId")
                        .HasColumnType("int");

                    b.Property<string>("TagsName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BooksId", "TagsName");

                    b.HasIndex("TagsName");

                    b.ToTable("BookTag");
                });

            modelBuilder.Entity("CopyWish", b =>
                {
                    b.Property<int>("ListCopiesId")
                        .HasColumnType("int");

                    b.Property<int>("WishesId")
                        .HasColumnType("int");

                    b.HasKey("ListCopiesId", "WishesId");

                    b.HasIndex("WishesId");

                    b.ToTable("CopyWish");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "2301D884-221A-4E7D-B509-0113DCC043E1",
                            ConcurrencyStamp = "d85eab84-450f-4ea6-8c5e-ad381977e39f",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3",
                            ConcurrencyStamp = "4787ef8e-4f85-4053-8836-bb22678915c1",
                            Name = "user",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                            RoleId = "2301D884-221A-4E7D-B509-0113DCC043E1"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ReBook.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("NumberOfRatings")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                            AccessFailedCount = 0,
                            Avatar = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624735508/Avatars/anonymous_placeholder_szrplp.jpg",
                            ConcurrencyStamp = "8ee76125-c069-4c7f-800a-7f183684b351",
                            Email = "admin@mvc.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedUserName = "ADMIN@MVC.COM",
                            NumberOfRatings = 0,
                            PasswordHash = "AQAAAAEAACcQAAAAEBTUgPC2PqoCb0mFu9h5m7rxScXHlQ7SsF9MbfFVE//a8V8rcriLkhma/9EiVNH1dQ==",
                            PhoneNumberConfirmed = false,
                            Rating = 0f,
                            SecurityStamp = "f40c213b-ef01-41ec-940e-ebcade3b80a5",
                            TwoFactorEnabled = false,
                            UserName = "admin@mvc.com"
                        });
                });

            modelBuilder.Entity("ReBook.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("ReBook.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("ReBook.Models.Copy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Avalaible")
                        .HasColumnType("bit");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("Condition")
                        .HasColumnType("int");

                    b.Property<int>("EditionId")
                        .HasColumnType("int");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Visuals")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("EditionId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Copies");
                });

            modelBuilder.Entity("ReBook.Models.Edition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("Cover")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Editor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN13")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Language")
                        .HasColumnType("int");

                    b.Property<int?>("pageCount")
                        .HasColumnType("int");

                    b.Property<string>("publishedDate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Editions");
                });

            modelBuilder.Entity("ReBook.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FromCopyId")
                        .HasColumnType("int");

                    b.Property<string>("FromCopyTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromCopyVisual")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FromStatus")
                        .HasColumnType("int");

                    b.Property<string>("FromUserAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromUserLogin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("ToCopyId")
                        .HasColumnType("int");

                    b.Property<string>("ToCopyTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToCopyVisual")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ToStatus")
                        .HasColumnType("int");

                    b.Property<string>("ToUserAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToUserLogin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("UserRatings")
                        .HasColumnType("real");

                    b.Property<bool>("UserReceived")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ReBook.Models.Pretender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CopyId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CopyId");

                    b.ToTable("Pretenders");
                });

            modelBuilder.Entity("ReBook.Models.Tag", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ReBook.Models.Trade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FromCopyId")
                        .HasColumnType("int");

                    b.Property<string>("FromCopyTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromCopyVisual")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FromStatus")
                        .HasColumnType("int");

                    b.Property<string>("FromUserAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromUserComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromUserLogin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("FromUserRating")
                        .HasColumnType("real");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("ToCopyId")
                        .HasColumnType("int");

                    b.Property<string>("ToCopyTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToCopyVisual")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ToStatus")
                        .HasColumnType("int");

                    b.Property<string>("ToUserAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToUserComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToUserLogin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("ToUserRating")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("ReBook.Models.Wish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EditionId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("EditionId");

                    b.HasIndex("UserId");

                    b.ToTable("Wishes");
                });

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.HasOne("ReBook.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReBook.Models.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookTag", b =>
                {
                    b.HasOne("ReBook.Models.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReBook.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CopyWish", b =>
                {
                    b.HasOne("ReBook.Models.Copy", null)
                        .WithMany()
                        .HasForeignKey("ListCopiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReBook.Models.Wish", null)
                        .WithMany()
                        .HasForeignKey("WishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ReBook.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ReBook.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReBook.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ReBook.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReBook.Models.Copy", b =>
                {
                    b.HasOne("ReBook.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReBook.Models.Edition", "Edition")
                        .WithMany()
                        .HasForeignKey("EditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReBook.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Book");

                    b.Navigation("Edition");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ReBook.Models.Edition", b =>
                {
                    b.HasOne("ReBook.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("ReBook.Models.Pretender", b =>
                {
                    b.HasOne("ReBook.Models.Copy", null)
                        .WithMany("Pretenders")
                        .HasForeignKey("CopyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReBook.Models.Wish", b =>
                {
                    b.HasOne("ReBook.Models.Edition", "Edition")
                        .WithMany()
                        .HasForeignKey("EditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReBook.Models.ApplicationUser", "User")
                        .WithMany("Wishes")
                        .HasForeignKey("UserId");

                    b.Navigation("Edition");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ReBook.Models.ApplicationUser", b =>
                {
                    b.Navigation("Wishes");
                });

            modelBuilder.Entity("ReBook.Models.Copy", b =>
                {
                    b.Navigation("Pretenders");
                });
#pragma warning restore 612, 618
        }
    }
}