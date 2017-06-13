(function () {
    app.modals.MySettingsModal = function () {

        var _profileService = abp.services.app.profile;
        var _initialTimezone = null;

        var _modalManager;
        var _$form = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$form = _modalManager.getModal().find('form[name=MySettingsModalForm]');
            _$form.validate();

            _initialTimezone = _$form.find("[name='Timezone']").val();
        };

        this.save = function () {
            if (!_$form.valid()) {
                return;
            }

            var profile = _$form.serializeFormToObject();

            _modalManager.setBusy(true);
            _profileService.updateCurrentUserProfile(profile)
                .done(function () {
                    $('#HeaderCurrentUserName').text(profile.UserName);
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();

                    var newTimezone = _$form.find("[name='Timezone']").val();

                    if (abp.clock.provider.supportsMultipleTimezone && _initialTimezone !== newTimezone) {
                        abp.message.info(app.localize('TimeZoneSettingChangedRefreshPageNotification')).done(function () {
                            window.location.reload();
                        });
                    }

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})();