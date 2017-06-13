(function () {
    appModule.controller('common.views.profile.linkAccountModal', [
        '$scope', 'appSession', '$uibModalInstance', 'uiGridConstants', '$uibModal', 'abp.services.app.userLink',
        function ($scope, appSession, $uibModalInstance, uiGridConstants, $uibModal, userLinkService) {
            var vm = this;

            vm.saving = false;

            vm.linkUser = {
                tenancyName: '',
                usernameOrEmailAddress: '',
                password: ''
            };

            vm.save = function () {
                vm.saving = true;
                userLinkService.linkToUser(vm.linkUser)
                    .then(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $uibModalInstance.close();
                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();