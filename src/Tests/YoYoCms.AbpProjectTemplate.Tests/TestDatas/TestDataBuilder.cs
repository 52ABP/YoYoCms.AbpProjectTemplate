using EntityFramework.DynamicFilters;
using YoYoCms.AbpProjectTemplate.EntityFramework;

namespace YoYoCms.AbpProjectTemplate.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly AbpProjectTemplateDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(AbpProjectTemplateDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();

            _context.SaveChanges();
        }
    }
}
