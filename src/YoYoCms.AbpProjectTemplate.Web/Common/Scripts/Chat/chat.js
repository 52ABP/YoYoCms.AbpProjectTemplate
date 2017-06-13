var app = app || {};
(function () {

    app.chat = app.chat || {};
    app.chat.side = {
        sender: 1,
        receiver: 2
    };

    app.chat.readState = {
        unread: 1,
        read: 2
    };

    app.chat.sendMessage = function () {
        console.log(arguments);
    };

})();
