(function () {
    appModule.controller('host.views.editions.createOrEditModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.edition', 'editionId',
        function ($scope, $uibModalInstance, editionService, editionId) {
            var vm = this;

            vm.saving = false;
            vm.edition = null;
            vm.featureEditData = null;

            vm.save = function () {
                if (!vm.featureEditData.isValid()) {
                    abp.message.warn(app.localize('InvalidFeaturesWarning'));
                    return;
                }

                vm.saving = true;
                editionService.createOrUpdateEdition({
                    edition: vm.edition,
                    featureValues: vm.featureEditData.featureValues
                }).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function() {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                editionService.getEditionForEdit({
                    id: editionId
                }).then(function (result) {
                    vm.edition = result.data.edition;
                    vm.featureEditData = {
                        features: result.data.features,
                        featureValues: result.data.featureValues
                    };
                });
            }

            init();
        }
    ]);
})();