(function ($) {
    $(function () {

        var chatService = abp.services.app.chat;
        var friendshipService = abp.services.app.friendship;

        var appSession = {
            load: function (callback) {
                abp.services.app.session.getCurrentLoginInformations({ async: false })
                    .done(function (result) {
                        appSession.user = result.user;
                        appSession.tenant = result.tenant;

                        callback();
                    });
            }
        };

        var chat = {
            friends: [],
            tenantToTenantChatAllowed: abp.features.isEnabled('App.ChatFeature.TenantToTenant'),
            tenantToHostChatAllowed: abp.features.isEnabled('App.ChatFeature.TenantToHost'),
            serverClientTimeDifference: 0,
            selectedUser: null,
            isOpen: false,

            getFriendOrNull: function (userId, tenantId) {
                var friend = _.where(chat.friends, { friendUserId: parseInt(userId), friendTenantId: tenantId ? parseInt(tenantId) : null });
                if (friend.length) {
                    return friend[0];
                }

                return null;
            },

            getFixedMessageTime: function (messageTime) {
                return moment(messageTime).add(-1 * chat.serverClientTimeDifference, 'seconds').format('YYYY-MM-DDTHH:mm:ssZ');
            },

            getFriendsAndSettings: function (callBack) {
                chatService.getUserChatFriendsWithSettings().done(function (result) {
                    chat.friends = result.friends;
                    chat.serverClientTimeDifference = app.calculateTimeDifference(abp.clock.now(), result.serverTime, 'seconds');

                    chat.triggerUnreadMessageCountChangeEvent();
                    chat.renderFriendLists(chat.friends);
                    callBack();
                });
            },

            loadLastState: function () {
                app.localStorage.getItem('app.chat.isOpen', function (isOpen) {
                    chat.isOpen = isOpen;
                    chat.adjustNotifyPosition();

                    app.localStorage.getItem('app.chat.pinned', function (pinned) {
                        chat.pinned = pinned;
                        var $sidebarPinner = $('a.page-quick-sidebar-pinner');
                        $sidebarPinner.find('i.icon-pin').attr('class', 'icon-pin ' + (chat.pinned ? 'pinned' : 'unpinned'));
                    });

                    if (chat.isOpen) {
                        $('body').addClass('page-quick-sidebar-open').promise().done(function () {
                            app.localStorage.getItem('app.chat.selectedUser', function (user) {
                                if (user) {
                                    $('.page-quick-sidebar-chat').addClass('page-quick-sidebar-content-item-shown');
                                    chat.selectFriend(user.friendUserId, user.friendTenantId);
                                } else {
                                    $('.page-quick-sidebar-chat').removeClass('page-quick-sidebar-content-item-shown');
                                }
                            });
                        });
                    }
                });
            },

            changeChatPanelIsOpenOnLocalStorage: function () {
                app.localStorage.setItem('app.chat.isOpen', chat.isOpen);
            },

            changeChatUserOnLocalStorage: function () {
                app.localStorage.setItem('app.chat.selectedUser', chat.selectedUser);
            },

            changeChatPanelPinnedOnLocalStorage: function () {
                app.localStorage.setItem('app.chat.pinned', chat.pinned);
            },

            changeChatPanelPinned: function(pinned) {
                chat.pinned = pinned;
                var $sidebarPinner = $(".page-quick-sidebar-pinner");
                $sidebarPinner.find('i.icon-pin').attr('class', 'icon-pin ' + (chat.pinned ? 'pinned' : 'unpinned'));
                chat.changeChatPanelPinnedOnLocalStorage();
            },

            //Friends
            selectFriend: function (friendUserId, friendTenantId) {
                var chatUser = chat.getFriendOrNull(friendUserId, friendTenantId);
                chat.selectedUser = chatUser;
                chat.changeChatUserOnLocalStorage();
                chat.user.setSelectedUserOnlineStatus(chatUser.isOnline);

                if (chat.selectedUser.friendProfilePictureId) {
                    var tenantId = chat.selectedUser.friendTenantId ? chat.selectedUser.friendTenantId : '';
                    $('#selectedChatUserImage').attr('src', abp.appPath + 'Profile/GetFriendProfilePictureById?id=' + chat.selectedUser.friendProfilePictureId + '&userId=' + chat.selectedUser.friendUserId + '&tenantId=' + tenantId);
                } else {
                    $('#selectedChatUserImage').attr('src', abp.appPath + 'Common/Images/default-profile-picture.png');
                }

                $('#selectedChatUserName').text(chat.user.getShownUserName(chat.selectedUser.friendTenancyName, chat.selectedUser.friendUserName));
                $('#ChatMessage').val('');

                if (chat.selectedUser.state !== app.consts.friendshipState.blocked) {
                    $('#liBanChatUser, #ChatMessageWrapper').show();
                    $('#liUnbanChatUser, #UnblockUserButton').hide();
                    $('#ChatMessage').removeAttr("disabled");
                    $('#ChatMessage').focus();
                    $('#SendChatMessageButton').removeAttr("disabled");
                } else {
                    $('#liBanChatUser, #ChatMessageWrapper').hide();
                    $('#liUnbanChatUser, #UnblockUserButton').show();
                    $('#ChatMessage').attr("disabled", "disabled");
                    $('#SendChatMessageButton').attr("disabled", "disabled");
                }

                if (!chatUser.messagesLoaded) {
                    chat.user.loadMessages(chatUser, function () {
                        chatUser.messagesLoaded = true;
                        chat.scrollToBottom();
                    });
                } else {
                    var renderedMessages = chat.renderMessages(chatUser.messages);
                    $('#UserChatMessages').html(renderedMessages);
                    chat.scrollToBottom();
                    $(".timeago").timeago();

                    chat.user.markAllUnreadMessagesOfUserAsRead(chat.selectedUser);
                }
            },

            changeFriendState: function (user, state) {
                var friend = chat.getFriendOrNull(user.friendUserId, user.friendTenantId);
                if (!friend) {
                    return;
                }

                friend.state = state;
                chat.renderFriendLists(chat.friends);
            },

            getFormattedFriends: function (friends) {
                $.each(friends, function (index, friend) {
                    friend.profilePicturePath = chat.getFriendProfilePicturePath(friend);
                    friend.shownUserName = chat.user.getShownUserName(friend.friendTenancyName, friend.friendUserName);
                });
                return friends;
            },

            renderFriendList: function (friends, $element) {
                var template = $('#UserFriendTemplate').html();
                Mustache.parse(template);

                var rendered = Mustache.render(template, friends);
                $element.html(rendered);
            },

            renderFriendLists: function (friends) {
                friends = chat.getFormattedFriends(friends);

                var acceptedFriends = _.where(friends, { state: app.consts.friendshipState.accepted });
                chat.renderFriendList(acceptedFriends, $('#friendListFriends'));

                var blockedFriends = _.where(friends, { state: app.consts.friendshipState.blocked });
                chat.renderFriendList(blockedFriends, $('#friendListBlockeds'));

                if (acceptedFriends.length) {
                    $('#EmptyFriendListInfo').hide();
                } else {
                    $('#EmptyFriendListInfo').show();
                }

                if (blockedFriends.length) {
                    $('#EmptyBlockedFriendListInfo').hide();
                } else {
                    $('#EmptyBlockedFriendListInfo').show();
                }
            },

            getFriendProfilePicturePath: function (friend) {
                if (!friend.friendProfilePictureId) {
                    return abp.appPath + 'Common/Images/default-profile-picture.png';
                }

                var tenantId = friend.friendTenantId ? friend.friendTenantId : '';
                return abp.appPath + 'Profile/GetFriendProfilePictureById?id=' + friend.friendProfilePictureId + '&userId=' + friend.friendUserId + '&tenantId=' + tenantId;
            },

            //Messages
            sendMessage: function () {
                if (!$('form[name=\'chatMessageForm\']').valid() || chat.selectedUser.state === app.consts.friendshipState.blocked) {
                    return;
                }

                $('#SendChatMessageButton').attr('disabled', 'disabled');

                app.chat.sendMessage({
                    tenantId: chat.selectedUser.friendTenantId,
                    userId: chat.selectedUser.friendUserId,
                    message: $('#ChatMessage').val(),
                    tenancyName: appSession.tenant ? appSession.tenant.tenancyName : null,
                    userName: appSession.user.userName,
                    profilePictureId: appSession.user.profilePictureId
                }, function () {
                    $('#ChatMessage').val('');
                    $('#SendChatMessageButton').removeAttr('disabled');
                });
            },

            getFormattedMessages: function (messages) {
                $.each(messages, function (index, message) {
                    message.creationTime = chat.getFixedMessageTime(message.creationTime);
                    message.cssClass = message.side === app.chat.side.sender ? 'post out' : 'post in';
                    message.shownUserName = message.side === app.chat.side.sender
                        ? appSession.user.userName
                        : chat.selectedUser.friendUserName;

                    message.profilePicturePath = message.side === app.chat.side.sender ?
                        (!appSession.user.profilePictureId ? (abp.appPath + 'Common/Images/default-profile-picture.png') : (abp.appPath + 'Profile/GetProfilePictureById?id=' + appSession.user.profilePictureId)) :
                        chat.getFriendProfilePicturePath(chat.selectedUser);
                });

                return messages;
            },

            renderMessages: function (messages) {
                messages = chat.getFormattedMessages(messages);

                var template = $('#UserChatMessageTemplate').html();
                Mustache.parse(template);

                return Mustache.render(template, messages);
            },

            scrollToBottom: function () {
                var $scrollArea = $('.page-quick-sidebar-chat-user-messages');
                var scrollToVal = $scrollArea.prop('scrollHeight') + 'px';
                $scrollArea.slimScroll({ scrollTo: scrollToVal });
            },

            //Events & UI

            adjustNotifyPosition: function () {
                if (chat.isOpen) {
                    app.changeNotifyPosition('toast-chat-open');
                } else {
                    app.changeNotifyPosition('toast-bottom-right');
                }
            },

            triggerUnreadMessageCountChangeEvent: function () {
                var totalUnreadMessageCount = 0;
                if (chat && chat.friends) {
                    totalUnreadMessageCount = _.reduce(chat.friends, function (memo, friend) { return memo + friend.unreadMessageCount; }, 0);
                }

                abp.event.trigger('app.chat.unreadMessageCountChanged', totalUnreadMessageCount);
            },

            bindUiEvents: function () {
                QuickSidebar.init(function (e, pos) {
                    if (pos === 0 && !chat.selectedUser.allPreviousMessagesLoaded && !chat.user.loadingPreviousUserMessages) {
                        chat.user.loadMessages(chat.selectedUser);
                    }
                });

                $('.page-quick-sidebar-back-to-list').on('click', function () {
                    chat.selectedUser = null;
                    chat.changeChatUserOnLocalStorage();
                });

                $('.dropdown-quick-sidebar-toggler a, .page-quick-sidebar-toggler, .quick-sidebar-toggler').on('click', function () {
                    chat.isOpen = $('body').hasClass('page-quick-sidebar-open');
                    chat.adjustNotifyPosition();
                    chat.changeChatPanelIsOpenOnLocalStorage();
                    chat.user.markAllUnreadMessagesOfUserAsRead(chat.selectedUser);
                });

                $('.page-quick-sidebar-chat-users').on('click', '.media-list > .media', function () {
                    var friendUserId = $(this).attr('data-friend-user-id');
                    var friendTenantId = $(this).attr('data-friend-tenant-id');
                    chat.selectFriend(friendUserId, friendTenantId);
                });

                $('#liBanChatUser a').click(function () {
                    chat.user.block(chat.selectedUser);
                });
                $('#liUnbanChatUser, #UnblockUserButton').click(function () {
                    chat.user.unblock(chat.selectedUser);
                });

                $('#SearchChatUserButton').click(function () {
                    chat.user.search();
                });

                $('#ChatUserSearchUserName, #ChatUserSearchTenancyName').keypress(function (e) {
                    if (e.which === 13) {
                        chat.user.search();
                    }
                });

                $('#SendChatMessageButton').click(function () {
                    chat.sendMessage();
                });

                $('#ChatMessage').keypress(function (e) {
                    if (e.which === 13) {
                        e.preventDefault();
                        chat.sendMessage();
                    }
                });

                $('#ChatUserSearchUserName').on('keyup', function () {
                    var userName = $(this).val();
                    if (userName) {
                        $('#SearchChatUserButton').removeClass('hidden');
                    } else {
                        $('#SearchChatUserButton').addClass('hidden');
                    }

                    var friends = _.filter(chat.friends, function (friend) {
                        return chat.user.getShownUserName(friend.friendTenancyName, friend.friendUserName).toLowerCase().indexOf(userName.toLowerCase()) >= 0;
                    });

                    chat.renderFriendLists(friends);
                });

                $('div.page-quick-sidebar-wrapper').on('mouseleave', function () {
                    if (chat.pinned) {
                        return;
                    }

                    $('body').removeClass('page-quick-sidebar-open');
                    chat.isOpen = false;
                    chat.adjustNotifyPosition();
                    chat.changeChatPanelIsOpenOnLocalStorage();
                });

                $('form[name=\'chatMessageForm\']').validate({
                    invalidHandler: function () {
                        $('#SendChatMessageButton').attr('disabled', 'disabled');
                    },

                    success: function () {
                        $('#SendChatMessageButton').removeAttr('disabled');
                    }
                });

                $('.page-quick-sidebar-pinner').click(function () {
                    chat.changeChatPanelPinned(!chat.pinned);
                });

                if (!chat.tenantToTenantChatAllowed && chat.tenantToHostChatAllowed) {
                    $('#InterTenantChatHintIcon').hide();
                }
            },

            registerEvents: function () {

                abp.event.on('app.chat.messageReceived', function (message) {
                    var user = chat.getFriendOrNull(message.targetUserId, message.targetTenantId);

                    if (user) {
                        user.messages = user.messages || [];
                        user.messages.push(message);

                        if (message.side === app.chat.side.receiver) {
                            user.unreadMessageCount += 1;
                            message.readState = app.chat.readState.unread;
                            chat.user.changeUnreadMessageCount(user.friendTenantId, user.friendUserId, user.unreadMessageCount);
                            chat.triggerUnreadMessageCountChangeEvent();

                            if (chat.isOpen && chat.selectedUser !== null && user.friendTenantId === chat.selectedUser.friendTenantId && user.friendUserId === chat.selectedUser.friendUserId) {
                                chat.user.markAllUnreadMessagesOfUserAsRead(chat.selectedUser);
                            } else {
                                abp.notify.info(
                                    abp.utils.formatString('{0}: {1}', user.friendUserName, abp.utils.truncateString(message.message, 100)),
                                    null,
                                    {
                                        onclick: function () {
                                            if (!$('body').hasClass('page-quick-sidebar-open')) {
                                                $('body').addClass('page-quick-sidebar-open');
                                                chat.isOpen = true;
                                                chat.changeChatPanelIsOpenOnLocalStorage();
                                            }

                                            if (!$('.page-quick-sidebar-chat').hasClass('page-quick-sidebar-content-item-shown')) {
                                                $('.page-quick-sidebar-chat').addClass('page-quick-sidebar-content-item-shown');
                                            }

                                            chat.selectFriend(user.friendUserId, user.friendTenantId);
                                            chat.changeChatPanelPinned(true);
                                        }
                                    });
                            }
                        }

                        if (chat.selectedUser !== null && user.friendUserId === chat.selectedUser.friendUserId && user.friendTenantId === chat.selectedUser.friendTenantId) {
                            var renderedMessage = chat.renderMessages([message]);
                            $('#UserChatMessages').append(renderedMessage);
                            $(".timeago").timeago();
                        }

                        chat.scrollToBottom();
                    }
                });

                abp.event.on('app.chat.friendshipRequestReceived', function (data, isOwnRequest) {
                    if (!isOwnRequest) {
                        abp.notify.info(abp.utils.formatString(app.localize('UserSendYouAFriendshipRequest'), data.friendUserName));
                    }

                    if (!_.where(chat.friends, { userId: data.friendUserId, tenantId: data.friendTenantId }).length) {
                        chat.friends.push(data);
                        chat.renderFriendLists(chat.friends);
                    }
                });

                abp.event.on('app.chat.userConnectionStateChanged', function (data) {
                    chat.user.setFriendOnlineStatus(data.friend.userId, data.friend.tenantId, data.isConnected);
                });

                abp.event.on('app.chat.userStateChanged', function (data) {
                    var user = chat.getFriendOrNull(data.friend.userId, data.friend.tenantId);
                    if (!user) {
                        return;
                    }

                    user.state = data.state;
                    chat.renderFriendLists(chat.friends);
                });

                abp.event.on('app.chat.allUnreadMessagesOfUserRead', function (data) {
                    var user = chat.getFriendOrNull(data.friend.userId, data.friend.tenantId);
                    if (!user) {
                        return;
                    }

                    user.unreadMessageCount = 0;
                    chat.user.changeUnreadMessageCount(user.friendTenantId, user.friendUserId, user.unreadMessageCount);
                    chat.triggerUnreadMessageCountChangeEvent();
                });

                abp.event.on('app.chat.connected', function () {
                    chat.getFriendsAndSettings(function () {
                        chat.bindUiEvents();
                        chat.loadLastState();
                    });
                });
            },

            init: function () {
                chat.registerEvents();

                appSession.load(function () {
                    chat.interTenantChatAllowed = abp.features.isEnabled('App.ChatFeature.TenantToTenant') ||
                            abp.features.isEnabled('App.ChatFeature.TenantToHost') ||
                            !appSession.tenant;
                });
            },

            user: {
                loadingPreviousUserMessages: false,
                userNameFilter: '',//todo@ismail -> should we use this here ?

                getShownUserName: function (tenanycName, userName) {
                    return (tenanycName ? tenanycName : '.') + '\\' + userName;
                },

                block: function (user) {
                    friendshipService.blockUser({
                        userId: user.friendUserId,
                        tenantId: user.friendTenantId
                    }).done(function () {
                        chat.changeFriendState(user, app.consts.friendshipState.blocked);
                        abp.notify.info(app.localize('UserBlocked'));

                        $('#ChatMessage').attr("disabled", "disabled");
                        $('#SendChatMessageButton').attr("disabled", "disabled");
                        $('#liBanChatUser, #ChatMessageWrapper').hide();
                        $('#liUnbanChatUser, #UnblockUserButton').show();
                    });
                },

                unblock: function (user) {
                    friendshipService.unblockUser({
                        userId: user.friendUserId,
                        tenantId: user.friendTenantId
                    }).done(function () {
                        chat.changeFriendState(user, app.consts.friendshipState.accepted);
                        abp.notify.info(app.localize('UserUnblocked'));

                        $('#ChatMessage').removeAttr("disabled");
                        $('#ChatMessage').focus();
                        $('#SendChatMessageButton').removeAttr("disabled");
                        $('#liBanChatUser, #ChatMessageWrapper').show();
                        $('#liUnbanChatUser, #UnblockUserButton').hide();
                    });
                },

                markAllUnreadMessagesOfUserAsRead: function (user) {
                    if (!user || !chat.isOpen) {
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
                    }).done(function () {
                        $.each(user.messages,
                            function (index, message) {
                                if (unreadMessageIds.indexOf(message.id) >= 0) {
                                    message.readState = app.chat.readState.read;
                                }
                            });
                    });
                },

                changeUnreadMessageCount: function (tenantId, userId, messageCount) {
                    if (!tenantId) {
                        tenantId = '';
                    }
                    var $userItems = $('li.media[data-friend-tenant-id="' + tenantId + '"][data-friend-user-id="' + userId + '"]');
                    if ($userItems) {
                        var $item = $($userItems[0]).find('.media-status span');
                        $item.html(messageCount);

                        if (messageCount) {
                            $item.removeClass('hidden');
                        } else {
                            $item.addClass('hidden');
                        }
                    }
                },

                loadMessages: function (user, callback) {
                    chat.user.loadingPreviousUserMessages = true;

                    var minMessageId = null;
                    if (user.messages && user.messages.length) {
                        minMessageId = _.min(user.messages, function (message) { return message.id; }).id;
                    }

                    chatService.getUserChatMessages({
                        minMessageId: minMessageId,
                        tenantId: user.friendTenantId,
                        userId: user.friendUserId
                    }).done(function (result) {
                        if (!user.messages) {
                            user.messages = [];
                        }

                        user.messages = result.items.concat(user.messages);

                        chat.user.markAllUnreadMessagesOfUserAsRead(user);

                        if (!result.items.length) {
                            user.allPreviousMessagesLoaded = true;
                        }

                        var renderedMessages = chat.renderMessages(user.messages);
                        $('#UserChatMessages').html(renderedMessages);

                        $(".timeago").timeago();

                        chat.user.loadingPreviousUserMessages = false;

                        if (callback) {
                            callback();
                        }
                    });
                },

                openSearchModal: function (tenantId) {
                    var lookupModal = app.modals.LookupModal.create({
                        title: app.localize('SelectAUser'),
                        serviceMethod: abp.services.app.commonLookup.findUsers,
                        filterText: $('#ChatUserSearchUserName').val(),
                        extraFilters: { tenantId: tenantId }
                    });

                    lookupModal.open({}, function (selectedItem) {
                        var userId = selectedItem.value;
                        friendshipService.createFriendshipRequest({
                            userId: userId,
                            tenantId: appSession.tenant !== null ? appSession.tenant.id : null
                        }).done(function () {
                            $('#ChatUserSearchUserName').val('');
                        });
                    });
                },

                search: function () {
                    var userNameValue = $('#ChatUserSearchUserName').val();

                    var tenancyName = '';
                    var userName = '';

                    if (userNameValue.indexOf('\\') === -1) {
                        userName = userNameValue;
                    } else {
                        var tenancyAndUserNames = userNameValue.split('\\');
                        tenancyName = tenancyAndUserNames[0];
                        userName = tenancyAndUserNames[1];
                    }

                    if (!tenancyName || !chat.interTenantChatAllowed) {
                        chat.user.openSearchModal(appSession.tenant ? appSession.tenant.id : null, userName);
                    } else {
                        friendshipService.createFriendshipRequestByUserName({
                            tenancyName: tenancyName,
                            userName: userName
                        }).done(function () {
                            $('#ChatUserSearchUserName').val('');
                        });
                    }
                },

                setFriendOnlineStatus: function (userId, tenantId, isOnline) {
                    var user = chat.getFriendOrNull(userId, tenantId);
                    if (!user) {
                        return;
                    }

                    user.isOnline = isOnline;

                    var statusClass = 'contact-status ' + (isOnline ? 'online' : 'offline');
                    var $userItems = $('li.media[data-friend-tenant-id="' + (tenantId ? tenantId : '') + '"][data-friend-user-id="' + userId + '"]');
                    if ($userItems) {
                        $($userItems[0]).find('.contact-status').attr('class', statusClass);
                    }

                    if (chat.selectedUser &&
                        tenantId === chat.selectedUser.friendTenantId &&
                        userId === chat.selectedUser.friendUserId) {

                        chat.user.setSelectedUserOnlineStatus(isOnline);
                    }
                },

                setSelectedUserOnlineStatus: function (isOnline) {

                    if (chat.selectedUser) {
                        var statusClass = 'contact-status ' + (isOnline ? 'online' : 'offline');
                        $('#selectedChatUserStatus').attr('class', statusClass);
                    }
                }
            }
        };

        chat.init();
    });
})(jQuery);