var EditTenantModal = (function ($) {
    app.modals.EditTenantModal = function() {

        var _modalManager;
        var _tenantService = abp.services.app.tenant;
        var _$tenantInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$tenantInformationForm = _modalManager.getModal().find('form[name=TenantInformationsForm]');
            _$tenantInformationForm.validate();
        };

        this.save = function () {
            if (!_$tenantInformationForm.valid()) {
                return;
            }

            var tenant = _$tenantInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _tenantService.updateTenant(
                tenant
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.editTenantModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);