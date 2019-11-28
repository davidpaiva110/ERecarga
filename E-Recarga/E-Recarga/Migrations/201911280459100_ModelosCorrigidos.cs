namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelosCorrigidos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AspNetUserId", c => c.String(nullable: false));
            AddColumn("dbo.RedeProprietarias", "AspNetUserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RedeProprietarias", "AspNetUserId");
            DropColumn("dbo.Users", "AspNetUserId");
        }
    }
}
