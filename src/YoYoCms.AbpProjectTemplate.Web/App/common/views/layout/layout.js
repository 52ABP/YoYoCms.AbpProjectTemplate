(function () {
    appModule.controller('common.views.layout', [
        '$scope',
        function ($scope) {
            $scope.$on('$viewContentLoaded', function () {
                App.initComponents(); // init core components
            });
        }
    ]);
})();