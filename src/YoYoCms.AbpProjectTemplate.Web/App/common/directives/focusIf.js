(function () {
    'use strict';
    appModule.directive('focusIf', ['$timeout',
        function ($timeout) {
            return {
                restrict: 'A',
                link: function ($scope, $element, $attrs) {
                    if ($attrs.focusIf) {
                        $scope.$watch($attrs.focusIf, focus);
                    } else {
                        focus(true);
                    }
                    function focus(condition) {
                        if (condition) {
                            $timeout(function () {
                                $element.focus();
                            }, $scope.$eval($attrs.focusDelay) || 0);
                        }
                    }
                }
            };
        }
    ]);
})();