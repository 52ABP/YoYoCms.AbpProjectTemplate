var app = app || {};
(function ($) {

    app.UserNotificationHelper = (function () {

        return function () {

            /* Message Extracting based on Notification Data Type ********/

            //add your custom notification data types here...

            /* Example:
            abp.notifications.messageFormatters['MyCompanyName.AbpProjectTemplate.MyNotificationDataType'] = function(userNotification) {
                return ...; //format and return message here
            };
            */

            var _notificationService = abp.services.yoyocms.notification;

            /* Converter functions ***************************************/

            function getUrl(userNotification) {
                switch (userNotification.notification.notificationName) {
                    case 'App.NewUserRegistered':
                        return '/Mpa/users?filterText=' + userNotification.notification.data.properties.emailAddress;
                    case 'App.NewTenantRegistered':
                        return '/Mpa/tenants?filterText=' + userNotification.notification.data.properties.tenancyName;
                        //Add your custom notification names to navigate to a URL when user clicks to a notification.
                }

                //No url for this notification
                return null;
            };

            /* PUBLIC functions ******************************************/

            var format = function (userNotification, truncateText) {
                var formatted = {
                    userNotificationId: userNotification.id,
                    text: abp.notifications.getFormattedMessageFromUserNotification(userNotification),
                    time: moment(userNotification.notification.creationTime).format("YYYY-MM-DD HH:mm:ss"),
                    icon: app.notification.getUiIconBySeverity(userNotification.notification.severity),
                    state: abp.notifications.getUserNotificationStateAsString(userNotification.state),
                    data: userNotification.notification.data,
                    url: getUrl(userNotification),
                    isUnread: userNotification.state === abp.notifications.userNotificationState.UNREAD,
                    timeAgo: moment(userNotification.notification.creationTime).fromNow()
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
                _notificationService.setAllNotificationsAsRead().done(function () {
                    abp.event.trigger('app.notifications.refresh');
                    callback && callback();
                });
            };

            var setAsRead = function (userNotificationId, callback) {
                _notificationService.setNotificationAsRead({
                    id: userNotificationId
                }).done(function () {
                    abp.event.trigger('app.notifications.read', userNotificationId);
                    callback && callback(userNotificationId);
                });
            };

            var openSettingsModal = function () {
                new app.ModalManager({
                    viewUrl: abp.appPath + 'Mpa/Notifications/SettingsModal',
                    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Notifications/_SettingsModal.js',
                    modalClass: 'NotificationSettingsModal'
                }).open();
            };

            /* Expose public API *****************************************/

            return {
                format: format,
                show: show,
                setAllAsRead: setAllAsRead,
                setAsRead: setAsRead,
                openSettingsModal: openSettingsModal
            };

        };

    })();

})(jQuery);