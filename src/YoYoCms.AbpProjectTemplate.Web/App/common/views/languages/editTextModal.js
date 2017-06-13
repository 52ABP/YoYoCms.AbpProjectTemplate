(function () {
    appModule.controller('common.views.languages.editTextModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.language', 'sourceName', 'baseLanguageName', 'languageName', 'allTexts', 'initialText',
        function ($scope, $uibModalInstance, languageService, sourceName, baseLanguageName, languageName, allTexts, initialText) {
            var vm = this;

            vm.sourceName = sourceName;
            vm.baseLanguageName = baseLanguageName;
            vm.languageName = languageName;
            vm.allTexts = allTexts;
            vm.currentText = initialText;

            vm.editingText = angular.extend({}, vm.currentText);
            vm.currentIndex = 0;
            vm.saving = false;

            //Calculating index of currentText in allTexts
            for (var i = 0; i < allTexts.length; i++) {
                if (vm.allTexts[i] == vm.currentText) {
                    vm.currentIndex = i;
                    break;
                }
            }

            vm.findFlag = function(languageName) {
                var language = _.findWhere(abp.localization.languages, { name: languageName });
                if (!language) {
                    return '';
                }

                return language.icon;
            };

            vm.findDisplayName = function (languageName) {
                var language = _.findWhere(abp.localization.languages, { name: languageName });
                if (!language) {
                    return '';
                }

                return language.displayName;
            };
            
            function save(close) {
                function executeAfterAction() {
                    if (close) {
                        $uibModalInstance.close();
                    } else {
                        //Go to next
                        vm.currentIndex++;
                        if (vm.allTexts.length <= vm.currentIndex) {
                            $uibModalInstance.close();
                            return;
                        }

                        vm.currentText = vm.allTexts[vm.currentIndex];
                        vm.editingText = angular.extend({}, vm.currentText);
                    }
                }

                if (vm.currentText.targetValue == vm.editingText.targetValue) {
                    executeAfterAction();
                    return;
                }

                vm.saving = true;
                languageService.updateLanguageText({
                    sourceName: vm.sourceName,
                    languageName: vm.languageName,
                    key: vm.editingText.key,
                    value: vm.editingText.targetValue
                }).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    vm.currentText.targetValue = vm.editingText.targetValue;
                    executeAfterAction();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.saveAndNext = function() {
                save(false);
            }

            vm.saveAndClose = function () {
                save(true);
            }

            vm.previous = function () {
                if (vm.currentIndex <= 0) {
                    return;
                }

                vm.currentIndex--;
                vm.currentText = vm.allTexts[vm.currentIndex];
                vm.editingText = angular.extend({}, vm.currentText);
            };

            vm.targetValueKeyPress = function ($event) {
                if ($event.key == 'PageDown') {
                    vm.saveAndNext();
                } else if ($event.key == 'PageUp') {
                    vm.previous();
                }
            }

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();