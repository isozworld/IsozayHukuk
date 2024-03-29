﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Isozay.Hukuk.EntityFrameworkCore
{

    [ReplaceDbContext(typeof(IIdentityDbContext))]
    [ReplaceDbContext(typeof(ITenantManagementDbContext))]
    [ConnectionStringName("Default")]
    public class HukukDbContext :
        AbpDbContext<HukukDbContext>,
        IIdentityDbContext,
        ITenantManagementDbContext
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */

        #region Entities from the modules

        /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
         * and replaced them for this DbContext. This allows you to perform JOIN
         * queries for the entities of these modules over the repositories easily. You
         * typically don't need that for other modules. But, if you need, you can
         * implement the DbContext interface of the needed module and use ReplaceDbContext
         * attribute just like IIdentityDbContext and ITenantManagementDbContext.
         *
         * More info: Replacing a DbContext of a module ensures that the related module
         * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
         */

        //Identity
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityClaimType> ClaimTypes { get; set; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
        public DbSet<IdentityLinkUser> LinkUsers { get; set; }

        // Tenant Management
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        #endregion

        public DbSet<Clients.Client> Clients { get; set; }
        public DbSet<Safes.Safe> Safes { get; set; }
        public DbSet<Safes.SafeTran> SafeTrans { get; set; }
        public DbSet<Clients.ClientTran> ClientTrans { get; set; }
        public DbSet<Currencies.Currency> Currencies { get; set; }
        public DbSet<Clients.ClientRelation> ClientRelations { get; set; }
        public DbSet<Fiches.Fiche> Fiches { get; set; }
        public DbSet<Fiches.FicheLine> FicheLines { get; set; }
        public DbSet<Fiches.FicheInstallment> FicheInstallments { get; set; }
        public DbSet<Items.Item> Items { get; set; }

        public HukukDbContext(DbContextOptions<HukukDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureIdentityServer();
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();

            /* Configure your own tables/entities inside here */

            builder.Entity<Clients.Client>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "Clients", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
                b.Property(x => x.Name).IsUnicode();
                //...
            });
            builder.Entity<Safes.Safe>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "Safes", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128).IsUnicode();
                //...
            });
            builder.Entity <Safes.SafeTran> (b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "SafeTrans", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                //...
            });
            builder.Entity<Clients.ClientTran>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "ClientTrans", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.FicheId).IsRequired(false);
                //...
            });
            builder.Entity<Clients.ClientRelation>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "ClientRelations", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                //...
            });
            builder.Entity<Currencies.Currency>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "Currencies", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                //...
            });
            builder.Entity<Fiches.Fiche>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "Fiches", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                //...
            });
            builder.Entity<Fiches.FicheLine>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "FicheLines", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                //...
            });
            builder.Entity<Fiches.FicheInstallment>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "FicheInstallments", HukukConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                //...
            });
            builder.Entity<Items.Item>(b =>
            {
                b.ToTable(HukukConsts.DbTablePrefix + "Items", HukukConsts.DbSchema);
                b.Property(x => x.Code).IsRequired().HasMaxLength(128).IsUnicode();
                //...
            });
        }
    }
}