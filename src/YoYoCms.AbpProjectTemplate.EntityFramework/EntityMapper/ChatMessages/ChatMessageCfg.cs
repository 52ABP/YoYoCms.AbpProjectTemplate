using System.Data.Entity.ModelConfiguration;
using YoYoCms.AbpProjectTemplate.Chat;

namespace YoYoCms.AbpProjectTemplate.EntityMapper.ChatMessages
{
    /// <summary>
    /// 聊天记录信息
    /// </summary>
    public class ChatMessageCfg:EntityTypeConfiguration<ChatMessage>
    {

        public ChatMessageCfg()
        {

            ToTable("ChatMessages", AbpProjectTemplateConsts.SchemaName.ABP);

        }
    }
}