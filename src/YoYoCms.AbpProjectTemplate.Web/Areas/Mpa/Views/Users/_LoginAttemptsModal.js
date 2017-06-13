(function ($) {
    app.modals.LoginAttemptsModal = function () {

        var _modalManager;

        function getCreationTime(creationTime) {
            return moment(creationTime).fromNow() + ' (' + moment(creationTime).format('YYYY-MM-DD hh:mm:ss') + ')';
        };

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _modalManager.getModal().find('p.login-attempt-date').each(function (index, p) {
                var $p = $(p);
                var date = $p.attr("data-date");
                var formattedDate = getCreationTime(date);
                $p.html(formattedDate);
            });
        };
    };
})(jQuery);