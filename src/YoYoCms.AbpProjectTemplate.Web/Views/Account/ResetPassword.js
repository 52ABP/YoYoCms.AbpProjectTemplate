var CurrentPage = function () {

    var _passwordComplexityHelper = new app.PasswordComplexityHelper();

    var handleResetPassword = function () {

        $('.pass-reset-form').validate({
            rules: {
                PasswordRepeat: {
                    equalTo: "#Password"
                }
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        var $element = $('#Password');
        _passwordComplexityHelper.setPasswordComplexityRules($element, window.passwordComplexitySetting);
    }

    return {
        init: function () {
            handleResetPassword();
        }
    };

}();