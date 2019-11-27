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
                        Cidade = c.String(),
                        Localizacao = c.String(),
                        Preco = c.Double(nullable: false),
                        RedeProprietariaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EstacaoId)
                .ForeignKey("dbo.RedeProprietarias", t => t.RedeProprietariaId, cascadeDelete: true)
                .Index(t => t.RedeProprietariaId);
            
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
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReservaId)
                .ForeignKey("dbo.Postoes", t => t.PostoId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostoId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Nif = c.String(),
                        NumeroCC = c.String(),
                        TitularCC = c.String(),
                        CCV = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.RedeProprietarias",
                c => new
                    {
                        RedeProprietariaId = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Nif = c.String(),
                        NumeroCC = c.String(),
                        TitularCC = c.String(),
                        CCV = c.String(),
                    })
                .PrimaryKey(t => t.RedeProprietariaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Estacaos", "RedeProprietariaId", "dbo.RedeProprietarias");
            DropForeignKey("dbo.Reservas", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reservas", "PostoId", "dbo.Postoes");
            DropForeignKey("dbo.Postoes", "EstacaoId", "dbo.Estacaos");
            DropIndex("dbo.Reservas", new[] { "UserId" });
            DropIndex("dbo.Reservas", new[] { "PostoId" });
            DropIndex("dbo.Postoes", new[] { "EstacaoId" });
            DropIndex("dbo.Estacaos", new[] { "RedeProprietariaId" });
            DropTable("dbo.RedeProprietarias");
            DropTable("dbo.Users");
            DropTable("dbo.Reservas");
            DropTable("dbo.Postoes");
            DropTable("dbo.Estacaos");
        }
    }
}
