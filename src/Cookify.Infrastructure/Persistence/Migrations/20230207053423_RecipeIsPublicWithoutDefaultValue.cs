using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cookify.Infrastructure.Persistence.Migrations
{
    public partial class RecipeIsPublicWithoutDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 350, DateTimeKind.Unspecified).AddTicks(1595), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 778, DateTimeKind.Unspecified).AddTicks(6094), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 349, DateTimeKind.Unspecified).AddTicks(5246), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 777, DateTimeKind.Unspecified).AddTicks(8134), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Recipes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 349, DateTimeKind.Unspecified).AddTicks(1113), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 777, DateTimeKind.Unspecified).AddTicks(2920), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RecipeCategories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 348, DateTimeKind.Unspecified).AddTicks(8991), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 776, DateTimeKind.Unspecified).AddTicks(9914), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProductMarkets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 348, DateTimeKind.Unspecified).AddTicks(8137), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 776, DateTimeKind.Unspecified).AddTicks(8667), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Likes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 348, DateTimeKind.Unspecified).AddTicks(4093), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 776, DateTimeKind.Unspecified).AddTicks(2947), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Ingredients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 347, DateTimeKind.Unspecified).AddTicks(6856), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 775, DateTimeKind.Unspecified).AddTicks(3309), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Favorites",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 347, DateTimeKind.Unspecified).AddTicks(2892), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 774, DateTimeKind.Unspecified).AddTicks(7601), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 778, DateTimeKind.Unspecified).AddTicks(6094), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 350, DateTimeKind.Unspecified).AddTicks(1595), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 777, DateTimeKind.Unspecified).AddTicks(8134), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 349, DateTimeKind.Unspecified).AddTicks(5246), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Recipes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 777, DateTimeKind.Unspecified).AddTicks(2920), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 349, DateTimeKind.Unspecified).AddTicks(1113), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RecipeCategories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 776, DateTimeKind.Unspecified).AddTicks(9914), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 348, DateTimeKind.Unspecified).AddTicks(8991), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProductMarkets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 776, DateTimeKind.Unspecified).AddTicks(8667), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 348, DateTimeKind.Unspecified).AddTicks(8137), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Likes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 776, DateTimeKind.Unspecified).AddTicks(2947), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 348, DateTimeKind.Unspecified).AddTicks(4093), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Ingredients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 775, DateTimeKind.Unspecified).AddTicks(3309), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 347, DateTimeKind.Unspecified).AddTicks(6856), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Favorites",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 0, 29, 28, 774, DateTimeKind.Unspecified).AddTicks(7601), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 2, 7, 5, 34, 23, 347, DateTimeKind.Unspecified).AddTicks(2892), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
