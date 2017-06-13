(function() {
    app.modals.EditOrganizationUnitModal = function () {

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
            _organizationUnitService.updateOrganizationUnit(
                organizationUnit
            ).done(function(result) {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                _modalManager.setResult(result);
            }).always(function() {
                _modalManager.setBusy(false);
            });
        };
    };
})();