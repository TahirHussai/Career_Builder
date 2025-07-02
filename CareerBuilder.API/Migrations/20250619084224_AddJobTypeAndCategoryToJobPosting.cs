using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerBuilder.API.Migrations
{
    /// <inheritdoc />
    public partial class AddJobTypeAndCategoryToJobPosting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "JobPostings",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "JobPostings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "JobPostings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "JobPostings",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "JobPostings");
        }
    }
}
