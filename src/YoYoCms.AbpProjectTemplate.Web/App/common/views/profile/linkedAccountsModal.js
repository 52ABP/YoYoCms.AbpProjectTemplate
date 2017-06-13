(function () {
    appModule.controller('common.views.profile.linkedAccountsModal', [
        '$scope', 'appSession', '$uibModalInstance', 'uiGridConstants', '$uibModal', 'abp.services.app.userLink',
        function ($scope, appSession, $uibModalInstance, uiGridConstants, $uibModal, userLinkService) {
            var vm = this;

            vm.saving = false;

            var requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.linkedUsersGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: app.localize('Actions'),
                        enableSorting: false,
                        width: 100,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '    <button ng-click="grid.appScope.switchToUser(row.entity)" class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="icon-login"></i> ' + app.localize('LogIn') + '</button>' +
                            '</div>'
                    },
                    {
                        name: app.localize('UserName'),
                        field: 'username',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            ' {{ grid.appScope.getShownLinkedUserName(row.entity)}} &nbsp;' +
                            '</div>',
                        minWidth: 140
                    },
                    {
                        name: app.localize('Delete'),
                        enableSorting: false,
                        width: 100,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '    <button ng-click="grid.appScope.deleteLinkedUser(row.entity)" class="btn btn-xs btn-danger red" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="icon-trash"></i></button>' +
                            '</div>'
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            requestParams.sorting = null;
                        } else {
                            requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getLinkedUsers();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        requestParams.skipCount = (pageNumber - 1) * pageSize;
                        requestParams.maxResultCount = pageSize;

                        vm.getLinkedUsers();
                    });
                },
                data: []
            };

            vm.getShownLinkedUserName = function (linkedUser) {
                return app.getShownLinkedUserName(linkedUser);
            };

            vm.getLinkedUsers = function () {
                vm.loading = true;
                userLinkService.getLinkedUsers({
                    skipCount: requestParams.skipCount,
                    maxResultCount: requestParams.maxResultCount,
                    sorting: requestParams.sorting
                }).then(function (result) {
                    vm.linkedUsersGridOptions.totalItems = result.data.totalCount;
                    vm.linkedUsersGridOptions.data = result.data.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.manageLinkedAccounts = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/common/views/profile/linkAccountModal.cshtml',
                    controller: 'common.views.profile.linkAccountModal as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    vm.getLinkedUsers();
                });
            }

            vm.deleteLinkedUser = function (linkedUser) {
                abp.message.confirm(
                    app.localize('LinkedUserDeleteWarningMessage', linkedUser.username),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            userLinkService.unlinkUser({
                                userId: linkedUser.id,
                                tenantId: linkedUser.tenantId
                            }).then(function () {
                                vm.getLinkedUsers();
                                abp.notify.success(app.localize('SuccessfullyUnlinked'));
                            });
                        }
                    }
                );
            };

            vm.switchToUser = function (linkedUser) {
                abp.ajax({
                    url: abp.appPath + 'Account/SwitchToLinkedAccount',
                    data: JSON.stringify({
                        targetUserId: linkedUser.id,
                        targetTenantId: linkedUser.tenantId
                    }),
                    success: function () {
                        app.utils.removeCookie(abp.security.antiForgery.tokenCookieName);
                    }
                });
            };

            vm.cancel = function() {
                $uibModalInstance.dismiss('cancel');
            }

            function init() {
                vm.getLinkedUsers();
            }

            init();
        }
    ]);
})();