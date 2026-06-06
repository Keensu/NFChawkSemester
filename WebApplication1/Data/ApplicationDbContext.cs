using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NFChawk.Models.Entities;

namespace NFChawk.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<NFT> NFTs { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Role>().ToTable("roles");
            

            modelBuilder.Entity<NFT>(entity =>
            {
                entity.ToTable("nfts");

                entity.HasOne(n => n.Collection)
                      .WithMany(c => c.NFTs)
                      .HasForeignKey(n => n.CollectionId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(n => n.Author)
                      .WithMany(u => u.NFTs)
                      .HasForeignKey(n => n.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(n => n.Price)
                      .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("collections");

                entity.HasOne(c => c.Author)
                      .WithMany(u => u.Collections)
                      .HasForeignKey(c => c.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BlogPost>().ToTable("blogposts");

            modelBuilder.Entity<Auction>()
                        .Property<uint>("xmin")
                        .HasColumnType("xid")
                        .ValueGeneratedOnAddOrUpdate()
                        .IsConcurrencyToken();

            modelBuilder.Entity<Auction>().ToTable("auctions");


            modelBuilder.Entity<Auction>()
                 .HasOne(a => a.NFT)
                 .WithOne(n => n.Auction)
                 .HasForeignKey<Auction>(a => a.NFTId);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Seller)
                .WithMany(u => u.CreatedAuctions)
                .HasForeignKey(a => a.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Winner)
                .WithMany()
                .HasForeignKey(a => a.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bid>().ToTable("bids");

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Bidder)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.BidderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>().ToTable("transactions");

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
