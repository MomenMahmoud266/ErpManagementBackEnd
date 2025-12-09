using System.Collections.Generic;

namespace ErpManagement.Domain.Constants.Statics;

public static class PermissionsStatic
{
    //public static List<GetPermissionsWithActions> GenerateAllPermissions()
    //{
    //    List<GetPermissionsWithActions> allPermissions = [];

    //    var modules = Enum.GetValues(typeof(PermissionsModulesEnum));

    //    foreach (var module in modules)
    //        allPermissions.AddRange(GeneratePermissionsList(module.ToString()!));

    //    return allPermissions;
    //}

    public static List<string> GenerateAllPermissions()
    {
        List<string> allPermissions = [];

        var modules = Enum.GetValues(typeof(PermissionsModulesEnum));

        foreach (var module in modules)
            allPermissions.AddRange(GeneratePermissionsList(module.ToString()!));

        return allPermissions;
    }

    public static List<string> GeneratePermissionsList(string module)
    {
        if (module == "Auth")
            return
                [
                    $"Permissions.{module}.View"
                ];

        else
            return
                [
                    $"Permissions.{module}.View",
                    $"Permissions.{module}.Create",
                    $"Permissions.{module}.Update",
                    $"Permissions.{module}.Delete",
                    $"Permissions.{module}.Print"
                ];
    }


    //public static List<GetPermissionsWithActions> GeneratePermissionsList(string module)
    //{
    //    if (module == "Auth")
    //        return
    //        [
    //            new()
    //            {
    //                ClaimValue = $"Permissions.{module}.View",
    //                NameAr = "التحكم فى كل الصلاحيات",
    //                NameEn = "Control all permissions",
    //                NameTr = "Tüm güçleri kontrol et"
    //            }
    //        ];

    //    else
    //        return
    //        [
    //            new()
    //            {
    //                ClaimValue = $"Permissions.{module}.View",
    //                NameAr = "عرض",
    //                NameEn = "View",
    //                NameTr = "Görüş"
    //            },
    //            new()
    //            {
    //                ClaimValue = $"Permissions.{module}.Create",
    //                NameAr = "إضافة",
    //                NameEn = "Create",
    //                NameTr = "Yaratmak"
    //            },
    //            new()
    //            {
    //                ClaimValue = $"Permissions.{module}.Update",
    //                NameAr = "تعديل",
    //                NameEn = "Update",
    //                NameTr = "Güncelleme"
    //            },
    //            new()
    //            {
    //                ClaimValue = $"Permissions.{module}.Delete",
    //                NameAr = "حذف",
    //                NameEn = "Delete",
    //                NameTr = "Silmek"
    //            },
    //            new()
    //            {
    //                ClaimValue = $"Permissions.{module}.Print",
    //                NameAr = "طباعة",
    //                NameEn = "Print",
    //                NameTr = "Yazdır"
    //            }
    //        ];
    //}

    public static class Auth
    {
        public const string View = "Permissions.Auth.View";
    }

    public static class Country
    {
        public const string View = "Permissions.Country.View";
        public const string Create = "Permissions.Country.Create";
        public const string Update = "Permissions.Country.Update";
        public const string Delete = "Permissions.Country.Delete";
        public const string ForceDelete = "Permissions.Country.ForceDelete";
        public const string Ptint = "Permissions.Country.Ptint";
    }
}