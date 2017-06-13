(function () {
    appModule.controller('tenant.views.settings.index', [
        '$scope', 'abp.services.app.tenantSettings',
        function ($scope, tenantSettingsService) {
            var vm = this;

            var usingDefaultTimeZone = false;
            var initialTimeZone = null;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.isMultiTenancyEnabled = abp.multiTenancy.isEnabled;
            vm.showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;
            vm.activeTabIndex = (!vm.isMultiTenancyEnabled || vm.showTimezoneSelection) ? 0 : 1;
            vm.loading = false;
            vm.settings = null;

            vm.getSettings = function () {
                vm.loading = true;
                tenantSettingsService.getAllSettings()
                    .then(function (result) {
                        vm.settings = result.data;
                        initialTimeZone = vm.settings.general.timezone;
                        usingDefaultTimeZone = vm.settings.general.timezoneForComparison === abp.setting.values["Abp.Timing.TimeZone"];
                    }).finally(function () {
                        vm.loading = false;
                    });
            };

            vm.saveAll = function () {
                tenantSettingsService.updateAllSettings(
                    vm.settings
                ).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));

                    if (abp.clock.provider.supportsMultipleTimezone && usingDefaultTimeZone && initialTimeZone !== vm.settings.general.timezone) {
                        abp.message.info(app.localize('TimeZoneSettingChangedRefreshPageNotification')).done(function () {
                            window.location.reload();
                        });
                    }
                });
            };

            vm.getSettings();
        }
    ]);
})();