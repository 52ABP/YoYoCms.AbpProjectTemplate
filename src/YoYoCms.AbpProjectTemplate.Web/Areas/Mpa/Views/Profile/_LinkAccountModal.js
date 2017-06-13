(function () {
    app.modals.LinkAccountModal = function () {

        var _userLinkService = abp.services.app.userLink;

        var _modalManager;
        var _$form = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$form = _modalManager.getModal().find('form[name=LinkAccountModalForm]');
            _$form.validate();
        };

        this.save = function () {
            if (!_$form.valid()) {
                return;
            }

            var userLogin = _$form.serializeFormToObject();

            _modalManager.setBusy(true);
            _userLinkService.linkToUser(userLogin)
                .done(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.setResult(result);
                    _modalManager.close();
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})();