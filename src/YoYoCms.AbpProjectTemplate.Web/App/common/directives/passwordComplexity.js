(function () {
    appModule.directive('passwordComplexity', [
        function () {
            return {
                restrict: 'A',
                require: 'ngModel',
                scope: {
                    passwordComplexitySetting: '='
                },
                link: function (scope, element, attr, ctrl) {

                    function customValidator(ngModelValue) {
                        var setting = scope.passwordComplexitySetting;

                        //Min Length
                        if (setting.minLength) {
                            if (ngModelValue.length >= setting.minLength) {
                                ctrl.$setValidity('passwordCompexityMinLengthValidator', true);
                            } else {
                                ctrl.$setValidity('passwordCompexityMinLengthValidator', false);
                            }
                        }

                        //Max Length
                        if (setting.maxLength) {
                            if (ngModelValue.length <= setting.maxLength) {
                                ctrl.$setValidity('passwordCompexityMaxLengthValidator', true);
                            } else {
                                ctrl.$setValidity('passwordCompexityMaxLengthValidator', false);
                            }
                        }

                        if (setting.useUpperCaseLetters) {
                            if (/[A-Z]/.test(ngModelValue)) {
                                ctrl.$setValidity('passwordCompexityUseUpperCaseLettersValidator', true);
                            } else {
                                ctrl.$setValidity('passwordCompexityUseUpperCaseLettersValidator', false);
                            }
                        }

                        if (setting.useLowerCaseLetters) {
                            if (/[a-z]/.test(ngModelValue)) {
                                ctrl.$setValidity('passwordCompexityUseLowerCaseLettersValidator', true);
                            } else {
                                ctrl.$setValidity('passwordCompexityUseLowerCaseLettersValidator', false);
                            }
                        }

                        if (setting.useNumbers) {
                            if (/[0-9]/.test(ngModelValue)) {
                                ctrl.$setValidity('passwordCompexityUseNumbersValidator', true);
                            } else {
                                ctrl.$setValidity('passwordCompexityUseNumbersValidator', false);
                            }
                        }

                        if (setting.usePunctuations) {
                            if (/[!@#\$%\^\&*'"\/{}\[\]?,;|)\(+=._-]+/.test(ngModelValue)) {
                                ctrl.$setValidity('passwordCompexityUsePunctuationsValidator', true);
                            } else {
                                ctrl.$setValidity('passwordCompexityUsePunctuationsValidator', false);
                            }
                        }

                        return ngModelValue;
                    }

                    ctrl.$parsers.push(customValidator);
                }
            };
        }
    ]);
})();