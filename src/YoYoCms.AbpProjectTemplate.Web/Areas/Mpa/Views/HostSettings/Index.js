(function ($) {
    $(function () {
        var _hostSettingsService = abp.services.app.hostSettings;
        var _initialTimeZone = $('#GeneralSettingsForm [name=Timezone]').val();
        var _usingDefaultTimeZone = $('#GeneralSettingsForm [name=TimezoneForComparison]').val() === abp.setting.values["Abp.Timing.TimeZone"];

        var _$tabPanel = $('#SettingsTabPanel');

        var _$smtpCredentialFormGroups = _$tabPanel
            .find('input[name=SmtpDomain],input[name=SmtpUserName],input[name=SmtpPassword]')
            .closest('.form-group');

        var _$tenantSettingsCheckboxes = _$tabPanel
           .find('input[name=IsNewRegisteredTenantActiveByDefault],input[name=UseCaptchaOnRegistration]')
           .closest('.md-checkbox');

        var _$userLockOutSettingsFormItems = $('#UserLockOutSettingsForm')
            .find('input')
            .not('#Setting_UserLockOut_IsEnabled')
            .closest('.form-group');

        var _$twoFactorLoginSettingsFormItems = $('#TwoFactorLoginSettingsForm')
            .find('input')
            .not('#Setting_TwoFactorLogin_IsEnabled')
            .closest('.md-checkbox');

        function toggleSmtpCredentialFormGroups() {
            if ($('#Settings_SmtpUseDefaultCredentials').is(':checked')) {
                _$smtpCredentialFormGroups.slideUp('fast');
            } else {
                _$smtpCredentialFormGroups.slideDown('fast');
            }
        }

        function toggleTenantManagementFormGroups() {
            if (!$('#Setting_AllowSelfRegistration').is(':checked')) {
                _$tenantSettingsCheckboxes.slideUp('fast');
            } else {
                _$tenantSettingsCheckboxes.slideDown('fast');
            }
        }

        function toggleUserLockOutSettingsFormItems() {
            if ($('#Setting_UserLockOut_IsEnabled').is(':checked')) {
                _$userLockOutSettingsFormItems.slideDown('fast');
            } else {
                _$userLockOutSettingsFormItems.slideUp('fast');
            }
        }

        function toggleTwoFactorLoginSettingsFormItems() {
            if ($('#Setting_TwoFactorLogin_IsEnabled').is(':checked')) {
                _$twoFactorLoginSettingsFormItems.slideDown('fast');
            } else {
                _$twoFactorLoginSettingsFormItems.slideUp('fast');
            }
        }

        toggleSmtpCredentialFormGroups();
        toggleTenantManagementFormGroups();
        toggleUserLockOutSettingsFormItems();
        toggleTwoFactorLoginSettingsFormItems();

        $('#Settings_SmtpUseDefaultCredentials').change(function () {
            toggleSmtpCredentialFormGroups();
        });

        $('#Setting_AllowSelfRegistration').change(function () {
            toggleTenantManagementFormGroups();
        });

        $('#Setting_UserLockOut_IsEnabled').change(function () {
            toggleUserLockOutSettingsFormItems();
        });

        $('#Setting_TwoFactorLogin_IsEnabled').change(function () {
            toggleTwoFactorLoginSettingsFormItems();
        });

        $('#Setting_PasswordComplexity_UseDefaultSettings').change(function (val) {
            if ($('#Setting_PasswordComplexity_UseDefaultSettings').is(":checked")) {
                $('#DefaultPasswordComplexitySettingsForm').show();
                $('#PasswordComplexitySettingsForm').hide();
            } else {
                $('#DefaultPasswordComplexitySettingsForm').hide();
                $('#PasswordComplexitySettingsForm').show();
            }
        });

        function getDefaultPasswordComplexitySettings() {
            //note: this is a fix for '$('#DefaultPasswordComplexitySettingsForm').serializeFormToObject()' always returns true for checkboxes if they are disabled.
            var $disabledDefaultPasswordInputs = $('#DefaultPasswordComplexitySettingsForm input:disabled');
            $disabledDefaultPasswordInputs.removeAttr("disabled");
            var defaultPasswordComplexitySettings = $('#DefaultPasswordComplexitySettingsForm').serializeFormToObject();
            $disabledDefaultPasswordInputs.attr("disabled", "disabled");
            return defaultPasswordComplexitySettings;
        }

        $('#SaveAllSettingsButton').click(function () {
            _hostSettingsService.updateAllSettings({
                general: $('#GeneralSettingsForm').serializeFormToObject(),
                tenantManagement: $('#TenantManagementSettingsForm').serializeFormToObject(),
                userManagement: $('#UserManagementSettingsForm').serializeFormToObject(),
                email: $('#EmailSmtpSettingsForm').serializeFormToObject(),
                chat: $('#ChatSettingsForm').serializeFormToObject(),
                security: {
                    useDefaultPasswordComplexitySettings: $('#Setting_PasswordComplexity_UseDefaultSettings').is(":checked"),
                    passwordComplexity: $('#PasswordComplexitySettingsForm').serializeFormToObject(),
                    defaultPasswordComplexity: getDefaultPasswordComplexitySettings(),
                    userLockOut: $('#UserLockOutSettingsForm').serializeFormToObject(),
                    twoFactorLogin: $('#TwoFactorLoginSettingsForm').serializeFormToObject()
                }
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));

                var newTimezone = $('#GeneralSettingsForm [name=Timezone]').val();
                if (abp.clock.provider.supportsMultipleTimezone &&
                        _usingDefaultTimeZone &&
                        _initialTimeZone !== newTimezone) {
                    abp.message.info(app.localize('TimeZoneSettingChangedRefreshPageNotification')).done(function () {
                        window.location.reload();
                    });
                }
            });
        });
        
        $('#SendTestEmailButton').click(function () {
            _hostSettingsService.sendTestEmail({
                emailAddress: $('#TestEmailAddressInput').val()
            }).done(function () {
                abp.notify.info(app.localize('TestEmailSentSuccessfully'));
            });
        });
    });
})(jQuery);