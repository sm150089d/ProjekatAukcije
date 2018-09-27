namespace ProjekatAukcije.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StringMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfProductsPage = c.Int(nullable: false),
                        DefaultAuctionDuration = c.Int(nullable: false),
                        SilverPackTokens = c.Int(nullable: false),
                        GoldPackTokens = c.Int(nullable: false),
                        PlatinumPackTokens = c.Int(nullable: false),
                        Currency = c.String(nullable: false, maxLength: 3),
                        ValueOfToken = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Auction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Picture = c.Binary(nullable: false),
                        Duration = c.Int(nullable: false),
                        StartingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceInTokens = c.Int(nullable: false),
                        TokenValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateTimeCreated = c.DateTime(nullable: false),
                        DateTimeOpened = c.DateTime(),
                        DateTimeClosed = c.DateTime(),
                        Status = c.String(nullable: false, maxLength: 10),
                        Currency = c.String(nullable: false, maxLength: 3),
                        IdOfUser = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Bid",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AuctionId = c.Guid(nullable: false),
                        TimeStarted = c.DateTime(nullable: false),
                        NumberOfTokens = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auction", t => t.AuctionId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AuctionId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 200),
                        Password = c.String(nullable: false),
                        NumberOfTokens = c.Int(nullable: false),
                        idAspNetUsers = c.String(),
                        Role = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TokenOrder",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Int(nullable: false),
                        TypeOfTokens = c.String(nullable: false),
                        NumberOfTokens = c.Int(nullable: false),
                        PackagePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TokenOrder", "UserId", "dbo.User");
            DropForeignKey("dbo.Bid", "UserId", "dbo.User");
            DropForeignKey("dbo.Auction", "User_Id", "dbo.User");
            DropForeignKey("dbo.Bid", "AuctionId", "dbo.Auction");
            DropIndex("dbo.TokenOrder", new[] { "UserId" });
            DropIndex("dbo.Bid", new[] { "AuctionId" });
            DropIndex("dbo.Bid", new[] { "UserId" });
            DropIndex("dbo.Auction", new[] { "User_Id" });
            DropTable("dbo.TokenOrder");
            DropTable("dbo.User");
            DropTable("dbo.Bid");
            DropTable("dbo.Auction");
            DropTable("dbo.Admin");
        }
    }
}
