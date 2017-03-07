namespace WebCreateQR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactDetails : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Contacts");
            AddColumn("dbo.Contacts", "ContactID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Contacts", "DisplayName", c => c.String());
            AddPrimaryKey("dbo.Contacts", "ContactID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Contacts");
            AlterColumn("dbo.Contacts", "DisplayName", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Contacts", "ContactID");
            AddPrimaryKey("dbo.Contacts", "DisplayName");
        }
    }
}
