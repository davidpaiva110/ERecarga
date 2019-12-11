namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModeloFinal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RedeProprietarias", "CCV", c => c.String(nullable: false, maxLength: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RedeProprietarias", "CCV", c => c.String(nullable: false, maxLength: 9));
        }
    }
}
