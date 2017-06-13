(function () {
    appModule.controller('common.views.EMPTY.modal', [
        '$scope', '$uibModalInstance', 
        function ($scope, $uibModalInstance) {
            var vm = this;

            vm.saving = false;

            vm.save = function () {
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {

            }

            init();
        }
    ]);
})();