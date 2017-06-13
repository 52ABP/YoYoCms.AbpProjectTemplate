(function () {
    appModule.factory('appUserNotificationHelper', [
            '$uibModal', '$location', 'abp.services.app.notification',
            function ($uibModal, $location, notificationService) {

                /* Message Extracting based on Notification Data Type ********/

                //add your custom notification data types here...

                /* Example:
                abp.notifications.messageFormatters['MyCompanyName.AbpProjectTemplate.MyNotificationDataType'] = function(userNotification) {
                    return ...; //format and return message here
                };
                */

                /* Converter functions ***************************************/

                function getUiIconBySeverity(severity) {
                    switch (severity) {
                        case abp.notifications.severity.SUCCESS:
                            return 'fa fa-check';
                        case abp.notifications.severity.WARN:
                            return 'fa fa-warning';
                        case abp.notifications.severity.ERROR:
                            return 'fa fa-bolt';
                        case abp.notifications.severity.FATAL:
                            return 'fa fa-bomb';
                        case abp.notifications.severity.INFO:
                        default:
                            return 'fa fa-info';
                    }
                }

                function getUrl(userNotification) {
                    switch (userNotification.notification.notificationName) {
                        case 'App.NewUserRegistered':
                            return '#!/users?filterText=' + userNotification.notification.data.properties.emailAddress;
                        case 'App.NewTenantRegistered':
                            return '#!/host/tenants?filterText=' + userNotification.notification.data.properties.tenancyName;
                            //Add your custom notification names to navigate to a URL when user clicks to a notification.
                    }

                    //No url for this notification
                    return null;
                }

                /* PUBLIC functions ******************************************/

                var format = function (userNotification, truncateText) {
                    var formatted = {
                        userNotificationId: userNotification.id,
                        text: abp.notifications.getFormattedMessageFromUserNotification(userNotification),
                        time: userNotification.notification.creationTime,
                        icon: getUiIconBySeverity(userNotification.notification.severity),
                        state: abp.notifications.getUserNotificationStateAsString(userNotification.state),
                        data: userNotification.notification.data,
                        url: getUrl(userNotification)
                    };

                    if (truncateText || truncateText === undefined) {
                        formatted.text = abp.utils.truncateStringWithPostfix(formatted.text, 100);
                    }

                    return formatted;
                };

                var show = function (userNotification) {
                    abp.notifications.showUiNotifyForUserNotification(userNotification, {
                        'onclick': function () {
                            //Take action when user clicks to live toastr notification
                            var url = getUrl(userNotification);
                            if (url) {
                                location.href = url;
                            }
                        }
                    });
                };

                var setAllAsRead = function (callback) {
                    notificationService.setAllNotificationsAsRead().then(function () {
                        abp.event.trigger('app.notifications.refresh');
                        callback && callback();
                    });
                };

                var setAsRead = function (userNotificationId, callback) {
                    notificationService.setNotificationAsRead({
                        id: userNotificationId
                    }).then(function () {
                        abp.event.trigger('app.notifications.read', userNotificationId);
                        callback && callback(userNotificationId);
                    });
                };

                var openSettingsModal = function () {
                    $uibModal.open({
                        templateUrl: '~/App/common/views/notifications/settingsModal.cshtml',
                        controller: 'common.views.notifications.settingsModal as vm',
                        backdrop: 'static'
                    });
                };

                /* Expose public API *****************************************/

                return {
                    format: format,
                    show: show,
                    setAllAsRead: setAllAsRead,
                    setAsRead: setAsRead,
                    openSettingsModal: openSettingsModal
                };
            }
    ]);
})();