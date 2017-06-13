(function () {

    appModule.controller('common.views.users.index', [
        '$scope', '$uibModal', '$stateParams', 'uiGridConstants', 'abp.services.app.user',
        function ($scope, $uibModal, $stateParams, uiGridConstants, userService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.advancedFiltersAreShown = false;
            vm.filterText = $stateParams.filterText || '';
            vm.currentUserId = abp.session.userId;

            vm.permissions = {
                create: abp.auth.hasPermission('Pages.Administration.Users.Create'),
                edit: abp.auth.hasPermission('Pages.Administration.Users.Edit'),
                changePermissions: abp.auth.hasPermission('Pages.Administration.Users.ChangePermissions'),
                impersonation: abp.auth.hasPermission('Pages.Administration.Users.Impersonation'),
                'delete': abp.auth.hasPermission('Pages.Administration.Users.Delete'),
                roles: abp.auth.hasPermission('Pages.Administration.Roles')
            };

            vm.requestParams = {
                permission: '',
                role: '',
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.userGridOptions = {
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
                        width: 120,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-if="grid.appScope.permissions.impersonation && row.entity.id != grid.appScope.currentUserId" ng-click="grid.appScope.impersonate(row.entity)">' + app.localize('LoginAsThisUser') + '</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.edit" ng-click="grid.appScope.editUser(row.entity)">' + app.localize('Edit') + '</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.changePermissions" ng-click="grid.appScope.editPermissions(row.entity)">' + app.localize('Permissions') + '</a></li>' +
                            '      <li><a ng-click="grid.appScope.unlockUser(row.entity)">' + app.localize('Unlock') + '</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.delete" ng-click="grid.appScope.deleteUser(row.entity)">' + app.localize('Delete') + '</a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: app.localize('UserName'),
                        field: 'userName',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <img ng-if="row.entity.profilePictureId" ng-src="' + abp.appPath + 'Profile/GetProfilePictureById?id={{row.entity.profilePictureId}}" width="22" height="22" class="img-rounded img-profile-picture-in-grid" />' +
                            '  <img ng-if="!row.entity.profilePictureId" src="' + abp.appPath + 'Common/Images/default-profile-picture.png" width="22" height="22" class="img-rounded" />' +
                            '  {{COL_FIELD CUSTOM_FILTERS}} &nbsp;' +
                            '</div>',
                        minWidth: 140
                    },
                    {
                        name: app.localize('Name'),
                        field: 'name',
                        minWidth: 120
                    },
                    {
                        name: app.localize('Surname'),
                        field: 'surname',
                        minWidth: 120
                    },
                    {
                        name: app.localize('Roles'),
                        field: 'getRoleNames()',
                        enableSorting: false,
                        minWidth: 160
                    },
                    {
                        name: app.localize('EmailAddress'),
                        field: 'emailAddress',
                        minWidth: 200
                    },
                    {
                        name: app.localize('EmailConfirm'),
                        field: 'isEmailConfirmed',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span ng-show="row.entity.isEmailConfirmed" class="label label-success">' + app.localize('Yes') + '</span>' +
                            '  <span ng-show="!row.entity.isEmailConfirmed" class="label label-default">' + app.localize('No') + '</span>' +
                            '</div>',
                        minWidth: 80
                    },
                    {
                        name: app.localize('Active'),
                        field: 'isActive',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span ng-show="row.entity.isActive" class="label label-success">' + app.localize('Yes') + '</span>' +
                            '  <span ng-show="!row.entity.isActive" class="label label-default">' + app.localize('No') + '</span>' +
                            '</div>',
                        minWidth: 80
                    },
                    {
                        name: app.localize('LastLoginTime'),
                        field: 'lastLoginTime',
                        cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    },
                    {
                        name: app.localize('CreationTime'),
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'L\'',
                        minWidth: 100
                    }
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            vm.requestParams.sorting = null;
                        } else {
                            vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        vm.getUsers();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                        vm.requestParams.maxResultCount = pageSize;

                        vm.getUsers();
                    });
                },
                data: []
            };

            vm.getUsers = function () {
                vm.loading = true;
                userService.getUsers($.extend({ filter: vm.filterText }, vm.requestParams))
                    .then(function (result) {
                        vm.userGridOptions.totalItems = result.data.totalCount;
                        vm.userGridOptions.data = addRoleNamesField(result.data.items);
                    }).finally(function () {
                        vm.loading = false;
                    });
            };

            function addRoleNamesField(users) {
                for (var i = 0; i < users.length; i++) {
                    var user = users[i];
                    user.getRoleNames = function () {
                        var roleNames = '';
                        for (var j = 0; j < this.roles.length; j++) {
                            if (roleNames.length) {
                                roleNames = roleNames + ', ';
                            }
                            roleNames = roleNames + this.roles[j].roleName;
                        };

                        return roleNames;
                    }
                }

                return users;
            }

            vm.editUser = function (user) {
                openCreateOrEditUserModal(user.id);
            };

            vm.createUser = function () {
                openCreateOrEditUserModal(null);
            };

            vm.editPermissions = function (user) {
                $uibModal.open({
                    templateUrl: '~/App/common/views/users/permissionsModal.cshtml',
                    controller: 'common.views.users.permissionsModal as vm',
                    backdrop: 'static',
                    resolve: {
                        user: function () {
                            return user;
                        }
                    }
                });
            };

            vm.impersonate = function (user) {
                app.utils.removeCookie(abp.security.antiForgery.tokenCookieName);
                abp.ajax({
                    url: abp.appPath + 'Account/Impersonate',
                    data: JSON.stringify({
                        tenantId: abp.session.tenantId,
                        userId: user.id
                    })
                });
            };

            vm.deleteUser = function (user) {
                if (user.userName == app.consts.userManagement.defaultAdminUserName) {
                    abp.message.warn(app.localize("{0}UserCannotBeDeleted", app.consts.userManagement.defaultAdminUserName));
                    return;
                }

                abp.message.confirm(
                    app.localize('UserDeleteWarningMessage', user.userName),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            userService.deleteUser({
                                id: user.id
                            }).then(function () {
                                vm.getUsers();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.unlockUser = function (user) {
                userService.unlockUser({
                        id: user.id
                    })
                    .then(function() {
                        abp.notify.success(app.localize('UnlockedTheUser', user.userName));
                    });
            };

            vm.exportToExcel = function () {
                userService.getUsersToExcel({})
                    .then(function (result) {
                        app.downloadTempFile(result.data);
                    });
            };

            function openCreateOrEditUserModal(userId) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/common/views/users/createOrEditModal.cshtml',
                    controller: 'common.views.users.createOrEditModal as vm',
                    backdrop: 'static',
                    resolve: {
                        userId: function () {
                            return userId;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    vm.getUsers();
                });
            }

            vm.getUsers();
        }]);
})();