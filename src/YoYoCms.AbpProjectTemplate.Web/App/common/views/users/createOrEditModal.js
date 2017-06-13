(function () {
    appModule.controller('common.views.users.createOrEditModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.user', 'userId',
        function ($scope, $uibModalInstance, userService, userId) {
            var vm = this;

            vm.saving = false;
            vm.user = null;
            vm.profilePictureId = null;
            vm.roles = [];
            vm.setRandomPassword = (userId == null);
            vm.sendActivationEmail = (userId == null);
            vm.canChangeUserName = true;
            vm.isTwoFactorEnabled = abp.setting.getBoolean("Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled");
            vm.isLockoutEnabled = abp.setting.getBoolean("Abp.Zero.UserManagement.UserLockOut.IsEnabled");

            vm.save = function () {
                var assignedRoleNames = _.map(
                    _.where(vm.roles, { isAssigned: true }), //Filter assigned roles
                    function (role) {
                        return role.roleName; //Get names
                    });

                if (vm.setRandomPassword) {
                    vm.user.password = null;
                }

                vm.saving = true;
                userService.createOrUpdateUser({
                    user: vm.user,
                    assignedRoleNames: assignedRoleNames,
                    sendActivationEmail: vm.sendActivationEmail,
                    setRandomPassword: vm.setRandomPassword
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

            vm.getAssignedRoleCount = function () {
                return _.where(vm.roles, { isAssigned: true }).length;
            };

            function init() {
                userService.getUserForEdit({
                    id: userId
                }).then(function (result) {
                    vm.user = result.data.user;
                    vm.profilePictureId = result.data.profilePictureId;
                    vm.user.passwordRepeat = vm.user.password;
                    vm.roles = result.data.roles;
                    vm.canChangeUserName = vm.user.userName != app.consts.userManagement.defaultAdminUserName;
                });
            }

            init();
        }
    ]);
})();