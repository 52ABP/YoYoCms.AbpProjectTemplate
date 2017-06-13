(function () {
    appModule.controller('common.views.languages.createOrEditModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.language', 'languageId',
        function ($scope, $uibModalInstance, languageService, languageId) {
            var vm = this;

            vm.saving = false;
            vm.language = null;
            vm.languageNames = [];
            vm.flags = [];
            vm.loading = false;

            vm.languageChanged = function () {
                //Try to find related language's flag
                if (!vm.language) {
                    return;
                }

                for (var i = 0; i < vm.flags.length; i++) {
                    if (vm.language.name.indexOf(vm.flags[i].displayText) == 0) {
                        vm.language.icon = vm.flags[i].value;
                        setTimeout(function () {
                            $('#LanguageIconSelectionCombobox').selectpicker('refresh');
                        }, 0);
                        break;
                    }
                }
            };

            vm.save = function () {
                vm.saving = true;
                languageService.createOrUpdateLanguage({
                    language: vm.language
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

            function init() {
                vm.loading = true;
                languageService.getLanguageForEdit({
                    id: languageId
                }).then(function (result) {
                    vm.language = result.data.language;
                    vm.languageNames = result.data.languageNames;
                    vm.flags = result.data.flags;
                    setTimeout(function () {
                        $('#LanguageIconSelectionCombobox').selectpicker('refresh');
                        $('#LanguageNameSelectionCombobox').selectpicker('refresh');
                    }, 0);
                    vm.loading = false;
                });
            }

            init();
        }
    ]);
})();