(function () {
    app.PasswordComplexityHelper = function () {

        function reviver(key, val) {
            if (key && key.charAt(0) !== key.charAt(0).toLowerCase())
                this[key.charAt(0).toLowerCase() + key.slice(1)] = val;
            else
                return val;
        };

        var _buildPasswordComplexityErrorMessage = function (setting) {
            var message = "<ul>";

            if (setting.minLength) {
                message += "<li>" + abp.utils.formatString(app.localize("PasswordComplexity_MinLength_Hint"), setting.minLength) + "</li>";
            }

            if (setting.maxLength) {
                message += "<li>" + abp.utils.formatString(app.localize("PasswordComplexity_MaxLength_Hint"), setting.maxLength) + "</li>";
            }

            if (setting.useUpperCaseLetters) {
                message += "<li>" + app.localize("PasswordComplexity_UseUpperCaseLetters_Hint") + "</li>";
            }

            if (setting.useLowerCaseLetters) {
                message += "<li>" + app.localize("PasswordComplexity_UseLowerCaseLetters_Hint") + "</li>";
            }

            if (setting.useNumbers) {
                message += "<li>" + app.localize("PasswordComplexity_UseNumbers_Hint") + "</li>";
            }

            if (setting.usePunctuations) {
                message += "<li>" + app.localize("PasswordComplexity_UsePunctuations_Hint") + "</li>";
            }

            return message + "</ul>";
        }

        var _setPasswordComplexityRules = function ($element, setting) {

            setting = JSON.parse(JSON.stringify(setting), reviver);

            if (setting) {
                var message = _buildPasswordComplexityErrorMessage(setting);

                jQuery.validator.addMethod("passwordComplexity", function (value, element) {
                    if (setting.minLength && value.length < setting.minLength) {
                        return false;
                    }

                    if (setting.maxLength && value.length > setting.maxLength) {
                        return false;
                    }

                    if (setting.useUpperCaseLetters && !/[A-Z]/.test(value)) {
                        return false;
                    }

                    if (setting.useLowerCaseLetters && !/[a-z]/.test(value)) {
                        return false;
                    }

                    if (setting.useNumbers && !/[0-9]/.test(value)) {
                        return false;
                    }

                    if (setting.usePunctuations && !/[!@#\$%\^\&*'"\/{}\[\]?,;|)\(+=._-]+/.test(value)) {
                        return false;
                    }

                    return true;
                }, message);

                $element.rules("add", "passwordComplexity");
            }
        };

        return {
            setPasswordComplexityRules: _setPasswordComplexityRules
        };
    };
})();