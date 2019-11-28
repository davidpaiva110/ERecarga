namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelosCorrigidos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Estacaos", "RedeProprietariaId", "dbo.RedeProprietarias");
            DropForeignKey("dbo.Reservas", "UserId", "dbo.Users");
            DropIndex("dbo.Estacaos", new[] { "RedeProprietariaId" });
            DropIndex("dbo.Reservas", new[] { "UserId" });
            DropPrimaryKey("dbo.Users");
            DropPrimaryKey("dbo.RedeProprietarias");
            AlterColumn("dbo.Estacaos", "RedeProprietariaId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Reservas", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Users", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.RedeProprietarias", "RedeProprietariaId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "UserId");
            AddPrimaryKey("dbo.RedeProprietarias", "RedeProprietariaId");
            CreateIndex("dbo.Estacaos", "RedeProprietariaId");
            CreateIndex("dbo.Reservas", "UserId");
            AddForeignKey("dbo.Estacaos", "RedeProprietariaId", "dbo.RedeProprietarias", "RedeProprietariaId");
            AddForeignKey("dbo.Reservas", "UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservas", "UserId", "dbo.Users");
            DropForeignKey("dbo.Estacaos", "RedeProprietariaId", "dbo.RedeProprietarias");
            DropIndex("dbo.Reservas", new[] { "UserId" });
            DropIndex("dbo.Estacaos", new[] { "RedeProprietariaId" });
            DropPrimaryKey("dbo.RedeProprietarias");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.RedeProprietarias", "RedeProprietariaId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Users", "UserId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Reservas", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Estacaos", "RedeProprietariaId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.RedeProprietarias", "RedeProprietariaId");
            AddPrimaryKey("dbo.Users", "UserId");
            CreateIndex("dbo.Reservas", "UserId");
            CreateIndex("dbo.Estacaos", "RedeProprietariaId");
            AddForeignKey("dbo.Reservas", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Estacaos", "RedeProprietariaId", "dbo.RedeProprietarias", "RedeProprietariaId", cascadeDelete: true);
        }
    }
}
