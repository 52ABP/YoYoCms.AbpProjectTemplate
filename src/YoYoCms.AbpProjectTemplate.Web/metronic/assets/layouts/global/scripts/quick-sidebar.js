/**
ATTENTION:: This file is changed and cannot be replaced with original Metronic version anymore.
**/
var QuickSidebar = function () {

    // Handles quick sidebar toggler
    var handleQuickSidebarToggler = function () {
        // quick sidebar toggler
        var $toggler = $('.dropdown-quick-sidebar-toggler a, .page-quick-sidebar-toggler, .quick-sidebar-toggler');
        //$toggler.unbind('click');
        $toggler.on('click', function () {
            $('body').toggleClass('page-quick-sidebar-open');
        });
    };

    // Handles quick sidebar chats
    var handleQuickSidebarChat = function (scrollEvent) {
        var wrapper = $('.page-quick-sidebar-wrapper');
        var wrapperChat = wrapper.find('.page-quick-sidebar-chat');

        var initSlimScroll = function ($item, scrollEvent) {
            if ($item.attr("data-initialized")) {
                return; // exit
            }

            var height;

            if ($item.attr("data-height")) {
                height = $item.attr("data-height");
            } else {
                height = $item.css('height');
            }

            var $slimScroll = $item.slimScroll({
                allowPageScroll: true, // allow page scroll when the element scroll is ended
                size: '7px',
                color: ($item.attr("data-handle-color") ? $item.attr("data-handle-color") : '#bbb'),
                wrapperClass: ($item.attr("data-wrapper-class") ? $item.attr("data-wrapper-class") : 'slimScrollDiv'),
                railColor: ($item.attr("data-rail-color") ? $item.attr("data-rail-color") : '#eaeaea'),
                position: App.isRTL() ? 'left' : 'right',
                height: height,
                alwaysVisible: ($item.attr("data-always-visible") == "1" ? true : false),
                railVisible: ($item.attr("data-rail-visible") == "1" ? true : false),
                disableFadeOut: true
            });

            $item.attr("data-initialized", "1");

            if (scrollEvent) {
                $slimScroll.bind('slimscrolling', scrollEvent);
            }
        };

        var initChatSlimScroll = function (scrollEvent) {
            var chatUsers = wrapper.find('.page-quick-sidebar-chat-users');
            var chatUsersHeight;

            chatUsersHeight = wrapper.height() - wrapper.find('.nav-tabs').outerHeight(true);

            // chat user list 
            App.destroySlimScroll(chatUsers);
            chatUsers.attr("data-height", chatUsersHeight);
            App.initSlimScroll(chatUsers);

            var chatMessages = wrapperChat.find('.page-quick-sidebar-chat-user-messages');
            var chatMessagesHeight = chatUsersHeight - wrapperChat.find('.page-quick-sidebar-chat-user-form').outerHeight(true);
            chatMessagesHeight = chatMessagesHeight -
                wrapperChat.find('.page-quick-sidebar-nav').outerHeight(true) -
                wrapperChat.find('.selected-chat-user').outerHeight(true);

            // user chat messages 
            App.destroySlimScroll(chatMessages);
            chatMessages.attr("data-height", chatMessagesHeight);
            //App.initSlimScroll(chatMessages);
            initSlimScroll(chatMessages, scrollEvent);
        };

        initChatSlimScroll(scrollEvent);
        App.addResizeHandler(initChatSlimScroll); // reinitialize on window resize

        wrapper.find('.page-quick-sidebar-chat-users').on('click', '.media-list > .media', function () {
            wrapperChat.addClass("page-quick-sidebar-content-item-shown");
        });

        var $backToList = wrapper.find('.page-quick-sidebar-chat-user .page-quick-sidebar-back-to-list');
        $backToList.on('click', function () {
            wrapperChat.removeClass("page-quick-sidebar-content-item-shown");
        });
    };

    return {

        init: function (scrollEvent) {
            //layout handlers
            handleQuickSidebarToggler(); // handles quick sidebar's toggler
            handleQuickSidebarChat(scrollEvent); // handles quick sidebar's chats
        },

        handleQuickSidebarChat: function (scrollEvent) {
            handleQuickSidebarChat(scrollEvent); // handles quick sidebar's chats
        }
    };

}();