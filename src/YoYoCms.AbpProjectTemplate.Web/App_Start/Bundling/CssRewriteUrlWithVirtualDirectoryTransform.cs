using System.Web;
using System.Web.Optimization;
using Abp.Extensions;

namespace YoYoCms.AbpProjectTemplate.Web.Bundling
{
    public class CssRewriteUrlWithVirtualDirectoryTransform : IItemTransform
    {
        private readonly CssRewriteUrlTransform _rewriteUrlTransform;

        public CssRewriteUrlWithVirtualDirectoryTransform()
        {
            _rewriteUrlTransform = new CssRewriteUrlTransform();
        }

        public string Process(string includedVirtualPath, string input)
        {
            var result = _rewriteUrlTransform.Process(includedVirtualPath, input);

            if (!HttpRuntime.AppDomainAppVirtualPath.IsNullOrEmpty() && HttpRuntime.AppDomainAppVirtualPath != "/")
            {
                result = result.Replace(@"url(/", @"url(" + HttpRuntime.AppDomainAppVirtualPath + @"/");
            }

            return result;
        }
    }
}