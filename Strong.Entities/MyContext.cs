using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Strong.Entities
{
    public partial class MyContext : DbContext
    {
        public MyContext()
        {

        }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {

        }

        public virtual DbSet<TbApilog> TbApilog { get; set; }
        public virtual DbSet<TbMenu> TbMenu { get; set; }
        public virtual DbSet<TbOperate> TbOperate { get; set; }
        public virtual DbSet<TbRole> TbRole { get; set; }
        public virtual DbSet<TbRoleLog> TbRoleLog { get; set; }
        public virtual DbSet<TbRoleLogProcess> TbRoleLogProcess { get; set; }
        public virtual DbSet<TbRoleMenu> TbRoleMenu { get; set; }
        public virtual DbSet<TbRoleOperate> TbRoleOperate { get; set; }
        public virtual DbSet<TbUser> TbUser { get; set; }
        public virtual DbSet<TbUserRole> TbUserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=192.168.31.199;Database=coretest;uid=coretest;pwd=coretest");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbApilog>(entity =>
            {
                entity.HasKey(e => e.Logid);

                entity.ToTable("TB_APILOG");

                entity.Property(e => e.Logid).HasColumnName("LOGID");

                entity.Property(e => e.Actiontime)
                    .HasColumnName("ACTIONTIME")
                    .HasColumnType("decimal(22, 0)");

                entity.Property(e => e.Apiname)
                    .HasColumnName("APINAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Createtime)
                    .HasColumnName("CREATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Errormsg)
                    .HasColumnName("ERRORMSG")
                    .HasMaxLength(2000);

                entity.Property(e => e.Errortype).HasColumnName("ERRORTYPE");

                entity.Property(e => e.Params)
                    .HasColumnName("PARAMS")
                    .HasColumnType("text");

                entity.Property(e => e.Parentid).HasColumnName("PARENTID");

                entity.Property(e => e.Requestid)
                    .HasColumnName("REQUESTID")
                    .HasMaxLength(200);

                entity.Property(e => e.Returntime)
                    .HasColumnName("RETURNTIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Userid).HasColumnName("USERID");
            });

            modelBuilder.Entity<TbMenu>(entity =>
            {
                entity.HasKey(e => e.Mflag);

                entity.ToTable("TB_MENU");

                entity.Property(e => e.Mflag)
                    .HasColumnName("MFLAG")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ico)
                    .HasColumnName("ICO")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("decimal(22, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Mlevel).HasColumnName("MLEVEL");

                entity.Property(e => e.Mname)
                    .HasColumnName("MNAME")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Mparent)
                    .HasColumnName("MPARENT")
                    .HasColumnType("decimal(22, 0)");

                entity.Property(e => e.Murl)
                    .HasColumnName("MURL")
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Mvisible)
                    .HasColumnName("MVISIBLE")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.Sort).HasColumnName("SORT");
            });

            modelBuilder.Entity<TbOperate>(entity =>
            {
                entity.ToTable("TB_OPERATE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("decimal(22, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Cname)
                    .HasColumnName("CNAME")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Isvalid)
                    .HasColumnName("ISVALID")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.Mflag)
                    .HasColumnName("MFLAG")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Opkey)
                    .IsRequired()
                    .HasColumnName("OPKEY")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opname)
                    .HasColumnName("OPNAME")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Tname)
                    .HasColumnName("TNAME")
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbRole>(entity =>
            {
                entity.HasKey(e => e.Roleid);

                entity.ToTable("TB_ROLE");

                entity.Property(e => e.Roleid).HasColumnName("ROLEID");

                entity.Property(e => e.Addtime)
                    .HasColumnName("ADDTIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Createuid).HasColumnName("CREATEUID");

                entity.Property(e => e.Identity)
                    .HasColumnName("IDENTITY")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Rname)
                    .HasColumnName("RNAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sort).HasColumnName("SORT");
            });

            modelBuilder.Entity<TbRoleLog>(entity =>
            {
                entity.ToTable("TB_ROLE_LOG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BeforeContent)
                    .HasColumnName("BEFORE_CONTENT")
                    .HasColumnType("text");

                entity.Property(e => e.Content)
                    .HasColumnName("CONTENT")
                    .HasColumnType("text");

                entity.Property(e => e.Oid)
                    .HasColumnName("OID")
                    .HasColumnType("decimal(22, 0)");

                entity.Property(e => e.OperateDate)
                    .HasColumnName("OPERATE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasMaxLength(2000);

                entity.Property(e => e.Savesql)
                    .HasColumnName("SAVESQL")
                    .HasColumnType("text");

                entity.Property(e => e.Sqlremark)
                    .HasColumnName("SQLREMARK")
                    .HasColumnType("text");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TbRoleLogProcess>(entity =>
            {
                entity.ToTable("TB_ROLE_LOG_PROCESS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lid)
                    .HasColumnName("LID")
                    .HasColumnType("decimal(22, 0)");

                entity.Property(e => e.Oman)
                    .HasColumnName("OMAN")
                    .HasMaxLength(50);

                entity.Property(e => e.Otime)
                    .HasColumnName("OTIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TbRoleMenu>(entity =>
            {
                entity.ToTable("TB_ROLE_MENU");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Mflag)
                    .HasColumnName("MFLAG")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Roleid).HasColumnName("ROLEID");
            });

            modelBuilder.Entity<TbRoleOperate>(entity =>
            {
                entity.ToTable("TB_ROLE_OPERATE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Opkey)
                    .HasColumnName("OPKEY")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Roleid).HasColumnName("ROLEID");
            });

            modelBuilder.Entity<TbUser>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("TB_USER");

                entity.Property(e => e.Userid).HasColumnName("USERID");

                entity.Property(e => e.Addtime)
                    .HasColumnName("ADDTIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Bhavelog)
                    .HasColumnName("BHAVELOG")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Createuid).HasColumnName("CREATEUID");

                entity.Property(e => e.Duty)
                    .HasColumnName("DUTY")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Issysadmin)
                    .HasColumnName("ISSYSADMIN")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.Loginname)
                    .IsRequired()
                    .HasColumnName("LOGINNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasColumnName("PWD")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Realname)
                    .IsRequired()
                    .HasColumnName("REALNAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseNumber).HasColumnName("RESPONSE_NUMBER");

                entity.Property(e => e.Sex)
                    .HasColumnName("SEX")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Sort).HasColumnName("SORT");

                entity.Property(e => e.Tel)
                    .HasColumnName("TEL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Town)
                    .HasColumnName("TOWN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Unitid).HasColumnName("UNITID");
            });

            modelBuilder.Entity<TbUserRole>(entity =>
            {
                entity.ToTable("TB_USER_ROLE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Roleid).HasColumnName("ROLEID");

                entity.Property(e => e.Userid).HasColumnName("USERID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
