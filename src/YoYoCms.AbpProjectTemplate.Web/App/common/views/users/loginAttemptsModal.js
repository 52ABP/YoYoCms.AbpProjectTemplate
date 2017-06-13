(function () {
    appModule.controller('common.views.users.loginAttemptsModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.userLogin',
        function ($scope, $uibModalInstance, userLoginService) {
            var vm = this;

            vm.loginAttempts = [];

            vm.getUserLoginAttempts = function () {
                userLoginService.getRecentUserLoginAttempts({}).then(function (result) {
                    vm.loginAttempts = result.data.items;

                    angular.forEach(vm.loginAttempts, function (loginAttempt) {
                        loginAttempt.profileImageUrl = (loginAttempt.result === 'Success' ?
                                                (abp.appPath + 'Profile/GetProfilePicture?v=' + new Date().valueOf()) :
                                                '/Common/Images/default-profile-picture.png');
                    });
                });
            };

            vm.getLoginAttemptClass = function (loginAttempt) {
                return loginAttempt.result === 'Success' ? 'label-success' :
                    'label-danger';
            }

            vm.getCreationTime = function (loginAttempt) {
                return moment(loginAttempt.creationTime).fromNow() + ' (' + moment(loginAttempt.creationTime).format('YYYY-MM-DD hh:mm:ss') + ')';
            };

            vm.formatLoginAttemptResult = function (loginAttemt) {
                return loginAttemt.result === 'Success' ? app.localize('Success') :
                    app.localize('Failed');
            }

            vm.close = function() {
                $uibModalInstance.close();
            };

            function init() {
                vm.getUserLoginAttempts();
            }

            init();
        }
    ]);
})();