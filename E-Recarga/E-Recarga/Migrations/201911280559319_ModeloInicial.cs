namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModeloInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Estacaos",
                c => new
                    {
                        EstacaoId = c.Int(nullable: false, identity: true),
                        Cidade = c.String(nullable: false),
                        Localizacao = c.String(nullable: false),
                        Preco = c.Double(nullable: false),
                        RedeProprietariaID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EstacaoId)
                .ForeignKey("dbo.RedeProprietarias", t => t.RedeProprietariaID)
                .Index(t => t.RedeProprietariaID);
            
            CreateTable(
                "dbo.Postoes",
                c => new
                    {
                        PostoId = c.Int(nullable: false, identity: true),
                        Estado = c.Boolean(nullable: false),
                        EstacaoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostoId)
                .ForeignKey("dbo.Estacaos", t => t.EstacaoId, cascadeDelete: true)
                .Index(t => t.EstacaoId);
            
            CreateTable(
                "dbo.Reservas",
                c => new
                    {
                        ReservaId = c.Int(nullable: false, identity: true),
                        HorarioInicio = c.DateTime(nullable: false),
                        HorarioFim = c.DateTime(nullable: false),
                        PostoId = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReservaId)
                .ForeignKey("dbo.Postoes", t => t.PostoId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.PostoId)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Nif = c.String(nullable: false, maxLength: 9),
                        NumeroCC = c.String(nullable: false, maxLength: 16),
                        TitularCC = c.String(nullable: false),
                        CCV = c.String(nullable: false, maxLength: 9),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.RedeProprietarias",
                c => new
                    {
                        RedeProprietariaID = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Nif = c.String(nullable: false, maxLength: 9),
                        NumeroCC = c.String(nullable: false, maxLength: 16),
                        TitularCC = c.String(nullable: false),
                        CCV = c.String(nullable: false, maxLength: 9),
                    })
                .PrimaryKey(t => t.RedeProprietariaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Estacaos", "RedeProprietariaID", "dbo.RedeProprietarias");
            DropForeignKey("dbo.Reservas", "UserID", "dbo.Users");
            DropForeignKey("dbo.Reservas", "PostoId", "dbo.Postoes");
            DropForeignKey("dbo.Postoes", "EstacaoId", "dbo.Estacaos");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reservas", new[] { "UserID" });
            DropIndex("dbo.Reservas", new[] { "PostoId" });
            DropIndex("dbo.Postoes", new[] { "EstacaoId" });
            DropIndex("dbo.Estacaos", new[] { "RedeProprietariaID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RedeProprietarias");
            DropTable("dbo.Users");
            DropTable("dbo.Reservas");
            DropTable("dbo.Postoes");
            DropTable("dbo.Estacaos");
        }
    }
}
