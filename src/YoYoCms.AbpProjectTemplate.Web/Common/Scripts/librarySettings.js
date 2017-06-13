(function () {

    //Set Moment Timezone
    if (abp.clock.provider.supportsMultipleTimezone && window.moment) {
        moment.tz.setDefault(abp.timing.timeZoneInfo.iana.timeZoneId);
    }

    //Localize Sweet Alert
    if (abp.libs.sweetAlert) {
        abp.libs.sweetAlert.config.info.confirmButtonText = app.localize("Ok");
        abp.libs.sweetAlert.config.success.confirmButtonText = app.localize("Ok");
        abp.libs.sweetAlert.config.warn.confirmButtonText = app.localize("Ok");
        abp.libs.sweetAlert.config.error.confirmButtonText = app.localize("Ok");

        abp.libs.sweetAlert.config.confirm.confirmButtonText = app.localize("Yes");
        abp.libs.sweetAlert.config.confirm.cancelButtonText = app.localize("Cancel");
    }

})();