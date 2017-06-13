(function () {
    appModule.controller('common.views.languages.texts', [
        '$scope', '$state', '$stateParams', '$uibModal', 'abp.services.app.language', 'uiGridConstants',
        function ($scope, $state, $stateParams, $uibModal, languageService, uiGridConstants) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            //Grid data
            var nonFilteredData = [];

            //Combobox values
            vm.sourceNames = [];
            vm.languages = [];

            //Filters
            vm.targetLanguageName = $stateParams.languageName;
            vm.sourceName = $stateParams.sourceName || 'AbpProjectTemplate';
            vm.baseLanguageName = $stateParams.baseLanguageName || abp.localization.currentLanguage.name;
            vm.targetValueFilter = $stateParams.targetValueFilter || 'ALL';
            vm.filterText = $stateParams.filterText || '';

            vm.loading = false;

            vm.gridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                appScopeProvider: vm,
                columnDefs: [
                    {
                        name: app.localize('Key'),
                        field: 'key',
                        minWidth: 70
                    },
                    {
                        name: app.localize('BaseValue'),
                        field: 'baseValue',
                        minWidth: 140
                    },
                    {
                        name: app.localize('TargetValue'),
                        field: 'targetValue',
                        minWidth: 140
                    },
                    {
                        name: app.localize('Edit'),
                        width: 60,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents text-center\">' +
                            '  <button ng-click="grid.appScope.openTextEditModal(row.entity)" class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>' +
                            '</div>'
                    }
                ],
                data: []
            };

            vm.applyFilters = function () {
                if (!vm.filterText && vm.targetValueFilter == 'ALL') {
                    vm.gridOptions.data = nonFilteredData;
                }

                var filterText = vm.filterText.trim().toLocaleLowerCase();
                vm.gridOptions.data = _.filter(nonFilteredData, function (text) {
                    if (vm.targetValueFilter == 'EMPTY' && text.targetValue) {
                        return false;
                    }

                    return (text.key && text.key.toLocaleLowerCase().indexOf(filterText) >= 0) ||
                    (text.baseValue && text.baseValue.toLocaleLowerCase().indexOf(filterText) >= 0) ||
                    (text.targetValue && text.targetValue.toLocaleLowerCase().indexOf(filterText) >= 0);
                });
            };

            vm.loadTexts = function () {
                vm.loading = true;
                languageService.getLanguageTexts({
                    sourceName: vm.sourceName,
                    baseLanguageName: vm.baseLanguageName,
                    targetLanguageName: vm.targetLanguageName
                }).then(function (result) {
                    nonFilteredData = result.data.items;
                    vm.applyFilters();
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.openTextEditModal = function(text) {
                $uibModal.open({
                    templateUrl: '~/App/common/views/languages/editTextModal.cshtml',
                    controller: 'common.views.languages.editTextModal as vm',
                    resolve: {
                        sourceName: function () {
                            return vm.sourceName;
                        },
                        baseLanguageName: function() {
                            return vm.baseLanguageName;
                        },
                        languageName: function () {
                            return vm.targetLanguageName;
                        },
                        allTexts: function() {
                            return vm.gridOptions.data;
                        },
                        initialText: function () {
                            return text;
                        }
                    }
                });
            };

            function initializeFilters() {
                var sources = _.filter(abp.localization.sources, function(source) {
                    return source.type == 'MultiTenantLocalizationSource';
                });

                vm.sourceNames = _.map(sources, function (value) {
                    return value.name;
                });

                vm.languages = abp.localization.languages;

                setTimeout(function () {
                    $('#TextSourceSelectionCombobox').selectpicker('refresh');
                }, 0);

                function reloadWhenChange(variableName) {
                    $scope.$watch(variableName, function (newValue, oldValue) {
                        if (newValue == oldValue) {
                            return;
                        }

                        $state.go('languageTexts', {
                            languageName: vm.targetLanguageName,
                            sourceName: vm.sourceName,
                            baseLanguageName: vm.baseLanguageName,
                            targetValueFilter: vm.targetValueFilter,
                            filterText: vm.filterText
                        }, {
                            location: 'replace'
                        });
                    });
                }

                reloadWhenChange('vm.sourceName');
                reloadWhenChange('vm.baseLanguageName');
                reloadWhenChange('vm.targetLanguageName');
            }

            initializeFilters();
            vm.loadTexts();
        }
    ]);
})();