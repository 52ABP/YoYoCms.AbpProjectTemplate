(function () {
    $(function () {

        var _$notificationsTable = $('#NotificationsTable');
        var _notificationService = abp.services.app.notification;

        var _$targetValueFilterSelectionCombobox = $('#TargetValueFilterSelectionCombobox');
        _$targetValueFilterSelectionCombobox.selectpicker();

        var _appUserNotificationHelper = new app.UserNotificationHelper();

        _$notificationsTable.jtable({

            title: app.localize('Notifications'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _notificationService.getUserNotifications
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    listClass: 'text-center',
                    display: function (data) {
                        var $span = $('<span></span>');

                        var $button = $('<button class="btn btn-xs btn-primary blue" title="' + app.localize('SetAsRead') + '"></button>').click(function (e) {
                            e.preventDefault();
                            setNotificationAsRead(data.record, function () {
                                $button.find('i')
                                    .removeClass('fa-circle-o')
                                    .addClass('fa-check');
                                $button.attr('disabled', 'disabled');

                            });
                        }).appendTo($span);

                        var $i = $('<i class="fa" >').appendTo($button);

                        var notificationState = _appUserNotificationHelper.format(data.record).state;

                        if (notificationState === 'READ') {
                            $button.attr('disabled', 'disabled');
                            $i.addClass('fa-check');
                        }

                        if (notificationState === 'UNREAD') {
                            $i.addClass('fa-circle-o');
                        }

                        return $span;
                    }
                },
                notification: {
                    title: app.localize('Notification'),
                    width: '70%',
                    display: function (data) {
                        var formattedRecord = _appUserNotificationHelper.format(data.record);
                        var rowClass = getRowClass(formattedRecord);

                        if (formattedRecord.url) {
                            return $('<a href="' + formattedRecord.url + '" class="' + rowClass + '">' + formattedRecord.text + '</a>');
                        } else {
                            return $('<span title="' + formattedRecord.text + '" class="' + rowClass + '">' + formattedRecord.text + '</span>');
                        }
                    }
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '20%',
                    display: function (data) {
                        var formattedRecord = _appUserNotificationHelper.format(data.record);
                        var rowClass = getRowClass(formattedRecord);
                        return $('<span title="' + moment(data.record.notification.creationTime).format("llll") + '" class="' + rowClass + '">' + formattedRecord.timeAgo + '</span> &nbsp;');
                    }
                }
            }

        });

        function getRowClass(formattedRecord) {
            return formattedRecord.state === 'READ' ? 'notification-read' : '';
        }

        function getNotifications() {
            _$notificationsTable.jtable('load', {
                state: _$targetValueFilterSelectionCombobox.val()
            });
        }

        function setNotificationAsRead(userNotification, callback) {
            _appUserNotificationHelper.setAsRead(userNotification.id, function () {
                if (callback) {
                    callback();
                }
            });
        }

        function setAllNotificationsAsRead() {
            _appUserNotificationHelper.setAllAsRead(function () {
                getNotifications();
            });
        };

        function openNotificationSettingsModal() {
            _appUserNotificationHelper.openSettingsModal();
        };

        _$targetValueFilterSelectionCombobox.change(function () {
            getNotifications();
        });

        $('#RefreshNotificationTableButton').click(function (e) {
            e.preventDefault();
            getNotifications();
        });

        $('#btnOpenNotificationSettingsModal').click(function (e) {
            openNotificationSettingsModal();
        });

        $('#btnSetAllNotificationsAsRead').click(function (e) {
            e.preventDefault();
            setAllNotificationsAsRead();
        });

        getNotifications();
    });
})();