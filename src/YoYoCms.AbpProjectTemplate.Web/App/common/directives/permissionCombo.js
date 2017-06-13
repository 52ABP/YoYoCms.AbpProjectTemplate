(function () {
    appModule.directive('permissionCombo', ['$timeout', 'abp.services.app.permission',
        function ($timeout, permissionService) {
            return {
                restrict: 'E',
                replace: true,
                templateUrl: '/App/common/directives/permissionCombo.cshtml',
                scope: {
                    selectedPermission: '=?'
                },
                link: function ($scope, element, attrs) {
                    $scope.permissions = [];
                    $scope.filterByPermissionText = app.localize('FilterByPermission');
                    $(element).addClass('edited');

                    permissionService.getAllPermissions({}).then(function (result) {

                        angular.forEach(result.data.items,
                            function (item) {
                                item.displayName = (Array(item.level + 1).join("--")) + ' ' + item.displayName;
                            });

                        $scope.permissions = result.data.items;
                        //refresh combo
                        $timeout(function () {
                            $(element).selectpicker('refresh');
                        });
                    });
                }
            };
        }
    ]);
})();