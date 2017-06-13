using YoYoCms.AbpProjectTemplate.Security;
using Shouldly;
using Xunit;

namespace YoYoCms.AbpProjectTemplate.Tests.Security
{
    public class PasswordComplexity_Tests
    {
        private readonly PasswordComplexitySetting _passwordComplexitySetting;

        public PasswordComplexity_Tests()
        {
            _passwordComplexitySetting = new PasswordComplexitySetting
            {
                MinLength = 4,
                MaxLength = 15,
                UseUpperCaseLetters = true,
                UseLowerCaseLetters = true,
                UseNumbers = true,
                UsePunctuations = true
            };
        }

        [InlineData("123qqweA!")]
        [InlineData("123qqweA._")]
        [Theory]
        public void ValidPassword_Tests(string password)
        {
            var isValid = new PasswordComplexityChecker().Check(_passwordComplexitySetting, password);
            isValid.ShouldBe(true);
        }


        [InlineData("123qqweA")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123")]
        [Theory]
        public void InvalidPassword_Tests(string password)
        {
            var isValid = new PasswordComplexityChecker().Check(_passwordComplexitySetting, password);
            isValid.ShouldBe(false);
        }
    }
}
