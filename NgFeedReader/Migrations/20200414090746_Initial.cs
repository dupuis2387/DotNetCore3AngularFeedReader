using Microsoft.EntityFrameworkCore.Migrations;

namespace NgFeedReader.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YoutubeVideoId",
                table: "FeedItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YoutubeVideoId",
                table: "FeedItems");
        }
    }
}
