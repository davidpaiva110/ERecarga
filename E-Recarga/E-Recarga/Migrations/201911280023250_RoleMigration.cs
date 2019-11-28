namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Estacaos", "Cidade", c => c.String(nullable: false));
            AlterColumn("dbo.Estacaos", "Localizacao", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Nome", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Nif", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.Users", "NumeroCC", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.Users", "TitularCC", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "CCV", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.RedeProprietarias", "Nome", c => c.String(nullable: false));
            AlterColumn("dbo.RedeProprietarias", "Nif", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.RedeProprietarias", "NumeroCC", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.RedeProprietarias", "TitularCC", c => c.String(nullable: false));
            AlterColumn("dbo.RedeProprietarias", "CCV", c => c.String(nullable: false, maxLength: 9));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RedeProprietarias", "CCV", c => c.String());
            AlterColumn("dbo.RedeProprietarias", "TitularCC", c => c.String());
            AlterColumn("dbo.RedeProprietarias", "NumeroCC", c => c.String());
            AlterColumn("dbo.RedeProprietarias", "Nif", c => c.String());
            AlterColumn("dbo.RedeProprietarias", "Nome", c => c.String());
            AlterColumn("dbo.Users", "CCV", c => c.String());
            AlterColumn("dbo.Users", "TitularCC", c => c.String());
            AlterColumn("dbo.Users", "NumeroCC", c => c.String());
            AlterColumn("dbo.Users", "Nif", c => c.String());
            AlterColumn("dbo.Users", "Nome", c => c.String());
            AlterColumn("dbo.Estacaos", "Localizacao", c => c.String());
            AlterColumn("dbo.Estacaos", "Cidade", c => c.String());
        }
    }
}
