(function ($) {
    $(function () {

        var _$auditLogsTable = $('#AuditLogsTable');
        var _$filterForm = $('#AuditLogFilterForm');
        var _auditLogService = abp.services.app.auditLog;

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        _$filterForm.find('input.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function(start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        _$auditLogsTable.jtable({

            title: app.localize('AuditLogs'),

            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _auditLogService.getAuditLogs
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var $div = $('<div class=\"text-center\"></div>');

                        $div.append('<button class="btn btn-default btn-xs"><i class="fa fa-search"></i></button>')
                            .click(function () {
                                showDetails(data.record);
                            });

                        return $div;
                    }
                },
                exception: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var $div = $('<div class=\"text-center\"></div>');

                        if (data.record.exception) {
                            $div.append('<i class="fa fa-warning font-yellow-gold"></i>');
                        } else {
                            $div.append('<i class="fa fa-check-circle font-green"></i>');
                        }

                        return $div;
                    }
                },
                executionTime: {
                    title: app.localize('Time'),
                    width: '13%',
                    display: function (data) {
                        return moment(data.record.executionTime).format('YYYY-MM-DD HH:mm:ss');
                    }
                },
                userName: {
                    title: app.localize('UserName'),
                    width: '10%'
                },
                serviceName: {
                    title: app.localize('Service'),
                    width: '17%',
                    sorting: false
                },
                methodName: {
                    title: app.localize('Action'),
                    width: '10%',
                    sorting: false
                },
                executionDuration: {
                    title: app.localize('Duration'),
                    width: '5%',
                    display: function (data) {
                        return app.localize('Xms', data.record.executionDuration);
                    }
                },
                clientIpAddress: {
                    title: app.localize('IpAddress'),
                    width: '10%',
                    sorting: false
                },
                clientName: {
                    title: app.localize('Client'),
                    width: '10%',
                    sorting: false
                },
                browserInfo: {
                    title: app.localize('Browser'),
                    width: '15%',
                    sorting: false
                }
            }

        });

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        function getAuditLogs() {
            _$auditLogsTable.jtable('load', createRequestParams());
        }

        function getFormattedParameters(parameters) {
            try {
                var json = JSON.parse(parameters);
                return JSON.stringify(json, null, 4);
            } catch (e) {
                return parameters;
            }
        }

        function showDetails(auditLog) {
            $('#AuditLogDetailModal_UserName').html(auditLog.userName);
            $('#AuditLogDetailModal_ClientIpAddress').html(auditLog.clientIpAddress);
            $('#AuditLogDetailModal_ClientName').html(auditLog.clientName);
            $('#AuditLogDetailModal_BrowserInfo').html(auditLog.browserInfo);
            $('#AuditLogDetailModal_ServiceName').html(auditLog.serviceName);
            $('#AuditLogDetailModal_MethodName').html(auditLog.methodName);
            $('#AuditLogDetailModal_ExecutionTime').html(moment(auditLog.executionTime).fromNow() + ' (' + moment(auditLog.executionTime).format('YYYY-MM-DD hh:mm:ss') + ')');
            $('#AuditLogDetailModal_Duration').html(app.localize('Xms', auditLog.executionDuration));
            $('#AuditLogDetailModal_Parameters').html(getFormattedParameters(auditLog.parameters));

            if (auditLog.impersonatorUserId) {
                $('#AuditLogDetailModal_ImpersonatorInfo').show();
            } else {
                $('#AuditLogDetailModal_ImpersonatorInfo').hide();
            }

            if (auditLog.exception) {
                $('#AuditLogDetailModal_Success').hide();
                $('#AuditLogDetailModal_Exception').show();
                $('#AuditLogDetailModal_Exception').html(auditLog.exception);
            } else {
                $('#AuditLogDetailModal_Exception').hide();
                $('#AuditLogDetailModal_Success').show();
            }

            if (auditLog.customData) {
                $('#AuditLogDetailModal_CustomData_None').hide();
                $('#AuditLogDetailModal_CustomData').show();
                $('#AuditLogDetailModal_CustomData').html(auditLog.customData);
            } else {
                $('#AuditLogDetailModal_CustomData').hide();
                $('#AuditLogDetailModal_CustomData_None').show();
            }

            $('#AuditLogDetailModal').modal('show');
        }

        getAuditLogs();

        $('#RefreshAuditLogsButton').click(function(e) {
            e.preventDefault();
            getAuditLogs();
        });

        $('#ExportAuditLogsToExcelButton').click(function (e) {
            e.preventDefault();
            _auditLogService.getAuditLogsToExcel(createRequestParams())
                .done(function(result) {
                    app.downloadTempFile(result);
                });
        });

        $('#ShowAdvancedFiltersSpan').click(function() {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        _$filterForm.keydown(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                getAuditLogs();
            }
        });
    });
})(jQuery);