var app = app || {};
(function () {

    app.utils = app.utils || {};

    app.utils.truncateString = function (str, maxLength, postfix) {
        if (!str || !maxLength || str.length <= maxLength) {
            return str;
        }

        if (postfix === false) {
            return str.substr(0, maxLength);
        }

        return str.substr(0, maxLength - 1) + '&#133;';
    }

    app.utils.removeCookie = function (key) {
        document.cookie = key + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    }

})();