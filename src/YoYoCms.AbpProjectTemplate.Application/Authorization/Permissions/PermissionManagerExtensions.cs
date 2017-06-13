using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization;
using Abp.Runtime.Validation;

namespace YoYoCms.AbpProjectTemplate.Authorization.Permissions
{
    public static class PermissionManagerExtensions
    {
        /// <summary>
        /// Gets all permissions by names.
        /// Throws <see cref="AbpValidationException"/> if can not find any of the permission names.
        /// </summary>
        public static IEnumerable<Abp.Authorization.Permission> GetPermissionsFromNamesByValidating(this IPermissionManager permissionManager, IEnumerable<string> permissionNames)
        {
            var permissions = new List<Abp.Authorization.Permission>();
            var undefinedPermissionNames = new List<string>();

            foreach (var permissionName in permissionNames)
            {
                var permission = permissionManager.GetPermissionOrNull(permissionName);
                if (permission == null)
                {
                    undefinedPermissionNames.Add(permissionName);
                }

                permissions.Add(permission);
            }

            if (undefinedPermissionNames.Count > 0)
            {
                throw new AbpValidationException(string.Format("There are {0} undefined permission names.", undefinedPermissionNames.Count))
                      {
                          ValidationErrors = undefinedPermissionNames.ConvertAll(permissionName => new ValidationResult("Undefined permission: " + permissionName))
                      };
            }

            return permissions;
        }
    }
}