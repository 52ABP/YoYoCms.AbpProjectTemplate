(function () {
    appModule.controller('common.views.notifications.index', [
        '$scope', '$uibModal', 'uiGridConstants', 'appUserNotificationHelper', 'abp.services.app.notification',
        function ($scope, $uibModal, uiGridConstants, appUserNotificationHelper, notificationService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.readStateFilter = 'ALL';
            var requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize
            };

            vm.gridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                enableSorting: false,
                appScopeProvider: vm,
                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'notification-read\' : row.entity.state == \'READ\' }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: app.localize('Actions'),
                        width: 80,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents text-center\">' +
                            '  <button ng-click="grid.appScope.setNotificationAsRead(row.entity)" ng-disabled="row.entity.state==\'READ\'" class="btn btn-xs btn-primary blue" title="' + app.localize('SetAsRead') + '">' +
                            '    <i class="fa" ng-class="{\'fa-check\': row.entity.state == \'READ\', \'fa-circle-o\': row.entity.state == \'UNREAD\'}"></i>' +
                            '  </button>' +
                            '</div>'
                    },
                    {
                        name: app.localize('Notification'),
                        field: 'text',
                        minWidth: 500,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <a ng-if="row.entity.url" ng-href="{{row.entity.url}}" title="{{row.entity.text}}">{{row.entity.text}}</a>' +
                            '  <span ng-if="!row.entity.url" title="{{row.entity.text}}">{{row.entity.text}}</span>' +
                            '</div>'
                    },
                    {
                        name: app.localize('CreationTime'),
                        field: 'time',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span title="{{row.entity.time | momentFormat: \'llll\'}}">{{COL_FIELD CUSTOM_FILTERS}}</span> &nbsp;' +
                            '</div>',
                        cellFilter: 'fromNow',
                        width: 150
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

                        vm.getNotifications();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        requestParams.skipCount = (pageNumber - 1) * pageSize;
                        requestParams.maxResultCount = pageSize;

                        vm.getNotifications();
                    });
                },
                data: []
            };

            vm.setAllNotificationsAsRead = function() {
                appUserNotificationHelper.setAllAsRead(function () {
                    vm.getNotifications();
                });
            };

            vm.openNotificationSettingsModal = function () {
                appUserNotificationHelper.openSettingsModal();
            };

            vm.setNotificationAsRead = function(userNotification) {
                appUserNotificationHelper.setAsRead(userNotification.userNotificationId, function() {
                    userNotification.state = 'READ';
                });
            };

            vm.getNotifications = function () {
                var state = null;
                if (vm.readStateFilter === 'UNREAD') {
                    state = abp.notifications.userNotificationState.UNREAD;
                }

                vm.loading = true;
                notificationService.getUserNotifications({
                    skipCount: requestParams.skipCount,
                    maxResultCount: requestParams.maxResultCount,
                    state: state
                }).then(function (result) {
                    vm.gridOptions.totalItems = result.data.totalCount;
                    vm.gridOptions.data = _.map(result.data.items, function(item) {
                        return appUserNotificationHelper.format(item, false);
                    });
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.getNotifications();
        }
    ]);
})();