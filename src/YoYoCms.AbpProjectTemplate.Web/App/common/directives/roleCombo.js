(function () {
    appModule.directive('roleCombo', ['$timeout', 'abp.services.app.role',
        function ($timeout, roleService) {
            return {
                restrict: 'E',
                replace: true,
                templateUrl: '/App/common/directives/roleCombo.cshtml',
                scope: {
                    selectedRole: '=?'
                },
                link: function ($scope, element, attrs) {
                    $scope.roles = [];
                    $scope.emptyText = attrs.emptyText || '';

                    roleService.getRoles({}).then(function (result) {
                        $scope.roles = result.data.items;
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