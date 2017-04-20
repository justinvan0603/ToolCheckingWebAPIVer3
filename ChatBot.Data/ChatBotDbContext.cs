using ChatBot.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatBot.Data
{
    public class ChatBotDbContext : IdentityDbContext<ApplicationUser>
    {
        public ChatBotDbContext(DbContextOptions<ChatBotDbContext> options) : base(options)
        {
        }


        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }
        //public DbSet<BOT_DOMAIN> BOT_DOMAINs { set; get; }
        public DbSet<MenuRole> MenuRoles { set; get; }
        public DbSet<Error> Errors { set; get; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            builder.Entity<IdentityUserRole<string>>().ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin<string>>().ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("ApplicationUserClaims");

            builder.Entity<IdentityUserToken<string>>().ToTable("ApplicationUserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("ApplicationRoleClaims");

            //builder.Entity("ChatBot.Model.Photo", b =>
            //{
            //    b.HasOne("ChatBot.Entities.ApplicationRoleGroup", "Album")
            //        .WithMany()
            //        .HasForeignKey("AlbumId")
            //        .OnDelete(DeleteBehavior.Cascade);
            //});
            builder.Entity<ApplicationRoleGroup>().HasKey(c => new { c.GroupId, c.RoleId });

            builder.Entity<ApplicationUserGroup>().HasKey(c => new { c.UserId, c.GroupId });
        }

    }
}