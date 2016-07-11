namespace BonContact.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModelAndDatbaseInitialization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        Line1 = c.String(),
                        Line2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        Country = c.String(),
                        AddressType = c.Int(nullable: false),
                        ContactID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.People", t => t.ContactID, cascadeDelete: true)
                .Index(t => t.ContactID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        DateAdded = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FilePaths",
                c => new
                    {
                        FilePathID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileType = c.Int(nullable: false),
                        PersonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FilePathID)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        PersonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileID)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "PersonID", "dbo.People");
            DropForeignKey("dbo.FilePaths", "PersonID", "dbo.People");
            DropForeignKey("dbo.Addresses", "ContactID", "dbo.People");
            DropIndex("dbo.Files", new[] { "PersonID" });
            DropIndex("dbo.FilePaths", new[] { "PersonID" });
            DropIndex("dbo.Addresses", new[] { "ContactID" });
            DropTable("dbo.Files");
            DropTable("dbo.FilePaths");
            DropTable("dbo.People");
            DropTable("dbo.Addresses");
        }
    }
}
