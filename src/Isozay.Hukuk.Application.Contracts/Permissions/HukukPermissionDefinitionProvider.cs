using Isozay.Hukuk.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Isozay.Hukuk.Permissions
{

    public class HukukPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var hukukGroup = context.AddGroup(HukukPermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(HukukPermissions.MyPermission1, L("Permission:MyPermission1"));
            //var bookStoreGroup = context.AddGroup(HukukPermissions.GroupName, L("Permission:Clients"));

            var clientsPermission = hukukGroup.AddPermission(HukukPermissions.Clients.Default, L("Permission:Clients"));
            clientsPermission.AddChild(HukukPermissions.Clients.Create, L("Permission:Clients.Create"));
            clientsPermission.AddChild(HukukPermissions.Clients.Edit, L("Permission:Clients.Edit"));
            clientsPermission.AddChild(HukukPermissions.Clients.Delete, L("Permission:Clients.Delete"));

            var safesPermission = hukukGroup.AddPermission(HukukPermissions.Safes.Default, L("Permission:Safes"));
            safesPermission.AddChild(HukukPermissions.Safes.Create, L("Permission:Safes.Create"));
            safesPermission.AddChild(HukukPermissions.Safes.Edit, L("Permission:Safes.Edit"));
            safesPermission.AddChild(HukukPermissions.Safes.Delete, L("Permission:Safes.Delete"));

            var itemsPermission = hukukGroup.AddPermission(HukukPermissions.Items.Default, L("Permission:Items"));
            itemsPermission.AddChild(HukukPermissions.Items.Create, L("Permission:Items.Create"));
            itemsPermission.AddChild(HukukPermissions.Items.Edit, L("Permission:Items.Edit"));
            itemsPermission.AddChild(HukukPermissions.Items.Delete, L("Permission:Items.Delete"));


            var fichesPermission = hukukGroup.AddPermission(HukukPermissions.Fiches.Default, L("Permission:Fiches"));
            fichesPermission.AddChild(HukukPermissions.Fiches.Create, L("Permission:Fiches.Create"));
            fichesPermission.AddChild(HukukPermissions.Fiches.Edit, L("Permission:Fiches.Edit"));
            fichesPermission.AddChild(HukukPermissions.Fiches.Delete, L("Permission:Fiches.Delete"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<HukukResource>(name);
        }
    }
}