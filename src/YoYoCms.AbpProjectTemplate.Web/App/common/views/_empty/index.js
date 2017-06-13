(function () {
    appModule.controller('common.views.EMPTY.index', [
        '$scope', '$uibModal',
        function ($scope, $uibModal) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            //start from here...
        }
    ]);
})();