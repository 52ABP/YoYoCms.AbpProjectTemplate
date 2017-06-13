(function () {
    appModule.controller('host.views.tenants.featuresModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.tenant', 'tenant',
        function ($scope, $uibModalInstance, tenantService, tenant) {
            var vm = this;
            
            vm.saving = false;
            vm.resettingFeatures = false;
            vm.tenant = tenant;
            vm.featureEditData = null;

            vm.resetFeatures = function () {
                vm.resettingFeatures = true;
                tenantService.resetTenantSpecificFeatures({
                    id: vm.tenant.id
                }).then(function () {
                    abp.notify.info(app.localize('ResetSuccessfully'));
                    loadFeatures();
                }).finally(function () {
                    vm.resettingFeatures = false;
                });
            };

            vm.save = function () {
                if (!vm.featureEditData.isValid()) {
                    abp.message.warn(app.localize('InvalidFeaturesWarning'));
                    return;
                }

                vm.saving = true;
                tenantService.updateTenantFeatures({
                    id: vm.tenant.id,
                    featureValues: vm.featureEditData.featureValues
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

            function loadFeatures() {
                tenantService.getTenantFeaturesForEdit({
                    id: vm.tenant.id
                }).then(function (result) {
                    vm.featureEditData = result.data;
                });
            }

            loadFeatures();
        }
    ]);
})();