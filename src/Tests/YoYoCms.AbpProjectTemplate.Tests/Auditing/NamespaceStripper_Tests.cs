using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoYoCms.AbpProjectTemplate.Auditing;
using Shouldly;
using Xunit;

namespace YoYoCms.AbpProjectTemplate.Tests.Auditing
{
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("YoYoCms.AbpProjectTemplate.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("YoYoCms.AbpProjectTemplate.Auditing.GenericEntityService`1[[YoYoCms.AbpProjectTemplate.Storage.BinaryObject, YoYoCms.AbpProjectTemplate.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("YoYoCms.AbpProjectTemplate.Auditing.XEntityService`1[YoYoCms.AbpProjectTemplate.Auditing.AService`5[[YoYoCms.AbpProjectTemplate.Storage.BinaryObject, YoYoCms.AbpProjectTemplate.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[YoYoCms.AbpProjectTemplate.Storage.TestObject, YoYoCms.AbpProjectTemplate.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
