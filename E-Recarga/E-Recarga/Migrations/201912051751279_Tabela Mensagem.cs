namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TabelaMensagem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mensagems",
                c => new
                    {
                        MensagemId = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MensagemId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Mensagems");
        }
    }
}
