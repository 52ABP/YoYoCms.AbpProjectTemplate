using System.Data.Entity.ModelConfiguration;
using YoYoCms.AbpProjectTemplate.Storage;

namespace YoYoCms.AbpProjectTemplate.EntityMapper.BinaryObjects
{
    /// <summary>
    /// 二进制对象存储
    /// </summary>
    public class BinaryObjectCfg:EntityTypeConfiguration<BinaryObject>
    {
        public BinaryObjectCfg()
        {

            ToTable("AppBinaryObjects", AbpProjectTemplateConsts.SchemaName.ABP);


        }
    }
}