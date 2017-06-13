(function () {
    app.modals.UserPermissionsModal = function() {

        var _userService = abp.services.app.user;

        var _modalManager;
        var _permissionsTree;

        function _resetUserSpecificPermissions() {
            _modalManager.setBusy(true);
            _userService.resetUserSpecificPermissions({
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

            _permissionsTree = new PermissionsTree();
            _permissionsTree.init(_modalManager.getModal().find('.permission-tree'));

            _modalManager.getModal().find('[data-toggle=tooltip]').tooltip();

            _modalManager.getModal().find('.reset-permissions-button').click(function () {
                _resetUserSpecificPermissions();
            });
        };

        this.save = function() {
            _modalManager.setBusy(true);
            _userService.updateUserPermissions({
                id: _modalManager.getArgs().id,
                grantedPermissionNames: _permissionsTree.getSelectedPermissionNames()
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})();