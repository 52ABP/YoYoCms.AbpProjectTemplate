(function () {
    app.modals.CreateOrEditEditionModal = function () {

        var _modalManager;
        var _editionService = abp.services.app.edition;
        var _$editionInformationForm = null;
        var _featuresTree;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _featuresTree = new FeaturesTree();
            _featuresTree.init(_modalManager.getModal().find('.feature-tree'));

            _$editionInformationForm = _modalManager.getModal().find('form[name=EditionInformationsForm]');
            _$editionInformationForm.validate();
        };

        this.save = function() {
            if (!_$editionInformationForm.valid()) {
                return;
            }

            if (!_featuresTree.isValid()) {
                abp.message.warn(app.localize('InvalidFeaturesWarning'));
                return;
            }

            var edition = _$editionInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _editionService.createOrUpdateEdition({
                edition: edition,
                featureValues: _featuresTree.getFeatureValues()
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditEditionModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})();