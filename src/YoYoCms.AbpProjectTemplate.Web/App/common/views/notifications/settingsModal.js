(function () {
    appModule.controller('common.views.notifications.settingsModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.notification',
        function ($scope, $uibModalInstance, notificationAppService) {
            var vm = this;

            vm.settings = null;

            vm.saving = false;

            vm.save = function () {
                vm.saving = true;
                notificationAppService.updateNotificationSettings(
                    vm.settings
                ).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                notificationAppService.getNotificationSettings({}).then(function (result) {
                    vm.settings = result.data;
                });
            }

            init();
        }
    ]);
})();