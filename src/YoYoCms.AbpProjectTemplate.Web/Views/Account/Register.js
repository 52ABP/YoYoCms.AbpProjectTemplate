var CurrentPage = function () {

    jQuery.validator.addMethod("customUsername", function (value, element) {
        if (value === $('input[name="EmailAddress"]').val()) {
            return true;
        }

        return !$.validator.methods.email.apply(this, arguments);
    }, abp.localization.localize("RegisterFormUserNameInvalidMessage"));

    var _passwordComplexityHelper = new app.PasswordComplexityHelper();

    var handleRegister = function () {

        $('.register-form').validate({
            rules: {
                PasswordRepeat: {
                    equalTo: "#RegisterPassword"
                },
                UserName: {
                    required: true,
                    customUsername: true
                }
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        var $element = $('#RegisterPassword');
        _passwordComplexityHelper.setPasswordComplexityRules($element, window.passwordComplexitySetting);
    }

    return {
        init: function () {
            handleRegister();
        }
    };

}();