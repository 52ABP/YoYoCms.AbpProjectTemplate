(function ($) {
    if (!$.validator) {
        return;
    }

    $.validator.setDefaults({
        errorElement: 'span',
        errorClass: 'help-block help-block-validation-error',
        focusInvalid: false,
        submitOnKeyPress: true,
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },

        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },

        errorPlacement: function (error, element) {
            if (element.closest('.input-icon').size() === 1) {
                error.insertAfter(element.closest('.input-icon'));
            } else {
                error.insertAfter(element);
            }
        },

        success: function (label) {
            label.closest('.form-group').removeClass('has-error');
            label.remove();
        },

        submitHandler: function (form) {
            $(form).find('.alert-danger').hide();
        }
    });
})(jQuery);