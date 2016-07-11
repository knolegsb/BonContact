namespace BonContact.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateDataInPeople : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.People (FirstName, LastName, DateAdded, Interests) Values ('Sean', 'John', 2016/02/21, 'Sports')");
        }
        
        public override void Down()
        {
        }
    }
}
