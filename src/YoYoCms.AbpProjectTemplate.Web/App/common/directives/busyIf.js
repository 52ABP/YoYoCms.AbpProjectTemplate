(function () {
    appModule.directive('busyIf', [
        function () {
            return {
                restrict: 'A',
                scope: {
                    busyIf: "="
                },
                link: function (scope, element, attrs) {
                    scope.$watch('busyIf', function () {
                        if (scope.busyIf) {
                            abp.ui.setBusy($(element));
                        } else {
                            abp.ui.clearBusy($(element));
                        }
                    });
                }
            };
        }
    ]);
})();