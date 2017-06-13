var app = app || {};
(function ($) {

    //Check if SignalR is defined
    if (!$ || !$.connection) {
        return;
    }

    //Create namespaces
    app.signalr = app.signalr || {};
    app.signalr.hubs = app.signalr.hubs || {};

    //Get the chat hub
    app.signalr.hubs.chat = app.signalr.hubs.chat || $.connection.chatHub;

    var chatHub = app.signalr.hubs.chat;
    if (!chatHub) {
        return;
    }

    $.connection.hub.stateChanged(function (data) {
        if (data.newState === $.connection.connectionState.connected) {
            abp.event.trigger('app.chat.connected');
        }
    });

    //$.connection.hub.reconnected(function() {
    //    abp.event.trigger('app.chat.reconnected');
    //});

    chatHub.client.getChatMessage = function (message) {
        abp.event.trigger('app.chat.messageReceived', message);
    };

    chatHub.client.getAllFriends = function (friends) {
        abp.event.trigger('abp.chat.friendListChanged', friends);
    };

    chatHub.client.getFriendshipRequest = function (friendData, isOwnRequest) {
        abp.event.trigger('app.chat.friendshipRequestReceived', friendData, isOwnRequest);
    };

    chatHub.client.getUserConnectNotification = function (friend, isConnected) {
        abp.event.trigger('app.chat.userConnectionStateChanged',
        {
            friend: friend,
            isConnected: isConnected
        });
    };

    chatHub.client.getUserStateChange = function (friend, state) {
        abp.event.trigger('app.chat.userStateChanged',
        {
            friend: friend,
            state: state
        });
    };

    chatHub.client.getallUnreadMessagesOfUserRead = function (friend) {
        abp.event.trigger('app.chat.allUnreadMessagesOfUserRead',
        {
            friend: friend
        });
    };

    app.chat.sendMessage = function (messageData, callback) {
        if ($.connection.hub.state !== $.signalR.connectionState.connected) {
            callback && callback();
            abp.notify.warn(app.localize('ChatIsNotConnectedWarning'));
            return;
        }

        chatHub.server.sendMessage(messageData).done(function (result) {
            if (result) {
                abp.notify.warn(result);
            }
        }).always(function () {
            callback && callback();
        });
    };

})(jQuery);