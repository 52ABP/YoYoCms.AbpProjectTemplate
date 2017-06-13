(function () {
    $(function () {

        var _$usersTable = $('#UsersTable');
        var _userService = abp.services.app.user;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Users.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Users.Edit'),
            changePermissions: abp.auth.hasPermission('Pages.Administration.Users.ChangePermissions'),
            impersonation: abp.auth.hasPermission('Pages.Administration.Users.Impersonation'),
            'delete': abp.auth.hasPermission('Pages.Administration.Users.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/CreateOrEditModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Users/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Users/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$usersTable.jtable({

            title: app.localize('Users'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _userService.getUsers
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '15%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        if (_permissions.impersonation && data.record.id != abp.session.userId) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('LoginAsThisUser') + '"><i class="fa fa-sign-in"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    app.utils.removeCookie(abp.security.antiForgery.tokenCookieName);
                                    abp.ajax({
                                        url: abp.appPath + 'Account/Impersonate',
                                        data: JSON.stringify({
                                            tenantId: abp.session.tenantId,
                                            userId: data.record.id
                                        })
                                    });
                                });
                        }

                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.changePermissions) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Permissions') + '"><i class="fa fa-list"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _userPermissionsModal.open({ id: data.record.id });
                                });
                        }

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Unlock') + '"><i class="fa fa-unlock"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                _userService.unlockUser({
                                    id: data.record.id
                                }).done(function() {
                                    abp.notify.success(app.localize('UnlockedTheUser', data.record.userName));
                                });
                            });

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-o"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteUser(data.record);
                                });
                        }

                        return $span;
                    }
                },
                userName: {
                    title: app.localize('UserName'),
                    width: '9%'
                },
                name: {
                    title: app.localize('Name'),
                    width: '10%'
                },
                surname: {
                    title: app.localize('Surname'),
                    width: '10%'
                },
                roles: {
                    title: app.localize('Roles'),
                    width: '12%',
                    sorting: false,
                    display: function (data) {
                        var roleNames = '';

                        for (var j = 0; j < data.record.roles.length; j++) {
                            if (roleNames.length) {
                                roleNames = roleNames + ', ';
                            }

                            roleNames = roleNames + data.record.roles[j].roleName;
                        };

                        return roleNames;
                    }
                },
                emailAddress: {
                    title: app.localize('EmailAddress'),
                    width: '15%'
                },
                isEmailConfirmed: {
                    title: app.localize('EmailConfirm'),
                    width: '6%',
                    display: function (data) {
                        if (data.record.isEmailConfirmed) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                isActive: {
                    title: app.localize('Active'),
                    width: '6%',
                    display: function (data) {
                        if (data.record.isActive) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                lastLoginTime: {
                    title: app.localize('LastLoginTime'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.lastLoginTime) {
                            return moment(data.record.lastLoginTime).format('L');
                        }

                        return '-';
                    }
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '7%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }

        });

        function getUsers(reload) {
            if (reload) {
                _$usersTable.jtable('reload');
            } else {
                _$usersTable.jtable('load', {
                    filter: $('#UsersTableFilter').val(),
                    permission: $("#PermissionSelectionCombo").val(),
                    role: $("#RoleSelectionCombo").val()
                });
            }
        }

        function deleteUser(user) {
            if (user.userName == app.consts.userManagement.defaultAdminUserName) {
                abp.message.warn(app.localize("{0}UserCannotBeDeleted", app.consts.userManagement.defaultAdminUserName));
                return;
            }

            abp.message.confirm(
                app.localize('UserDeleteWarningMessage', user.userName),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userService.deleteUser({
                            id: user.id
                        }).done(function () {
                            getUsers(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewUserButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportUsersToExcelButton').click(function () {
            _userService
                .getUsersToExcel({})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetUsersButton, #RefreshUserListButton').click(function (e) {
            e.preventDefault();
            getUsers();
        });

        abp.event.on('app.createOrEditUserModalSaved', function () {
            getUsers(true);
        });

        getUsers();

        $('#UsersTableFilter').focus();
    });
})();