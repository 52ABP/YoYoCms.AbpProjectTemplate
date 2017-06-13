(function () {
    appModule.controller('common.views.layout.header', [
        '$rootScope', '$scope', '$uibModal', 'appSession', 'appUserNotificationHelper', 'abp.services.app.notification', 'abp.services.app.userLink',
        function ($rootScope, $scope, $uibModal, appSession, appUserNotificationHelper, notificationService, userLinkService) {
            var vm = this;

            $scope.$on('$includeContentLoaded', function () {
                Layout.initHeader(); // init header
            });

            vm.languages = abp.localization.languages;
            vm.currentLanguage = abp.localization.currentLanguage;
            vm.isImpersonatedLogin = abp.session.impersonatorUserId;
            vm.notifications = [];
            vm.unreadNotificationCount = 0;
            vm.unreadChatMessageCount = 0;
            vm.recentlyUsedLinkedUsers = [];

            vm.getShownUserName = function () {
                if (!abp.multiTenancy.isEnabled) {
                    return appSession.user.userName;
                } else {
                    if (appSession.tenant) {
                        return appSession.tenant.tenancyName + '\\' + appSession.user.userName;
                    } else {
                        return '.\\' + appSession.user.userName;
                    }
                }
            };

            vm.getShownLinkedUserName = function (linkedUser) {
                return app.getShownLinkedUserName(linkedUser);
            };

            vm.editMySettings = function () {
                $uibModal.open({
                    templateUrl: '~/App/common/views/profile/mySettingsModal.cshtml',
                    controller: 'common.views.profile.mySettingsModal as vm',
                    backdrop: 'static'
                });
            };

            vm.changePassword = function () {
                $uibModal.open({
                    templateUrl: '~/App/common/views/profile/changePassword.cshtml',
                    controller: 'common.views.profile.changePassword as vm',
                    backdrop: 'static'
                });
            };

            vm.changePicture = function () {
                $uibModal.open({
                    templateUrl: '~/App/common/views/profile/changePicture.cshtml',
                    controller: 'common.views.profile.changePicture as vm',
                    backdrop: 'static'
                });
            };

            vm.changeLanguage = function (languageName) {
                location.href = abp.appPath + 'AbpLocalization/ChangeCulture?cultureName=' + languageName + '&returnUrl=' + window.location.pathname + window.location.hash;
            };

            vm.backToMyAccount = function () {
                abp.ajax({
                    url: abp.appPath + 'Account/BackToImpersonator'
                });
            }

            vm.loadNotifications = function () {
                notificationService.getUserNotifications({
                    maxResultCount: 3
                }).then(function (result) {
                    vm.unreadNotificationCount = result.data.unreadCount;
                    vm.notifications = [];
                    $.each(result.data.items, function (index, item) {
                        vm.notifications.push(appUserNotificationHelper.format(item));
                    });
                });
            }

            vm.setAllNotificationsAsRead = function () {
                appUserNotificationHelper.setAllAsRead();
            };

            vm.setNotificationAsRead = function (userNotification) {
                appUserNotificationHelper.setAsRead(userNotification.userNotificationId);
            }

            vm.openNotificationSettingsModal = function () {
                appUserNotificationHelper.openSettingsModal();
            };

            vm.manageLinkedAccounts = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/common/views/profile/linkedAccountsModal.cshtml',
                    controller: 'common.views.profile.linkedAccountsModal as vm',
                    backdrop: 'static'
                });

                modalInstance.result.finally(function () {
                    vm.getRecentlyUsedLinkedUsers();
                });
            };

            vm.getRecentlyUsedLinkedUsers = function () {
                userLinkService.getRecentlyUsedLinkedUsers()
                   .then(function (result) {
                       vm.recentlyUsedLinkedUsers = result.data.items;
                   }).finally(function () {
                       vm.loading = false;
                   });
            }

            vm.switchToUser = function (linkedUser) {
                abp.ajax({
                    url: abp.appPath + 'Account/SwitchToLinkedAccount',
                    data: JSON.stringify({
                        targetUserId: linkedUser.id,
                        targetTenantId: linkedUser.tenantId
                    }),
                    success: function() {
                        app.utils.removeCookie(abp.security.antiForgery.tokenCookieName);
                    }
                });
            };

            vm.showLoginAttempts = function () {
                $uibModal.open({
                    templateUrl: '~/App/common/views/users/loginAttemptsModal.cshtml',
                    controller: 'common.views.users.loginAttemptsModal as vm',
                    backdrop: 'static'
                });
            };

            abp.event.on('abp.notifications.received', function (userNotification) {
                appUserNotificationHelper.show(userNotification);
                vm.loadNotifications();
            });

            abp.event.on('app.notifications.refresh', function () {
                vm.loadNotifications();
            });

            abp.event.on('app.notifications.read', function (userNotificationId) {
                for (var i = 0; i < vm.notifications.length; i++) {
                    if (vm.notifications[i].userNotificationId == userNotificationId) {
                        vm.notifications[i].state = 'READ';
                    }
                }

                vm.unreadNotificationCount -= 1;
            });

            //Chat
            abp.event.on('app.chat.unreadMessageCountChanged', function (messageCount) {
                vm.unreadChatMessageCount = messageCount;
            });

            function init() {
                vm.loadNotifications();
                vm.getRecentlyUsedLinkedUsers();
            }

            init();
        }
    ]);
})();