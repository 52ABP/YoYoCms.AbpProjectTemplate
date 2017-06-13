(function () {
    appModule.directive('buttonBusy', function () {
          return {
              restrict: 'A',
              scope: {
                  buttonBusy: '='
              },
              link: function ($scope, element, attrs) {

                  var disabledBefore = false;
                  var $button = $(element);
                  var $buttonInnerSpan = $button.find('span');
                  var buttonOriginalText = null;
                  
                  var $icon = $button.find('i');
                  var iconOriginalClasses = null;

                  $scope.$watch('buttonBusy', function () {
                      if ($scope.buttonBusy) {
                          //disable button
                          $button.attr('disabled', 'disabled');
                          //change icon
                          if ($icon.length) {
                              iconOriginalClasses = $icon.attr('class');
                              $icon.removeClass();
                              $icon.addClass('fa fa-spin fa-spinner');
                          }
                          //change text
                          if (attrs.busyText && $buttonInnerSpan.length) {
                              buttonOriginalText = $buttonInnerSpan.html();
                              $buttonInnerSpan.html(attrs.busyText);
                          }

                          disabledBefore = true;
                      } else {
                          if (!disabledBefore) {
                              return;
                          }

                          //enable button
                          $button.removeAttr('disabled');
                          //restore icon
                          if ($icon.length && iconOriginalClasses) {
                              $icon.removeClass();
                              $icon.addClass(iconOriginalClasses);
                          }
                          //restore text
                          if ($buttonInnerSpan.length && buttonOriginalText) {
                              $buttonInnerSpan.html(buttonOriginalText);
                          }
                      }
                  });
              }
          };
      });
})();