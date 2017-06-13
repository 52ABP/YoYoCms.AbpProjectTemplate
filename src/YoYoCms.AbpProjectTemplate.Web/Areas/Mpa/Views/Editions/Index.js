(function () {
    $(function () {

        var _$editionsTable = $('#EditionsTable');
        var _editionService = abp.services.app.edition;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Editions.Create'),
            edit: abp.auth.hasPermission('Pages.Editions.Edit'),
            'delete': abp.auth.hasPermission('Pages.Editions.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Editions/CreateOrEditModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Editions/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditEditionModal'
        });

        _$editionsTable.jtable({

            title: app.localize('Editions'),

            actions: {
                listAction: {
                    method: _editionService.getEditions
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '30%',
                    display: function (data) {
                        var $span = $('<span></span>');

                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-o"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteEdition(data.record);
                                });
                        }

                        return $span;
                    }
                },
                displayName: {
                    title: app.localize('EditionName'),
                    width: '35%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '35%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }

        });

        function deleteEdition(edition) {
            abp.message.confirm(
                app.localize('EditionDeleteWarningMessage', edition.displayName),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _editionService.deleteEdition({
                            id: edition.id
                        }).done(function () {
                            getEditions();
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        };

        $('#CreateNewEditionButton').click(function () {
            _createOrEditModal.open();
        });

        function getEditions() {
            _$editionsTable.jtable('load');
        }

        abp.event.on('app.createOrEditEditionModalSaved', function () {
            getEditions();
        });

        getEditions();
    });
})();