(function () {
    app.modals.TenantFeaturesModal = function() {

        var _tenantService = abp.services.app.tenant;

        var _modalManager;
        var _featuresTree;

        function _resetTenantSpecificFeatures() {
            _modalManager.setBusy(true);
            _tenantService.resetTenantSpecificFeatures({
                id: _modalManager.getArgs().id
            }).done(function () {
                abp.notify.info(app.localize('ResetSuccessfully'));
                _modalManager.getModal().on('hidden.bs.modal', function (e) {
                    _modalManager.reopen();
                });
                _modalManager.close();
            }).always(function () {
                _modalManager.setBusy(false);
            });
        }

        this.init = function(modalManager) {
            _modalManager = modalManager;

            _featuresTree = new FeaturesTree();
            _featuresTree.init(_modalManager.getModal().find('.feature-tree'));

            _modalManager.getModal().find('[data-toggle=tooltip]').tooltip();

            _modalManager.getModal().find('.reset-features-button').click(function () {
                _resetTenantSpecificFeatures();
            });
        };

        this.save = function () {
            if (!_featuresTree.isValid()) {
                abp.message.warn(app.localize('InvalidFeaturesWarning'));
                return;
            }

            _modalManager.setBusy(true);
            _tenantService.updateTenantFeatures({
                id: _modalManager.getArgs().id,
                featureValues: _featuresTree.getFeatureValues()
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})();