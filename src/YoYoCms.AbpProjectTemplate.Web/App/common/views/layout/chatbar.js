(function () {
    appModule.controller('common.views.layout.chatbar', [
        '$rootScope', '$scope', '$uibModal', '$timeout', '$interval', 'appSession', 'lookupModal', 'abp.services.app.commonLookup', 'abp.services.app.chat', 'abp.services.app.friendship',
        function ($rootScope, $scope, $uibModal, $timeout, $interval, appSession, lookupModal, commonLookup, chatService, friendshipService) {
            var vm = this;

            vm.consts = {
                appPath: abp.appPath,
                friendshipState: app.consts.friendshipState
            };

            vm.chat = {

                friends: [],
                tenantToTenantChatAllowed: abp.features.isEnabled('App.ChatFeature.TenantToTenant'),
                tenantToHostChatAllowed: abp.features.isEnabled('App.ChatFeature.TenantToHost'),
                interTenantChatAllowed: abp.features.isEnabled('App.ChatFeature.TenantToTenant') || abp.features.isEnabled('App.ChatFeature.TenantToHost') || !appSession.tenant,
                serverClientTimeDifference: 0,
                selectedUser: null,
                currentUser: appSession.user,
                isOpen: false,
                pinned: false,
                message: '',
                sendingMessage: false,

                user: {

                    loadingPreviousUserMessages: false,
                    userNameFilter: '',

                    getShownUserName: function (tenanycName, userName) {
                        return (tenanycName ? tenanycName : '.') + '\\' + userName;
                    },

                    block: function (user) {
                        friendshipService.blockUser({
                            userId: user.friendUserId,
                            tenantId: user.friendTenantId
                        }).then(function () {
                            abp.notify.info(app.localize('UserBlocked'));
                        });
                    },

                    unblock: function (user) {
                        friendshipService.unblockUser({
                            userId: user.friendUserId,
                            tenantId: user.friendTenantId
                        }).then(function () {
                            abp.notify.info(app.localize('UserUnblocked'));
                        });
                    },

                    markAllUnreadMessagesOfUserAsRead: function (user) {
                        if (!user || !vm.chat.isOpen) {
                            return;
                        }

                        var unreadMessages = _.where(user.messages, { readState: app.chat.readState.unread });
                        var unreadMessageIds = _.pluck(unreadMessages, 'id');

                        if (!unreadMessageIds.length) {
                            return;
                        }

                        chatService.markAllUnreadMessagesOfUserAsRead({
                            tenantId: user.friendTenantId,
                            userId: user.friendUserId
                        }).then(function () {
                            angular.forEach(user.messages,
                                function (message) {
                                    if (unreadMessageIds.indexOf(message.id) >= 0) {
                                        message.readState = app.chat.readState.read;
                                    }
                                });
                        });
                    },

                    loadMessages: function (user, callback) {
                        vm.chat.user.loadingPreviousUserMessages = true;

                        var minMessageId = null;
                        if (user.messages && user.messages.length) {
                            minMessageId = _.min(user.messages, function (message) { return message.id; }).id;
                        }

                        chatService.getUserChatMessages({
                            minMessageId: minMessageId,
                            tenantId: user.friendTenantId,
                            userId: user.friendUserId
                        }).then(function (result) {
                            if (!user.messages) {
                                user.messages = [];
                            }

                            user.messages = result.data.items.concat(user.messages);
                            vm.chat.user.markAllUnreadMessagesOfUserAsRead(user);

                            if (!result.data.items.length) {
                                user.allPreviousMessagesLoaded = true;
                            }

                            vm.chat.user.loadingPreviousUserMessages = false;
                            if (callback) {
                                callback();
                            }
                        });
                    },

                    openSearchModal: function (tenantId, userName) {
                        lookupModal.open({
                            title: app.localize('SelectAUser'),
                            serviceMethod: commonLookup.findUsers,
                            filterText: userName,
                            extraFilters: {
                                tenantId: tenantId
                            },
                            callback: function (selectedItem) {
                                var userId = selectedItem.value;
                                friendshipService.createFriendshipRequest({
                                    userId: userId,
                                    tenantId: appSession.tenant !== null ? appSession.tenant.id : null
                                }).then(function () {
                                    vm.chat.user.userNameFilter = '';
                                });
                            }
                        });
                    },

                    search: function () {
                        var tenancyName = '';
                        var userName = '';

                        if (vm.chat.user.userNameFilter.indexOf('\\') === -1) {
                            userName = vm.chat.user.userNameFilter;
                        } else {
                            var tenancyAndUserNames = vm.chat.user.userNameFilter.split('\\');
                            tenancyName = tenancyAndUserNames[0];
                            userName = tenancyAndUserNames[1];
                        }

                        if (!tenancyName || !vm.chat.interTenantChatAllowed) {
                            vm.chat.user.openSearchModal(appSession.tenant ? appSession.tenant.id : null, userName);
                        } else {
                            friendshipService.createFriendshipRequestByUserName({
                                tenancyName: tenancyName,
                                userName: userName
                            }).then(function () {
                                vm.chat.user.userNameFilter = '';
                            });
                        }
                    }
                },

                getFriendOrNull: function (userId, tenantId) {
                    var friend = _.where(vm.chat.friends, { friendUserId: userId, friendTenantId: tenantId });
                    if (friend.length) {
                        return friend[0];
                    }

                    return null;
                },

                getFilteredFriends: function (state, userNameFilter) {
                    return _.filter(vm.chat.friends, function (friend) {
                        return friend.state === state &&
                                vm.chat.user.getShownUserName(friend.friendTenancyName, friend.friendUserName)
                                    .toLocaleLowerCase()
                                    .indexOf(userNameFilter.toLocaleLowerCase()) >= 0;
                    });
                },

                getUserNameByChatSide: function (chatSide) {
                    return chatSide === app.chat.side.sender ?
                       vm.chat.currentUser.userName :
                       vm.chat.selectedUser.friendUserName;
                },

                getFixedMessageTime: function (messageTime) {
                    return moment(messageTime).add(-1 * vm.chat.serverClientTimeDifference, 'seconds').format('YYYY-MM-DDTHH:mm:ssZ');
                },

                getFriendsAndSettings: function (callback) {
                    chatService.getUserChatFriendsWithSettings().then(function (result) {
                        vm.chat.friends = result.data.friends;
                        vm.chat.serverClientTimeDifference = app.calculateTimeDifference(abp.clock.now(), result.serverTime, 'seconds');

                        vm.chat.triggerUnreadMessageCountChangeEvent();
                        callback();
                    });
                },

                scrollToBottom: function () {
                    $timeout(function () {
                        var $scrollArea = $('.page-quick-sidebar-chat-user-messages');
                        var scrollToVal = $scrollArea.prop('scrollHeight') + 'px';
                        $scrollArea.slimScroll({ scrollTo: scrollToVal });
                    }, 100);
                },

                loadLastState: function () {
                    app.localStorage.getItem('app.chat.isOpen', function (isOpen) {
                        vm.chat.isOpen = isOpen;

                        app.localStorage.getItem('app.chat.pinned',
                            function (pinned) {
                                vm.chat.pinned = pinned;
                            });

                        if (isOpen) {
                            $('body').addClass('page-quick-sidebar-open').promise().done(function () {
                                app.localStorage.getItem('app.chat.selectedUser', function (selectedUser) {
                                    if (selectedUser) {
                                        $('.page-quick-sidebar-chat').addClass('page-quick-sidebar-content-item-shown');
                                        vm.chat.selectFriend(selectedUser);
                                    } else {
                                        $('.page-quick-sidebar-chat').removeClass('page-quick-sidebar-content-item-shown');
                                    }
                                });
                            });
                        }
                    });
                },

                selectFriend: function (friend) {
                    var chatUser = vm.chat.getFriendOrNull(friend.friendUserId, friend.friendTenantId);
                    vm.chat.selectedUser = chatUser;
                    if (!chatUser) {
                        return;
                    }

                    vm.chat.message = '';

                    if (!chatUser.messagesLoaded) {
                        vm.chat.user.loadMessages(chatUser, function () {
                            chatUser.messagesLoaded = true;
                            vm.chat.scrollToBottom();
                        });
                    } else {
                        vm.chat.user.markAllUnreadMessagesOfUserAsRead(vm.chat.selectedUser);
                        vm.chat.scrollToBottom();
                    }
                },

                sendMessage: function () {
                    if (!vm.chat.message) {
                        return;
                    }

                    vm.chat.sendingMessage = true;
                    app.chat.sendMessage({
                        tenantId: vm.chat.selectedUser.friendTenantId,
                        userId: vm.chat.selectedUser.friendUserId,
                        message: vm.chat.message,
                        tenancyName: appSession.tenant ? appSession.tenant.tenancyName : null,
                        userName: appSession.user.userName,
                        profilePictureId: appSession.user.profilePictureId
                    }, function () {
                        $scope.safeApply(function () {
                            vm.chat.message = '';
                            vm.chat.sendingMessage = false;
                        });
                    });
                },

                reversePinned: function () {
                    vm.chat.pinned = !vm.chat.pinned;
                },

                bindUiEvents: function () {
                    $timeout(function () {
                        QuickSidebar.init(function (e, pos) {
                            if (pos === 0 && !vm.chat.selectedUser.allPreviousMessagesLoaded && !vm.chat.user.loadingPreviousUserMessages) {
                                vm.chat.user.loadMessages(vm.chat.selectedUser);
                            }
                        });

                    }, 1000);

                    var $backToList = $('.page-quick-sidebar-back-to-list');
                    $backToList.on('click', function () {
                        $scope.$apply(function () {
                            vm.chat.selectedUser = null;
                        });
                    });

                    var $sidebarTogglers = $('.dropdown-quick-sidebar-toggler a, .page-quick-sidebar-toggler, .quick-sidebar-toggler');
                    $sidebarTogglers.on('click', function () {
                        $scope.$apply(function () {
                            vm.chat.isOpen = !$('body').hasClass('page-quick-sidebar-open');
                        });
                    });

                    //Close chat panel when mouse moved outside of it, if not pinned
                    $('div.page-quick-sidebar-wrapper').on('mouseleave', function () {
                        if (vm.chat.pinned) {
                            return;
                        }

                        $('body').removeClass('page-quick-sidebar-open');
                        $scope.$apply(function () {
                            vm.chat.isOpen = false;
                        });
                    });
                },

                adjustNotifyPosition: function () {
                    if (vm.chat.isOpen) {
                        app.changeNotifyPosition('toast-chat-open');
                    } else {
                        app.changeNotifyPosition('toast-bottom-right');
                    }
                },

                triggerUnreadMessageCountChangeEvent: function () {
                    var totalUnreadMessageCount = 0;

                    if (vm.chat && vm.chat.friends) {
                        totalUnreadMessageCount = _.reduce(vm.chat.friends,
                            function (memo, friend) {
                                return memo + friend.unreadMessageCount;
                            }, 0);
                    }

                    abp.event.trigger('app.chat.unreadMessageCountChanged', totalUnreadMessageCount);
                },

                registerWatches: function () {
                    $scope.$watch('vm.chat.selectedUser',
                        function (newValue, oldValue) {
                            if (newValue === oldValue) {
                                return;
                            }
                            app.localStorage.setItem('app.chat.selectedUser', newValue);
                        });

                    $scope.$watch('vm.chat.pinned',
                        function (newValue, oldValue) {
                            if (newValue === oldValue) {
                                return;
                            }
                            app.localStorage.setItem('app.chat.pinned', newValue);
                        });

                    $scope.$watch('vm.chat.isOpen',
                        function (newValue, oldValue) {
                            if (newValue === oldValue) {
                                return;
                            }

                            vm.chat.adjustNotifyPosition();

                            if (newValue) {
                                vm.chat.user.markAllUnreadMessagesOfUserAsRead(vm.chat.selectedUser);
                            }

                            app.localStorage.setItem('app.chat.isOpen', newValue);
                        });
                },

                registerEvents: function () {
                    abp.event.on('app.chat.messageReceived', function (message) {
                        var user = vm.chat.getFriendOrNull(message.targetUserId, message.targetTenantId);
                        if (!user) {
                            return;
                        }

                        user.messages = user.messages || [];
                        $scope.$apply(function () {
                            user.messages.push(message);

                            if (message.side === app.chat.side.receiver) {
                                user.unreadMessageCount += 1;
                                message.readState = app.chat.readState.unread;
                                vm.chat.triggerUnreadMessageCountChangeEvent();

                                if (vm.chat.isOpen && vm.chat.selectedUser !== null && user.friendTenantId === vm.chat.selectedUser.friendTenantId && user.friendUserId === vm.chat.selectedUser.friendUserId) {
                                    vm.chat.user.markAllUnreadMessagesOfUserAsRead(user);
                                } else {
                                    abp.notify.info(
                                        abp.utils.formatString('{0}: {1}', user.friendUserName, abp.utils.truncateString(message.message, 100)),
                                        null,
                                        {
                                            onclick: function () {
                                                if (!$('body').hasClass('page-quick-sidebar-open')) {
                                                    $('body').addClass('page-quick-sidebar-open');
                                                    $scope.$apply(function () {
                                                        vm.chat.isOpen = true;
                                                    });
                                                }

                                                if (!$('.page-quick-sidebar-chat').hasClass('page-quick-sidebar-content-item-shown')) {
                                                    $('.page-quick-sidebar-chat').addClass('page-quick-sidebar-content-item-shown');
                                                }

                                                vm.chat.selectFriend(user);
                                                $scope.$apply(function () {
                                                    vm.chat.pinned = true;
                                                });
                                            }
                                        });
                                }
                            }

                            vm.chat.scrollToBottom();
                        });
                    });

                    abp.event.on('app.chat.friendshipRequestReceived', function (data, isOwnRequest) {
                        if (!isOwnRequest) {
                            abp.notify.info(abp.utils.formatString(app.localize('UserSendYouAFriendshipRequest'), data.friendUserName));
                        }

                        if (!_.where(vm.chat.friends, { userId: data.friendUserId, tenantId: data.friendTenantId }).length) {
                            $scope.$apply(function () {
                                vm.chat.friends.push(data);
                            });
                        }
                    });

                    abp.event.on('app.chat.userConnectionStateChanged', function (data) {
                        var user = vm.chat.getFriendOrNull(data.friend.userId, data.friend.tenantId);
                        if (!user) {
                            return;
                        }

                        $scope.$apply(function () {
                            user.isOnline = data.isConnected;
                        });
                    });

                    abp.event.on('app.chat.userStateChanged', function (data) {
                        var user = vm.chat.getFriendOrNull(data.friend.userId, data.friend.tenantId);
                        if (!user) {
                            return;
                        }

                        $scope.$apply(function () {
                            user.state = data.state;
                        });
                    });

                    abp.event.on('app.chat.allUnreadMessagesOfUserRead', function (data) {
                        var user = vm.chat.getFriendOrNull(data.friend.userId, data.friend.tenantId);
                        if (!user) {
                            return;
                        }

                        $scope.$apply(function () {
                            user.unreadMessageCount = 0;
                            vm.chat.triggerUnreadMessageCountChangeEvent();
                        });
                    });

                    abp.event.on('app.chat.connected', function () {
                        vm.chat.getFriendsAndSettings(function () {
                            app.waitUntilElementIsReady('.page-quick-sidebar-wrapper, .quick-sidebar-toggler', function () {
                                vm.chat.bindUiEvents();

                                app.waitUntilElementIsReady('.page-quick-sidebar-chat', function () {
                                    vm.chat.loadLastState();
                                });
                            });
                        });
                    });
                },

                init: function () {
                    vm.chat.registerWatches();
                    vm.chat.registerEvents();
                }
            };

            vm.chat.init();
        }
    ]);
})();