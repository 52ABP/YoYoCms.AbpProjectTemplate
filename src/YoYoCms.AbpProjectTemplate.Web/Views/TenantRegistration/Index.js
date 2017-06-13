var CurrentPage = function () {

    var _passwordComplexityHelper = new app.PasswordComplexityHelper();

    var handleRegister = function () {

        $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
        },
        app.localize('TenantName_Regex_Description'));

        $('.register-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                AdminPasswordRepeat: {
                    equalTo: "#AdminPassword"
                },
                TenancyName: {
                    required: true,
                    regex: '^[a-zA-Z][a-zA-Z0-9_-]{1,}$'
                }
            },

            messages: {

            },

            invalidHandler: function (event, validator) {

            },

            highlight: function (element) {
                $(element).closest('.form-group').addClass('has-error');
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                if (element.closest('.input-icon').size() === 1) {
                    error.insertAfter(element.closest('.input-icon'));
                } else {
                    error.insertAfter(element);
                }
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        $('.register-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.register-form').valid()) {
                    $('.register-form').submit();
                }
                return false;
            }
        });

        _passwordComplexityHelper.setPasswordComplexityRules($("input[name=AdminPassword]"), window.passwordComplexitySetting);
    }

    return {
        init: function () {
            handleRegister();
        }
    };

}();