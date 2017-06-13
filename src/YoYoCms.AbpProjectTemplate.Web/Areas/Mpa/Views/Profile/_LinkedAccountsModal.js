(function ($) {
    app.modals.LinkedAccountsModal = function () {

        var _modalManager;
        var _$linkedAccountsTable = $('#LinkedAccountsTable');
        var _userLinkService = abp.services.app.userLink;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var _linkNewAccountModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Profile/LinkAccountModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Profile/_LinkAccountModal.js',
            modalClass: 'LinkAccountModal'
        });

        $('#LinkNewAccountButton').click(function () {
            _linkNewAccountModal.open({}, function() {
                getLinkedUsers();
            });
        });

        _$linkedAccountsTable.jtable({

            paging: true,

            actions: {
                listAction: {
                    method: _userLinkService.getLinkedUsers
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
                    display: function (data) {
                        var $div = $('<div></div>');

                        $('<button class="btn btn-xs btn-primary blue"><i class="icon-login"></i>' + app.localize('LogIn') + '</button>')
                            .appendTo($div)
                            .click(function () {
                                switchToUser(data.record);
                            });

                        return $div;

                    }
                },
                userName: {
                    title: app.localize('UserName'),
                    width: '70%',
                    display: function (data) {
                        var $div = $('<div></div>');
                        $('<span>' + app.getShownLinkedUserName(data.record) + '</span>').appendTo($div);
                        return $div;
                    }
                },
                unlink: {
                    title: app.localize('Delete'),
                    width: '10%',
                    display: function (data) {
                        var $div = $('<span></span>');
                        $('<button class="btn btn-xs btn-danger red"><i class="icon-trash"></i></button>')
                            .appendTo($div)
                            .click(function () {
                                deleteLinkedUser(data.record);
                            });

                        return $div;
                    }
                }
            }

        });

        function switchToUser(linkedUser) {
            abp.ajax({
                url: abp.appPath + 'Account/SwitchToLinkedAccount',
                data: JSON.stringify({
                    targetUserId: linkedUser.id,
                    targetTenantId: linkedUser.tenantId
                }),
                success: function () {
                    app.utils.removeCookie(abp.security.antiForgery.tokenCookieName);
                }
            });
        }

        function deleteLinkedUser(linkedUser) {
            abp.message.confirm(
               app.localize('LinkedUserDeleteWarningMessage', linkedUser.username),
               function (isConfirmed) {
                   if (isConfirmed) {
                       _userLinkService.unlinkUser({
                           userId: linkedUser.id,
                           tenantId: linkedUser.tenantId
                       }).done(function () {
                           getLinkedUsers();
                           abp.notify.success(app.localize('SuccessfullyUnlinked'));
                       });
                   }
               }
           );
        }

        function getLinkedUsers() {
            _$linkedAccountsTable.jtable('load');
        }

        getLinkedUsers();
    };
})(jQuery);