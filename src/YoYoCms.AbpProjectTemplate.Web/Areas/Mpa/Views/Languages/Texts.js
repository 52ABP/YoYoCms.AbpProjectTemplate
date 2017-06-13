(function () {
    $(function () {

        var _$textsTable = $('#TextsTable');
        var _languageService = abp.services.app.language;

        var _editTextModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Languages/EditTextModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Languages/_EditTextModal.js',
            modalClass: 'EditLanguageTextModal'
        });

        _$textsTable.jtable({

            title: app.localize('LanguageTexts'),

            paging: true,
            sorting: true,

            actions: {
                listAction: {
                    method: _languageService.getLanguageTexts
                }
            },

            fields: {
                key: {
                    key: true,
                    list: true,
                    title: app.localize('Key'),
                    width: '30%',
                    display: function (data) {
                        return '<span title="' + data.record.key + '">' + app.utils.truncateString(data.record.key, 32) + '</span>';
                    }
                },
                baseValue: {
                    title: app.localize('BaseValue'),
                    width: '30%',
                    display: function (data) {
                        return '<span title="' + (data.record.baseValue || '') + '">' + (app.utils.truncateString(data.record.baseValue, 32) || '') + '</span>';
                    }
                },
                targetValue: {
                    title: app.localize('TargetValue'),
                    width: '30%',
                    display: function (data) {
                        return '<span title="' + (data.record.targetValue || '') + '">' + (app.utils.truncateString(data.record.targetValue, 32) || '') + '</span>';
                    }
                },
                actions: {
                    title: '',
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                _editTextModal.open({
                                    sourceName: $('#TextSourceSelectionCombobox').val(),
                                    baseLanguageName: $('#TextBaseLanguageSelectionCombobox').val(),
                                    languageName: $('#TextTargetLanguageSelectionCombobox').val(),
                                    key: data.record.key
                                });
                            });

                        return $span;
                    }
                }
            }

        });

        $('#TextBaseLanguageSelectionCombobox,#TextTargetLanguageSelectionCombobox')
            .selectpicker({
                iconBase: "famfamfam-flag",
                tickIcon: "fa fa-check"
            });

        $('#TextSourceSelectionCombobox,#TargetValueFilterSelectionCombobox')
            .selectpicker({
                iconBase: "fa",
                tickIcon: "fa fa-check"
            });

        $('#RefreshTextsButton').click(function(e) {
            e.preventDefault();
            loadTable();
        });

        $('#TextsFilterForm select').change(function() {
            loadTable();
        });

        $('#TextFilter').focus();

        function loadTable() {
            _$textsTable.jtable('load', {
                targetLanguageName: $('#TextTargetLanguageSelectionCombobox').val(),
                sourceName: $('#TextSourceSelectionCombobox').val(),
                baseLanguageName: $('#TextBaseLanguageSelectionCombobox').val(),
                targetValueFilter: $('#TargetValueFilterSelectionCombobox').val(),
                filterText: $('#TextFilter').val()
            });
        }

        function reloadTable() {
            _$textsTable.jtable('reload');
        }

        abp.event.on('app.editLanguageTextModalSaved', function() {
            reloadTable();
        });

        loadTable();
    });
})();