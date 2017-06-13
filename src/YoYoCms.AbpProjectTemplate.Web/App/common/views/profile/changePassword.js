(function () {
    appModule.controller('common.views.profile.changePassword', [
        '$scope', 'appSession', '$uibModalInstance', 'abp.services.app.profile',
        function ($scope, appSession, $uibModalInstance, profileService) {
            var vm = this;

            vm.saving = false;
            vm.passwordInfo = null;
            vm.passwordComplexitySetting = {};
            vm.passwordComplexityMessages = {};

            var init = function () {
                profileService.getPasswordComplexitySetting().then(function (result) {
                    vm.passwordComplexitySetting = result.data.setting;

                    vm.passwordComplexityMessages = {
                        minLenght: abp.utils.formatString(app.localize("PasswordComplexity_MinLength_Hint"), vm.passwordComplexitySetting.minLength),
                        maxLenght: abp.utils.formatString(app.localize("PasswordComplexity_MaxLength_Hint"), vm.passwordComplexitySetting.maxLength),
                        useUpperCaseLetters: app.localize("PasswordComplexity_UseUpperCaseLetters_Hint"),
                        useLowerCaseLetters: app.localize("PasswordComplexity_UseLowerCaseLetters_Hint"),
                        useNumbers: app.localize("PasswordComplexity_UseNumbers_Hint"),
                        usePunctuations: app.localize("PasswordComplexity_UsePunctuations_Hint")
                    }
                });
            };

            vm.save = function () {
                vm.saving = true;
                profileService.changePassword(vm.passwordInfo)
                    .then(function () {
                        abp.notify.info(app.localize('YourPasswordHasChangedSuccessfully'));
                        $uibModalInstance.close();
                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            init();
        }
    ]);
})();