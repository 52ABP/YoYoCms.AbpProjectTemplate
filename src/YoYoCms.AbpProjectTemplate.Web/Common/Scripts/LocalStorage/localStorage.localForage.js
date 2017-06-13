var app = app || {};
(function () {

    app.localStorage = app.localStorage || {};

    app.localStorage.setItem = function (key, value) {
        if (!localforage) {
            return;
        }

        localforage.setItem(key, value);
    };

    app.localStorage.getItem = function (key, callback) {
        if (!localforage || !callback) {
            return;
        }

        localforage.getItem(key)
            .then(function (value) {
                callback(value);
            });
    };

})();
