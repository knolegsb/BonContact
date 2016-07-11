namespace BonContact.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNullForAddressTypeAndAddedInterestsColumnInContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Interests", c => c.String());
            AlterColumn("dbo.Addresses", "AddressType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "AddressType", c => c.Int(nullable: false));
            DropColumn("dbo.People", "Interests");
        }
    }
}
