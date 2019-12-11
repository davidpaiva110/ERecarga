namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelosUsereRedeProprietariaCorrigidos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "CCV", c => c.String(nullable: false, maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "CCV", c => c.String(nullable: false, maxLength: 9));
        }
    }
}
