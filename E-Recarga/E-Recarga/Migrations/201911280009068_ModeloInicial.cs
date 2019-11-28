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
                        Nome = c.String(nullable: false),
                        Nif = c.String(nullable: false, maxLength: 9),
                        NumeroCC = c.String(nullable: false, maxLength: 16),
                        TitularCC = c.String(nullable: false),
                        CCV = c.String(nullable: false, maxLength: 9),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.RedeProprietarias",
                c => new
                    {
                        RedeProprietariaId = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Nif = c.String(nullable: false, maxLength: 9),
                        NumeroCC = c.String(nullable: false, maxLength: 16),
                        TitularCC = c.String(nullable: false),
                        CCV = c.String(nullable: false, maxLength: 9),
                    })
                .PrimaryKey(t => t.RedeProprietariaId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Estacaos", "RedeProprietariaId", "dbo.RedeProprietarias");
            DropForeignKey("dbo.Reservas", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reservas", "PostoId", "dbo.Postoes");
            DropForeignKey("dbo.Postoes", "EstacaoId", "dbo.Estacaos");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reservas", new[] { "UserId" });
            DropIndex("dbo.Reservas", new[] { "PostoId" });
            DropIndex("dbo.Postoes", new[] { "EstacaoId" });
            DropIndex("dbo.Estacaos", new[] { "RedeProprietariaId" });
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
