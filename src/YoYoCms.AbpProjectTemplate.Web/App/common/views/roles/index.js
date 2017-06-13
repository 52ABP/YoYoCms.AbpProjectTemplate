(function () {

    appModule.controller('common.views.roles.index', [
        '$scope', '$uibModal', '$templateCache', 'abp.services.app.role', 'uiGridConstants',
        function ($scope, $uibModal, $templateCache, roleService, uiGridConstants) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.requestParams = {
                permission: ''
            };

            vm.permissions = {
                create: abp.auth.hasPermission('Pages.Administration.Roles.Create'),
                edit: abp.auth.hasPermission('Pages.Administration.Roles.Edit'),
                'delete': abp.auth.hasPermission('Pages.Administration.Roles.Delete')
            };

            vm.roleGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                appScopeProvider: vm,
                columnDefs: [
                    {
                        name: app.localize('Actions'),
                        enableSorting: false,
                        width: 120,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-if="grid.appScope.permissions.edit" ng-click="grid.appScope.editRole(row.entity)">' + app.localize('Edit') + '</a></li>' +
                            '      <li><a ng-if="!row.entity.isStatic && grid.appScope.permissions.delete" ng-click="grid.appScope.deleteRole(row.entity)">' + app.localize('Delete') + '</a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: app.localize('RoleName'),
                        field: 'displayName',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  {{COL_FIELD CUSTOM_FILTERS}} &nbsp;' +
                            '  <span ng-show="row.entity.isStatic" class="label label-info" uib-popover="' + app.localize('StaticRole_Tooltip') + '" popover-placement="bottom" popover-trigger="\'mouseenter click\'">' + app.localize('Static') + '</span>&nbsp;' +
                            '  <span ng-show="row.entity.isDefault" class="label label-default" uib-popover="' + app.localize('DefaultRole_Description') + '" popover-placement="bottom" popover-trigger="\'mouseenter click\'">' + app.localize('Default') + '</span>' +
                            '</div>'
                    },
                    {
                        name: app.localize('CreationTime'),
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'L\''
                    }
                ],
                data: []
            };

            if (!vm.permissions.edit && !vm.permissions.delete) {
                vm.roleGridOptions.columnDefs.shift();
            }

            vm.getRoles = function () {
                vm.loading = true;
                roleService.getRoles(vm.requestParams).then(function (result) {
                    vm.roleGridOptions.data = result.data.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.editRole = function (role) {
                openCreateOrEditRoleModal(role.id);
            };

            vm.deleteRole = function (role) {
                abp.message.confirm(
                    app.localize('RoleDeleteWarningMessage', role.displayName),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            roleService.deleteRole({
                                id: role.id
                            }).then(function () {
                                vm.getRoles();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.createRole = function () {
                openCreateOrEditRoleModal(null);
            };

            function openCreateOrEditRoleModal(roleId) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/common/views/roles/createOrEditModal.cshtml',
                    controller: 'common.views.roles.createOrEditModal as vm',
                    backdrop: 'static',
                    resolve: {
                        roleId: function () {
                            return roleId;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    vm.getRoles();
                });
            }

            vm.getRoles();
        }
    ]);
})();