using Abp.Application.Features;
using Abp.UI;
using YoYoCms.AbpProjectTemplate.Features;

namespace YoYoCms.AbpProjectTemplate.Chat
{
    public class ChatFeatureChecker : AbpProjectTemplateDomainServiceBase, IChatFeatureChecker
    {
        private readonly IFeatureChecker _featureChecker;

        public ChatFeatureChecker(
            IFeatureChecker featureChecker
        )
        {
            _featureChecker = featureChecker;
        }

        public void CheckChatFeatures(int? sourceTenantId, int? targetTenantId)
        {
            CheckChatFeaturesInternal(sourceTenantId, targetTenantId, ChatSide.Sender);
            CheckChatFeaturesInternal(targetTenantId, sourceTenantId, ChatSide.Receiver);
        }

        private void CheckChatFeaturesInternal(int? sourceTenantId, int? targetTenantId, ChatSide side)
        {
            var localizationPosfix = side == ChatSide.Sender ? "ForSender" : "ForReceiver";
            if (sourceTenantId.HasValue)
            {
                if (!_featureChecker.IsEnabled(sourceTenantId.Value, AppFeatures.ChatFeature))
                {
                    throw new UserFriendlyException(L("ChatFeatureIsNotEnabled" + localizationPosfix));
                }

                if (targetTenantId.HasValue)
                {
                    if (sourceTenantId == targetTenantId)
                    {
                        return;
                    }

                    if (!_featureChecker.IsEnabled(sourceTenantId.Value, AppFeatures.TenantToTenantChatFeature))
                    {
                        throw new UserFriendlyException(L("TenantToTenantChatFeatureIsNotEnabled" + localizationPosfix));
                    }
                }
                else
                {
                    if (!_featureChecker.IsEnabled(sourceTenantId.Value, AppFeatures.TenantToHostChatFeature))
                    {
                        throw new UserFriendlyException(L("TenantToHostChatFeatureIsNotEnabled" + localizationPosfix));
                    }
                }
            }
            else
            {
                if (targetTenantId.HasValue)
                {
                    if (!_featureChecker.IsEnabled(targetTenantId.Value, AppFeatures.TenantToHostChatFeature))
                    {
                        throw new UserFriendlyException(L("TenantToHostChatFeatureIsNotEnabled" + (side == ChatSide.Sender ? "ForReceiver" : "ForSender")));
                    }
                }
            }
        }
    }
}