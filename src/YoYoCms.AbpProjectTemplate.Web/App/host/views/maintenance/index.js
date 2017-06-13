(function () {
    appModule.controller('host.views.maintenance.index', [
        '$scope', '$window', 'abp.services.app.caching', 'abp.services.app.webLog',
        function ($scope, $window, cachingService, webLogService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.caches = null;
            vm.logs = '';

            //Caching
            vm.getCaches = function () {
                vm.loading = true;
                cachingService.getAllCaches()
                    .then(function (result) {
                        vm.caches = result.data.items;
                    }).finally(function () {
                        vm.loading = false;
                    });
            };

            vm.clearCache = function (cacheName) {
                cachingService.clearCache({
                    id: cacheName
                }).then(function () {
                    abp.notify.success(app.localize('CacheSuccessfullyCleared'));
                });
            }

            vm.clearAllCaches = function () {
                cachingService.clearAllCaches().then(function () {
                    abp.notify.success(app.localize('AllCachesSuccessfullyCleared'));
                });
            };

            //Web Logs
            vm.getWebLogs = function () {
                webLogService.getLatestWebLogs({}).then(function (result) {
                    vm.logs = vm.getFormattedLogs(result.data.latesWebLogLines);
                    fixWebLogsPanelHeight();
                });
            };

            vm.downloadWebLogs = function () {
                webLogService.downloadWebLogs({}).then(function (result) {
                    app.downloadTempFile(result.data);
                });
            };

            vm.getFormattedLogs = function (logLines) {
                var resultHtml = '';
                angular.forEach(logLines, function (logLine) {
                    resultHtml += '<span class="log-line">' + _.escape(logLine)
                    .replace('DEBUG', '<span class="label label-default">DEBUG</span>')
                    .replace('INFO', '<span class="label label-info">INFO</span>')
                    .replace('WARN', '<span class="label label-warning">WARN</span>')
                    .replace('ERROR', '<span class="label label-danger">ERROR</span>')
                    .replace('FATAL', '<span class="label label-danger">FATAL</span>') + '</span>';
                });
                return resultHtml;
            }

            function fixWebLogsPanelHeight() {
                var windowHeight = angular.element($window).height();
                var panelHeight = $('.full-height').height();
                var difference = windowHeight - panelHeight;
                var fixedHeight = panelHeight + difference;
                $('.full-height').css('height', (fixedHeight - 350) + 'px');
            }

            angular.element($window).bind('resize', function () {
                fixWebLogsPanelHeight();
            });

            vm.init = function () {
                vm.getCaches();
                vm.getWebLogs();
            }

            vm.init();
        }
    ]);
})();