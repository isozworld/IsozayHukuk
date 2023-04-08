using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Isozay.Hukuk.Localization;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace Isozay.Hukuk.Blazor.Menus
{

    public class HukukMenuContributor : IMenuContributor
    {
        private readonly IConfiguration _configuration;

        public HukukMenuContributor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
            else if (context.Menu.Name == StandardMenus.User)
            {
                await ConfigureUserMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<HukukResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    HukukMenus.Home,
                    l["Menu:Home"],
                    "/",
                    icon: "fas fa-home"
                )
            );

            var Islemler =
             new ApplicationMenuItem(
                 "İşlemler",
                 "İşlemler",
                 icon: @"fas fa-file-import"
             );
            Islemler.AddItem(
             new ApplicationMenuItem(
                 "İşlemler",
                 "Fişler",
                 icon: "fas fa-circle",
                 url: "/fiches"
             )
         );


            var Sabitler =
             new ApplicationMenuItem(
                 "Sabitler",
                 "Sabitler",
                 icon: @"fas fa-file"
             ); 


            Sabitler.AddItem(
                 new ApplicationMenuItem(
                     "Sabitler",
                     "Müşteriler-Müvekkiller",
                      icon: "fas fa-circle",
                     url: "/clients"
                 )
             );
            Sabitler.AddItem(
                 new ApplicationMenuItem(
                     "Sabitler",
                     "Müvekkil Hesap Hareketleri",
                      icon: "fas fa-circle",
                     url: "/clienttrans"
                 )
             );
            Sabitler.AddItem(
             new ApplicationMenuItem(
                 "Sabitler",
                 "Kasa Tanımları",
                  icon: "fas fa-circle",
                 url: "/safes"
             )
         );
            Sabitler.AddItem(
             new ApplicationMenuItem(
                 "Sabitler",
                 "Stok - Dava Tanımları",
                  icon: "fas fa-circle",
                 url: "/items"
             )
         );
            context.Menu.Items.Insert(1, Islemler);
            context.Menu.Items.Insert(1, Sabitler);
            return Task.CompletedTask;
        }

        private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
        {
            var accountStringLocalizer = context.GetLocalizer<AccountResource>();

            var identityServerUrl = _configuration["AuthServer:Authority"] ?? "";

            context.Menu.AddItem(new ApplicationMenuItem(
                "Account.Manage",
                accountStringLocalizer["MyAccount"],
                $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
                icon: "fa fa-cog",
                order: 1000,
                null).RequireAuthenticated());

            return Task.CompletedTask;
        }
    }
}