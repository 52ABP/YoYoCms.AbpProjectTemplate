(function () {
    appModule.controller('common.views.users.permissionsModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'user',
        function ($scope, $uibModalInstance, userService, user) {
            var vm = this;
            
            vm.saving = false;
            vm.resettingPermissions = false;
            vm.user = user;
            vm.permissionEditData = null;

            vm.resetPermissions = function () {
                vm.resettingPermissions = true;
                userService.resetUserSpecificPermissions({
                    id: vm.user.id
                }).then(function () {
                    abp.notify.info(app.localize('ResetSuccessfully'));
                    loadPermissions();
                }).finally(function () {
                    vm.resettingPermissions = false;
                });
            };

            vm.save = function () {
                vm.saving = true;
                userService.updateUserPermissions({
                    id: vm.user.id,
                    grantedPermissionNames: vm.permissionEditData.grantedPermissionNames
                }).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function loadPermissions() {
                userService.getUserPermissionsForEdit({
                    id: vm.user.id
                }).then(function (result) {
                    vm.permissionEditData = result.data;
                });
            }

            loadPermissions();
        }
    ]);
})();