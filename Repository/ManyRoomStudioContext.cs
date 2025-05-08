using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Models.EntitiesView;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ManyRoomStudio.Repository
{
    public class ManyRoomStudioContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<EmailSetupDetail> EmailSetupDetails { get; set; }
        public DbSet<MasterDetail> MasterDetails { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingRoom> BookingRooms { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomEvent> RoomEvents { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<RoomStaffMapping>  RoomStaffMappings { get; set; }
        public DbSet<Franchiseekey>  Franchiseekeys { get; set; }
        public DbSet<RoomFranchiseeAdminMapping> RoomFranchiseeAdminMappings { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

        public DbSet<vwStaffModel> vwStaffModels { get; set; }

        public ManyRoomStudioContext(DbContextOptions<ManyRoomStudioContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User - Role (Many to One)
            builder.Entity<User>()
                .HasOne(u => u.Roles)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID);
            //.OnDelete(DeleteBehavior.Restrict);

            // Booking - User (Many to One)
            builder.Entity<Booking>()
                .HasOne(b => b.CustomerDetail)
                .WithMany()
                .HasForeignKey(b => b.UserID);
            //.OnDelete(DeleteBehavior.Restrict);

            // Booking - BookingRoom (One to Many)
            //builder.Entity<Booking>()
            //    .HasMany(b => b.BookingRoomList)
            //    .WithOne(br => br.BookingDetail)
            //    .HasForeignKey(br => br.BookingID);
            //.OnDelete(DeleteBehavior.Cascade);

            // BookingRoom - Room (Many to One)
            builder.Entity<BookingRoom>()
                .HasOne(br => br.RoomDetail)
                .WithMany()
                .HasForeignKey(br => br.RoomID);
            //.OnDelete(DeleteBehavior.Restrict);

            // Room - RoomImage (One to Many)
            builder.Entity<Room>()
                .HasMany(r => r.RoomImages)
                .WithOne(ri => ri.RoomDetail)
                .HasForeignKey(ri => ri.RoomID);
            //.OnDelete(DeleteBehavior.Cascade);

            // Room - RoomEvent (One to Many)
            builder.Entity<Room>()
                .HasMany(r => r.RoomEvents)
                .WithOne(re => re.RoomDetail)
                .HasForeignKey(re => re.RoomID);
            //.OnDelete(DeleteBehavior.Cascade);

            // RoomEvent - MasterDetail (Many to One)
            builder.Entity<RoomEvent>()
                .HasOne(re => re.EventDetail)
                .WithMany(md => md.RoomEventList)
                .HasForeignKey(re => re.EventID);
                //.OnDelete(DeleteBehavior.Restrict);
        }

        public static ManyRoomStudioContext Create()
        {
            DbContextOptionsBuilder<ManyRoomStudioContext> optionsBuilder = new DbContextOptionsBuilder<ManyRoomStudioContext>();

            var connectionString = AppConfig.Get("ConnectionStrings:DefaultConnection");
            if (connectionString != null)
                optionsBuilder.UseSqlServer(connectionString);
            else
                throw new Exception($"Connection string is null.");

            return new ManyRoomStudioContext(optionsBuilder.Options);
        }
    }

}
