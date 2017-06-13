(function () {
    app.modals.NotificationSettingsModal = function () {

        var _notificationAppService = abp.services.app.notification;

        var _modalManager;
        var _$form = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$form = _modalManager.getModal().find('form[name=NotificationSettingsForm]');
            _$form.validate();

            $('.make-switch').bootstrapSwitch().on('switchChange.bootstrapSwitch', function () {
                var receiveNotifications = $(this).attr("checked") === 'checked';
                var notificationCount = $("div.notification").length;

                if (notificationCount) {
                    $('.notification-types-header').removeClass('hidden');
                } else {
                    $('.notification-types-header').addClass('hidden');
                }

                if (notificationCount && !receiveNotifications) {
                    $('.disable-info').removeClass('hidden');
                } else {
                    $('.disable-info').addClass('hidden');
                }

                if (!receiveNotifications) {
                    $('div.notification input[type=checkbox]').attr('disabled', 'disabled');
                } else {
                    $('div.notification input[type=checkbox]').removeAttr('disabled');
                }
            });
        };

        function _findNotificationSubscriptions() {
            var notifications = [];
            $.each($('.notification input[type=checkbox]'), function (index, item) {
                notifications.push({
                    name: $(item).attr("id").replace('Notification_', ''),
                    isSubscribed: $(item).attr("checked") === 'checked'
                });
            });
            return notifications;
        }

        this.save = function () {
            if (!_$form.valid()) {
                return;
            }

            var notificationSettings = _$form.serializeFormToObject();
            notificationSettings.receiveNotifications = $('#NotificationSettigs_ReceiveNotifications').bootstrapSwitch('state');
            notificationSettings.notifications = _findNotificationSubscriptions();

            _modalManager.setBusy(true);
            _notificationAppService.updateNotificationSettings(notificationSettings)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})();