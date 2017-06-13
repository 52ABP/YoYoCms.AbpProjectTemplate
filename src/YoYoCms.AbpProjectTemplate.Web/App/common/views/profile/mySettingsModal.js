(function () {
    appModule.controller('common.views.profile.mySettingsModal', [
        '$scope', 'appSession', '$uibModalInstance', 'abp.services.app.profile',
        function ($scope, appSession, $uibModalInstance, profileService) {
            var vm = this;

            var initialTimezone = null;

            vm.saving = false;
            vm.user = null;
            vm.canChangeUserName = true;
            vm.showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;

            vm.save = function () {
                vm.saving = true;
                profileService.updateCurrentUserProfile(vm.user)
                    .then(function () {
                        appSession.user.name = vm.user.name;
                        appSession.user.surname = vm.user.surname;
                        appSession.user.userName = vm.user.userName;
                        appSession.user.emailAddress = vm.user.emailAddress;
                        
                        abp.notify.info(app.localize('SavedSuccessfully'));

                        $uibModalInstance.close();
                        
                        if (abp.clock.provider.supportsMultipleTimezone && initialTimezone !== vm.user.timezone) {
                            abp.message.info(app.localize('TimeZoneSettingChangedRefreshPageNotification')).done(function() {
                                window.location.reload();
                            });
                        }

                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                profileService.getCurrentUserProfileForEdit({
                    id: appSession.user.id
                }).then(function (result) {
                    vm.user = result.data;
                    vm.canChangeUserName = vm.user.userName != app.consts.userManagement.defaultAdminUserName;
                    initialTimezone = vm.user.timezone;
                });
            }

            init();
        }
    ]);
})();