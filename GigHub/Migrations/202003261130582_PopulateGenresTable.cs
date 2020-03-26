namespace GigHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PopulateGenresTable : DbMigration
    {
        public override void Up()
        {
            Sql("Insert into Genres (Id, Name) values (1,'Rock')");
            Sql("Insert into Genres (Id, Name) values (2,'Jazz')");
            Sql("Insert into Genres (Id, Name) values (3,'Techno')");
            Sql("Insert into Genres (Id, Name) values (4,'Country')");
        }

        public override void Down()
        {
            Sql("Delete from Genres where Id in (1,2,3,4)");
        }
    }
}
