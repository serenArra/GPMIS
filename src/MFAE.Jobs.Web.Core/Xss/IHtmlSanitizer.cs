using Abp.Dependency;

namespace MFAE.Jobs.Web.Xss
{
    public interface IHtmlSanitizer: ITransientDependency
    {
        string Sanitize(string html);
    }
}