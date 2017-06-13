(function () {
    appModule.controller('common.views.welcome.index', [
        '$scope',
        function ($scope) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            //...
        }]);
})();