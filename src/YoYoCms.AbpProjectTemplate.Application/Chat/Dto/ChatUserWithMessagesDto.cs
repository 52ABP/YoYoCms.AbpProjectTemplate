using System.Collections.Generic;

namespace YoYoCms.AbpProjectTemplate.Chat.Dto
{
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}