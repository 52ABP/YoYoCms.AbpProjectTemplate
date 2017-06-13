(function ($) {
    app.modals.ChangePasswordModal = function () {

        var _profileService = abp.services.app.profile;
        var _passwordComplexityHelper = new app.PasswordComplexityHelper();

        var _modalManager;
        var _$form = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _profileService.getPasswordComplexitySetting().done(function (result) {
                _$form = _modalManager.getModal().find('form[name=ChangePasswordModalForm]');
                _$form.validate({
                    errorElement: 'span',
                    errorClass: 'help-block text-danger',
                    errorPlacement: function (error, element) {
                        if (element.closest('.input-icon').size() === 1) {
                            error.insertAfter(element.closest('.input-icon'));
                        } else {
                            error.insertAfter(element.parent());
                        }
                    }
                });

                _passwordComplexityHelper.setPasswordComplexityRules(_$form.find("input[name=NewPassword]"), result.setting);
            });
        };

        this.save = function () {
            if (!_$form.valid()) {
                return;
            }

            _modalManager.setBusy(true);
            _profileService.changePassword(_$form.serializeFormToObject())
                .done(function () {
                    abp.notify.info(app.localize('YourPasswordHasChangedSuccessfully'));
                    _modalManager.close();
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);