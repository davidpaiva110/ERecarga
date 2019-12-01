namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModeloReservasAtualizado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservas", "Data", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reservas", "HoraInicio", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reservas", "HoraFim", c => c.DateTime(nullable: false));
            DropColumn("dbo.Reservas", "HorarioInicio");
            DropColumn("dbo.Reservas", "HorarioFim");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservas", "HorarioFim", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reservas", "HorarioInicio", c => c.DateTime(nullable: false));
            DropColumn("dbo.Reservas", "HoraFim");
            DropColumn("dbo.Reservas", "HoraInicio");
            DropColumn("dbo.Reservas", "Data");
        }
    }
}
