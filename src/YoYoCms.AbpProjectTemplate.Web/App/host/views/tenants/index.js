(function () {
    appModule.controller('host.views.tenants.index', [
        '$scope', '$stateParams', '$uibModal', 'uiGridConstants', 'abp.services.app.tenant', 'abp.services.app.commonLookup', 'lookupModal',
        function ($scope, $stateParams, $uibModal, uiGridConstants, tenantService, commonLookupService, lookupModal) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.filterText = $stateParams.filterText || null;

            vm.permissions = {
                create: abp.auth.hasPermission('Pages.Tenants.Create'),
                edit: abp.auth.hasPermission('Pages.Tenants.Edit'),
                changeFeatures: abp.auth.hasPermission('Pages.Tenants.ChangeFeatures'),
                impersonation: abp.auth.hasPermission('Pages.Tenants.Impersonation'),
                'delete': abp.auth.hasPermission('Pages.Tenants.Delete')
            };

            var requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.tenantGridOptions = {
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
                        width: 200,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-if="grid.appScope.permissions.impersonation" ng-click="grid.appScope.impersonate(row.entity)">' + app.localize('LoginAsThisTenant') + '</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.edit" ng-click="grid.appScope.editTenant(row.entity)">' + app.localize('Edit') + '</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.changeFeatures" ng-click="grid.appScope.editFeatures(row.entity)">' + app.localize('Features') + '</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.delete" ng-click="grid.appScope.deleteTenant(row.entity)">' + app.localize('Delete') + '</a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: app.localize('TenancyCodeName'),
                        field: 'tenancyName',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <i ng-show="row.entity.connectionString" title="' + app.localize('HasOwnDatabase') + '" class="fa fa-database"></i> {{row.entity.tenancyName}}' +
                            '</div>'
                    },
                    {
                        name: app.localize('Name'),
                        field: 'name'
                    },
                    {
                        name: app.localize('Edition'),
                        field: 'editionDisplayName'
                    },
                    {
                        name: app.localize('Active'),
                        field: 'isActive',
                        maxWidth: 100,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span ng-show="row.entity.isActive" class="label label-success">' + app.localize('Yes') + '</span>' +
                            '  <span ng-show="!row.entity.isActive" class="label label-default">' + app.localize('No') + '</span>' +
                            '</div>'
                    },
                    {
                        name: app.localize('CreationTime'),
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'L\''
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

                        vm.getTenants();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        requestParams.skipCount = (pageNumber - 1) * pageSize;
                        requestParams.maxResultCount = pageSize;

                        vm.getTenants();
                    });
                },
                data: []
            };

            if (!vm.permissions.edit && !vm.permissions.delete) {
                vm.tenantGridOptions.columnDefs.shift();
            }

            vm.getTenants = function () {
                vm.loading = true;
                tenantService.getTenants({
                    skipCount: requestParams.skipCount,
                    maxResultCount: requestParams.maxResultCount,
                    sorting: requestParams.sorting,
                    filter: vm.filterText
                }).then(function (result) {
                    vm.tenantGridOptions.totalItems = result.data.totalCount;
                    vm.tenantGridOptions.data = result.data.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.impersonate = function (tenant) {
                lookupModal.open({
                    title: app.localize('SelectAUser'),
                    serviceMethod: commonLookupService.findUsers,
                    extraFilters: {
                        tenantId: tenant.id
                    },
                    callback: function (selectedItem) {
                        app.utils.removeCookie(abp.security.antiForgery.tokenCookieName);
                        abp.ajax({
                            url: abp.appPath + 'Account/Impersonate',
                            data: JSON.stringify({
                                tenantId: tenant.id,
                                userId: selectedItem.value
                            })
                        });
                    }
                });
            };

            vm.editTenant = function (tenant) {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/host/views/tenants/editModal.cshtml',
                    controller: 'host.views.tenants.editModal as vm',
                    backdrop: 'static',
                    resolve: {
                        tenantId: function () {
                            return tenant.id;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    vm.getTenants();
                });
            };

            vm.createTenant = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '/App/host/views/tenants/createModal.cshtml',
                    controller: 'host.views.tenants.createModal as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    vm.getTenants();
                });
            };

            vm.deleteTenant = function (tenant) {
                abp.message.confirm(
                    app.localize('TenantDeleteWarningMessage', tenant.tenancyName),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            tenantService.deleteTenant({
                                id: tenant.id
                            }).then(function () {
                                vm.getTenants();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.editFeatures = function (tenant) {
                $uibModal.open({
                    templateUrl: '~/App/host/views/tenants/featuresModal.cshtml',
                    controller: 'host.views.tenants.featuresModal as vm',
                    backdrop: 'static',
                    resolve: {
                        tenant: function () {
                            return tenant;
                        }
                    }
                });
            };

            vm.getTenants();
        }]);
})();