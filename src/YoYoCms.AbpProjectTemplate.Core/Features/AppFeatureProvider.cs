using Abp.Application.Features;
using Abp.Localization;
using Abp.UI.Inputs;

namespace YoYoCms.AbpProjectTemplate.Features
{
    /* This feature provider is just for an example.
     * You can freely delete all features and add your own.
     */
    public class AppFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            var chatFeature = context.Create(
                  AppFeatures.ChatFeature,
                  defaultValue: "false",
                  displayName: L("ChatFeature"),
                  inputType: new CheckboxInputType()
                  );

            chatFeature.CreateChildFeature(
                AppFeatures.TenantToTenantChatFeature,
                defaultValue: "false",
                displayName: L("TenantToTenantChatFeature"),
                inputType: new CheckboxInputType()
                );

            chatFeature.CreateChildFeature(
                AppFeatures.TenantToHostChatFeature,
                defaultValue: "false",
                displayName: L("TenantToHostChatFeature"),
                inputType: new CheckboxInputType()
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpProjectTemplateConsts.LocalizationSourceName);
        }
    }
}
