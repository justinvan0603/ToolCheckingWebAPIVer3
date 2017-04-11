using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatBot.Models
{
    public partial class DEFACEWEBSITEContext : DbContext
    {
        public virtual DbSet<Configtype> Configtype { get; set; }
        public virtual DbSet<Domainchange> Domainchange { get; set; }
        public virtual DbSet<Domainprofile> Domainprofile { get; set; }
        public virtual DbSet<Features> Features { get; set; }
        public virtual DbSet<Ips> Ips { get; set; }
        public virtual DbSet<Listdomain> Listdomain { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Optionlinks> Optionlinks { get; set; }
        public virtual DbSet<Options> Options { get; set; }
        public virtual DbSet<ScheduleDt> ScheduleDt { get; set; }
        public virtual DbSet<Schedules> Schedules { get; set; }
        public virtual DbSet<Sysrole> Sysrole { get; set; }
        public virtual DbSet<Sysroledetail> Sysroledetail { get; set; }
        public virtual DbSet<UserDomain> UserDomain { get; set; }
        public virtual DbSet<Userconfig> Userconfig { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserConfigObject> UserConfigObject { get; set; }
        public virtual DbSet<ListdomainObject> ListdomainObject { get; set; }
        public virtual DbSet<OptionSearchObject> OptionSearchObject { get; set; }
        public virtual DbSet<UserDomainSearchObject> UserDomainSearchObject { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=27.0.12.24;Database=DEFACEWEBSITE;Integrated Security=False;User Id=deface;Password=123456");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configtype>(entity =>
            {
                entity.ToTable("configtype");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConfName)
                    .HasColumnName("CONF_NAME")
                    .HasMaxLength(500);

                entity.Property(e => e.ConfType)
                    .HasColumnName("CONF_TYPE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Notes)
                    .HasColumnName("NOTES")
                    .HasMaxLength(500);

                entity.Property(e => e.Prioritize).HasColumnName("PRIORITIZE");

                entity.Property(e => e.ValueType)
                    .HasColumnName("VALUE_TYPE")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Domainchange>(entity =>
            {
                entity.ToTable("domainchange");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Domain)
                    .HasColumnName("DOMAIN")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.DomainId).HasColumnName("DOMAIN_ID");

                entity.Property(e => e.IsLeaf)
                    .HasColumnName("IS_LEAF")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.ParentId).HasColumnName("PARENT_ID");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Values)
                    .HasColumnName("VALUES")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Domainprofile>(entity =>
            {
                entity.ToTable("domainprofile");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Domain)
                    .HasColumnName("DOMAIN")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.DomainId).HasColumnName("DOMAIN_ID");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Values)
                    .HasColumnName("VALUES")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Features>(entity =>
            {
                entity.ToTable("features");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproveDt)
                    .HasColumnName("APPROVE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.AuthStatus)
                    .HasColumnName("AUTH_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.CheckerId)
                    .HasColumnName("CHECKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.Contents)
                    .HasColumnName("CONTENTS")
                    .HasMaxLength(500);

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.EditDt)
                    .HasColumnName("EDIT_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.EditorId)
                    .HasColumnName("EDITOR_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.FeaType)
                    .HasColumnName("FEA_TYPE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Level).HasColumnName("LEVEL");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.RecordStatus)
                    .HasColumnName("RECORD_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Resource)
                    .HasColumnName("RESOURCE")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Ips>(entity =>
            {
                entity.ToTable("ips");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasColumnName("domain")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Listdomain>(entity =>
            {
                entity.ToTable("listdomain");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproveDt)
                    .HasColumnName("APPROVE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.AuthStatus)
                    .HasColumnName("AUTH_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.CheckerId)
                    .HasColumnName("CHECKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(500);

                entity.Property(e => e.Domain)
                    .HasColumnName("DOMAIN")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.EditDt)
                    .HasColumnName("EDIT_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.EditorId)
                    .HasColumnName("EDITOR_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.RecordStatus)
                    .HasColumnName("RECORD_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.ToTable("logs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(500);

                entity.Property(e => e.LogCode).HasColumnName("LOG_CODE");

                entity.Property(e => e.LogType)
                    .HasColumnName("LOG_TYPE")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.TrnId).HasColumnName("TRN_ID");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menu");

                entity.Property(e => e.MenuId).HasColumnName("MENU_ID");

                entity.Property(e => e.AuthStatus)
                    .HasColumnName("AUTH_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.CheckerId)
                    .HasColumnName("CHECKER_ID")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.DateApprove)
                    .HasColumnName("DATE_APPROVE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Isapprove)
                    .HasColumnName("ISAPPROVE")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.IsapproveFunc)
                    .HasColumnName("ISAPPROVE_FUNC")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.MenuLink)
                    .HasColumnName("MENU_LINK")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasColumnName("MENU_NAME")
                    .HasMaxLength(100);

                entity.Property(e => e.MenuNameEl)
                    .HasColumnName("MENU_NAME_EL")
                    .HasMaxLength(100);

                entity.Property(e => e.MenuOrder).HasColumnName("MENU_ORDER");

                entity.Property(e => e.MenuParent)
                    .HasColumnName("MENU_PARENT")
                    .HasColumnType("varchar(6)");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.ToTable("messages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("CONTENT");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasColumnName("DOMAIN")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Icon)
                    .HasColumnName("ICON")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Keyterm)
                    .HasColumnName("KEYTERM")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE")
                    .HasMaxLength(500);

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'warning'");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasColumnName("USER")
                    .HasColumnType("varchar(15)");
            });

            modelBuilder.Entity<Optionlinks>(entity =>
            {
                entity.ToTable("optionlinks");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.DomainId)
                    .HasColumnName("DOMAIN_ID")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Link)
                    .HasColumnName("LINK")
                    .HasMaxLength(500);

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.OptionsId).HasColumnName("OPTIONS_ID");

                entity.Property(e => e.RecordStatus)
                    .HasColumnName("RECORD_STATUS")
                    .HasColumnType("varchar(1)");
            });

            modelBuilder.Entity<Options>(entity =>
            {
                entity.ToTable("options");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproveDt)
                    .HasColumnName("APPROVE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.AuthStatus)
                    .HasColumnName("AUTH_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.CheckerId)
                    .HasColumnName("CHECKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(500);

                entity.Property(e => e.DomainId)
                    .HasColumnName("DOMAIN_ID")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.EditDt)
                    .HasColumnName("EDIT_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.EditorId)
                    .HasColumnName("EDITOR_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.IsLimit)
                    .HasColumnName("IS_LIMIT")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.RecordStatus)
                    .HasColumnName("RECORD_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Times).HasColumnName("TIMES");
            });

            modelBuilder.Entity<ScheduleDt>(entity =>
            {
                entity.ToTable("schedule_dt");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DomainId)
                    .HasColumnName("DOMAIN_ID")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.ExeDate)
                    .HasColumnName("EXE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Executed)
                    .HasColumnName("EXECUTED")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.LinkId)
                    .HasColumnName("LINK_ID")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.ResType)
                    .HasColumnName("RES_TYPE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SchDate)
                    .HasColumnName("SCH_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.SchTerm)
                    .HasColumnName("SCH_TERM")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Schedules>(entity =>
            {
                entity.ToTable("schedules");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.EventTime).HasColumnName("EVENT_TIME");

                entity.Property(e => e.ExecDate)
                    .HasColumnName("EXEC_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsLeaf)
                    .HasColumnName("IS_LEAF")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.Notes)
                    .HasColumnName("NOTES")
                    .HasMaxLength(500);

                entity.Property(e => e.PAlert)
                    .HasColumnName("P_ALERT")
                    .HasColumnType("numeric");

                entity.Property(e => e.PDone)
                    .HasColumnName("P_DONE")
                    .HasColumnType("numeric");

                entity.Property(e => e.PWarning)
                    .HasColumnName("P_WARNING")
                    .HasColumnType("numeric");

                entity.Property(e => e.ParentId).HasColumnName("PARENT_ID");

                entity.Property(e => e.ProcessStatus)
                    .HasColumnName("PROCESS_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.SchDate)
                    .HasColumnName("SCH_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.SchTerm)
                    .HasColumnName("SCH_TERM")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TotalDomain)
                    .HasColumnName("TOTAL_DOMAIN")
                    .HasColumnType("numeric");

                entity.Property(e => e.TotalLink)
                    .HasColumnName("TOTAL_LINK")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<Sysrole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK_SYSGROUP");

                entity.ToTable("sysrole");

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.AuthStatus)
                    .HasColumnName("AUTH_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.CheckerId)
                    .HasColumnName("CHECKER_ID")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.DateApprove)
                    .HasColumnName("DATE_APPROVE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Isapprove)
                    .HasColumnName("ISAPPROVE")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.RoleDesc)
                    .HasColumnName("ROLE_DESC")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Sysroledetail>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.MenuId })
                    .HasName("PK_SYSGRPMENU");

                entity.ToTable("sysroledetail");

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.MenuId)
                    .HasColumnName("MENU_ID")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Isapprove).HasColumnName("ISAPPROVE");

                entity.Property(e => e.Ischecknull).HasColumnName("ISCHECKNULL");

                entity.Property(e => e.Isclose).HasColumnName("ISCLOSE");

                entity.Property(e => e.Isdelete).HasColumnName("ISDELETE");

                entity.Property(e => e.Isedit).HasColumnName("ISEDIT");

                entity.Property(e => e.Isinsert).HasColumnName("ISINSERT");

                entity.Property(e => e.Issearch).HasColumnName("ISSEARCH");

                entity.Property(e => e.Isupdate).HasColumnName("ISUPDATE");

                entity.Property(e => e.Isview).HasColumnName("ISVIEW");
            });

            modelBuilder.Entity<UserDomain>(entity =>
            {
                entity.ToTable("user_domain");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DomainId)
                    .HasColumnName("DOMAIN_ID")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Notes)
                    .HasColumnName("NOTES")
                    .HasMaxLength(500);

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Userconfig>(entity =>
            {
                entity.ToTable("userconfig");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConfType)
                    .HasColumnName("CONF_TYPE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ConfValue)
                    .HasColumnName("CONF_VALUE")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproveDt)
                    .HasColumnName("APPROVE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Apptoken)
                    .HasColumnName("APPTOKEN")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.AuthStatus)
                    .HasColumnName("AUTH_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.CheckerId)
                    .HasColumnName("CHECKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(500);

                entity.Property(e => e.EditDt)
                    .HasColumnName("EDIT_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.EditorId)
                    .HasColumnName("EDITOR_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Fullname)
                    .HasColumnName("FULLNAME")
                    .HasMaxLength(200);

                entity.Property(e => e.MakerId)
                    .HasColumnName("MAKER_ID")
                    .HasColumnType("varchar(15)");

                entity.Property(e => e.ParentId).HasColumnName("PARENT_ID");

                entity.Property(e => e.Password)
                    .HasColumnName("PASSWORD")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Phone).HasColumnName("PHONE");

                entity.Property(e => e.RecordStatus)
                    .HasColumnName("RECORD_STATUS")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasColumnType("varchar(50)");
            });
        }
    }
}