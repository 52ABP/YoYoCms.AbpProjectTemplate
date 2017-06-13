using System.Text.RegularExpressions;
using Abp.Extensions;

namespace YoYoCms.AbpProjectTemplate.Security
{
    public class PasswordComplexityChecker
    {
        public bool Check(PasswordComplexitySetting setting, string password)
        {
            if (password.IsNullOrEmpty())
            {
                return false;
            }

            if (password.Length < setting.MinLength)
            {
                return false;
            }

            if (password.Length > setting.MaxLength)
            {
                return false;
            }

            if (setting.UseUpperCaseLetters)
            {
                var useUpperCaseLettersRegex = new Regex("[A-Z]");
                if (!useUpperCaseLettersRegex.IsMatch(password))
                {
                    return false;
                }
            }

            if (setting.UseLowerCaseLetters)
            {
                var useLowerCaseLettersRegex = new Regex("[a-z]");
                if (!useLowerCaseLettersRegex.IsMatch(password))
                {
                    return false;
                }
            }

            if (setting.UseNumbers)
            {
                var useNumbersRegex = new Regex("[0-9]");
                if (!useNumbersRegex.IsMatch(password))
                {
                    return false;
                }
            }

            if (setting.UsePunctuations)
            {
                var usePunctuations = new Regex(@"[\p{P}\p{S}]");
                if (!usePunctuations.IsMatch(password))
                {
                    return false;
                }
            }


            return true;
        }
    }
}
