(function () {

    appModule.controller('host.views.editions.index', [
        '$scope', '$uibModal', 'uiGridConstants', 'abp.services.app.edition',
        function ($scope, $uibModal, uiGridConstants, editionService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;

            vm.permissions = {
                create: abp.auth.hasPermission('Pages.Editions.Create'),
                edit: abp.auth.hasPermission('Pages.Editions.Edit'),
                'delete': abp.auth.hasPermission('Pages.Editions.Delete')
            };

            vm.editionGridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                appScopeProvider: vm,
                columnDefs: [
                    {
                        name: app.localize('Actions'),
                        width: 120,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <div class="btn-group dropdown" uib-dropdown="" dropdown-append-to-body>' +
                            '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                            '    <ul uib-dropdown-menu>' +
                            '      <li><a ng-if="grid.appScope.permissions.edit" ng-click="grid.appScope.editEdition(row.entity)">' + app.localize('Edit') + '</a></li>' +
                            '      <li><a ng-if="grid.appScope.permissions.delete" ng-click="grid.appScope.deleteEdition(row.entity)">' + app.localize('Delete') + '</a></li>' +
                            '    </ul>' +
                            '  </div>' +
                            '</div>'
                    },
                    {
                        name: app.localize('EditionName'),
                        field: 'displayName'
                    },
                    {
                        name: app.localize('CreationTime'),
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'L\''
                    }
                ],
                data: []
            };

            vm.getEditions = function () {
                vm.loading = true;
                editionService.getEditions({}).then(function (result) {
                    vm.editionGridOptions.data = result.data.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            function openCreateOrEditEditionModal(editionId) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/host/views/editions/createOrEditModal.cshtml',
                    controller: 'host.views.editions.createOrEditModal as vm',
                    backdrop: 'static',
                    resolve: {
                        editionId: function () {
                            return editionId;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    vm.getEditions();
                });
            }

            vm.createEdition = function() {
                openCreateOrEditEditionModal(null);
            };

            vm.editEdition = function(edition) {
                openCreateOrEditEditionModal(edition.id);
            };

            vm.deleteEdition = function(edition) {
                abp.message.confirm(
                    app.localize('EditionDeleteWarningMessage', edition.displayName),
                    function(isConfirmed) {
                        if (isConfirmed) {
                            editionService.deleteEdition({
                                id: edition.id
                            }).then(function () {
                                vm.getEditions();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.getEditions();
        }
    ]);
})();