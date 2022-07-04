namespace Isozay.Hukuk.Permissions
{

    public static class HukukPermissions
    {
        public const string GroupName = "Hukuk";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";
        public static class Clients
        {
            public const string Default = GroupName + ".Clients";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        public static class Safes
        {
            public const string Default = GroupName + ".Safes";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        public static class Items
        {
            public const string Default = GroupName + ".Items";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Fiches
        {
            public const string Default = GroupName + ".Fiches";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}