using System.Data.Entity.ModelConfiguration;
using YoYoCms.AbpProjectTemplate.Friendships;

namespace YoYoCms.AbpProjectTemplate.EntityMapper.FriendShips
{
    /// <summary>
    /// 好友列表
    /// </summary>
    public class FriendshipCfg : EntityTypeConfiguration<Friendship>
    {


        public FriendshipCfg()
        {
            ToTable("AppFriendships", AbpProjectTemplateConsts.SchemaName.ABP);


        }
    }
}