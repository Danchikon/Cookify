﻿// <auto-generated />
using System;
using Cookify.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cookify.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(CookifyDbContext))]
    [Migration("20230202134124_AddedMealCategoryEntity")]
    partial class AddedMealCategoryEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Cookify.Domain.Favorite.FavoriteEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 2, 2, 13, 41, 24, 432, DateTimeKind.Unspecified).AddTicks(1397), new TimeSpan(0, 0, 0, 0, 0)));

                    b.Property<Guid?>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("RecipeId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("Cookify.Domain.Ingredient.IngredientEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 2, 2, 13, 41, 24, 432, DateTimeKind.Unspecified).AddTicks(4672), new TimeSpan(0, 0, 0, 0, 0)));

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasDefaultValue("");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Title");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Cookify.Domain.IngredientRecipe.IngredientRecipeEntity", b =>
                {
                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IngredientId")
                        .HasColumnType("uuid");

                    b.HasKey("RecipeId", "IngredientId");

                    b.HasIndex("IngredientId");

                    b.ToTable("IngredientRecipes");
                });

            modelBuilder.Entity("Cookify.Domain.Like.LikeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 2, 2, 13, 41, 24, 432, DateTimeKind.Unspecified).AddTicks(8456), new TimeSpan(0, 0, 0, 0, 0)));

                    b.Property<Guid?>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("RecipeId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Cookify.Domain.MealCategory.MealCategoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 2, 2, 13, 41, 24, 433, DateTimeKind.Unspecified).AddTicks(1512), new TimeSpan(0, 0, 0, 0, 0)));

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("MealCategories");
                });

            modelBuilder.Entity("Cookify.Domain.Recipe.RecipeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 2, 2, 13, 41, 24, 433, DateTimeKind.Unspecified).AddTicks(2845), new TimeSpan(0, 0, 0, 0, 0)));

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasDefaultValue("");

                    b.Property<string>("Instruction")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasDefaultValue("");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MealCategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("MealCategoryId");

                    b.HasIndex("Title");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Cookify.Domain.User.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTimeOffset(new DateTime(2023, 2, 2, 13, 41, 24, 433, DateTimeKind.Unspecified).AddTicks(6424), new TimeSpan(0, 0, 0, 0, 0)));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(42)
                        .HasColumnType("character varying(42)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Cookify.Domain.Favorite.FavoriteEntity", b =>
                {
                    b.HasOne("Cookify.Domain.User.UserEntity", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cookify.Domain.Recipe.RecipeEntity", "Recipe")
                        .WithMany("Favorites")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cookify.Domain.IngredientRecipe.IngredientRecipeEntity", b =>
                {
                    b.HasOne("Cookify.Domain.Ingredient.IngredientEntity", "Ingredient")
                        .WithMany("IngredientRecipes")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cookify.Domain.Recipe.RecipeEntity", "Recipe")
                        .WithMany("IngredientRecipes")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Cookify.Domain.Like.LikeEntity", b =>
                {
                    b.HasOne("Cookify.Domain.User.UserEntity", "User")
                        .WithMany("Likes")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cookify.Domain.Recipe.RecipeEntity", "Recipe")
                        .WithMany("Likes")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cookify.Domain.Recipe.RecipeEntity", b =>
                {
                    b.HasOne("Cookify.Domain.User.UserEntity", "User")
                        .WithMany("Recipes")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cookify.Domain.MealCategory.MealCategoryEntity", "MealCategory")
                        .WithMany("Recipes")
                        .HasForeignKey("MealCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MealCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cookify.Domain.Ingredient.IngredientEntity", b =>
                {
                    b.Navigation("IngredientRecipes");
                });

            modelBuilder.Entity("Cookify.Domain.MealCategory.MealCategoryEntity", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("Cookify.Domain.Recipe.RecipeEntity", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("IngredientRecipes");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Cookify.Domain.User.UserEntity", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("Likes");

                    b.Navigation("Recipes");
                });
#pragma warning restore 612, 618
        }
    }
}
