using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Emarketing.Authorization
{
    public class EmarketingAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_WithdrawRequests, L("WithdrawRequests"));
            context.CreatePermission(PermissionNames.Pages_Packages, L("Packages"));
            context.CreatePermission(PermissionNames.Pages_PackageAds, L("PackageAds"));
            context.CreatePermission(PermissionNames.Pages_UserReferralRequests, L("UserReferralRequests"));
            context.CreatePermission(PermissionNames.Pages_UserRequests, L("UserRequests"));
            context.CreatePermission(PermissionNames.Pages_UserPackageSubscriptionDetails, L("UserPackageSubscriptionDetails"));
            context.CreatePermission(PermissionNames.Pages_UserPackageAdDetails, L("UserPackageAdDetails"));
            context.CreatePermission(PermissionNames.Pages_UserWithdrawDetails, L("UserWithdrawDetails"));
            context.CreatePermission(PermissionNames.Pages_UserPersonalDetails, L("UserPersonalDetails"));
            context.CreatePermission(PermissionNames.Pages_UserReferrals, L("UserReferrals"));
            
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EmarketingConsts.LocalizationSourceName);
        }
    }
}
