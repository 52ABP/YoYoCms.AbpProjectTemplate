(function () {
    appModule.controller('common.views.layout.sidebar', [
        '$scope', '$state',
        function ($scope, $state) {
            var vm = this;

            vm.menu = abp.nav.menus.MainMenu;

            $scope.$on('$includeContentLoaded', function () {
                setTimeout(function () {
                    Layout.initSidebar($state); // init sidebar
                    $(window).trigger('resize');
                }, 0);                
            });
        }
    ]);
})();