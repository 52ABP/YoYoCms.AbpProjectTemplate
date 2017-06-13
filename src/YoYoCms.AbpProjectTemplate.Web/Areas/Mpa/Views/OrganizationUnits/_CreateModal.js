(function() {
    app.modals.CreateOrganizationUnitModal = function () {

        var _modalManager;
        var _organizationUnitService = abp.services.app.organizationUnit;
        var _$form = null;

        this.init = function(modalManager) {
            _modalManager = modalManager;

            _$form = _modalManager.getModal().find('form[name=OrganizationUnitForm]');
            _$form.validate({ ignore: "" });
        };

        this.save = function() {
            if (!_$form.valid()) {
                return;
            }

            var organizationUnit = _$form.serializeFormToObject();

            _modalManager.setBusy(true);
            _organizationUnitService.createOrganizationUnit(
                organizationUnit
            ).done(function(result) {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.setResult(result);
                _modalManager.close();
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})();