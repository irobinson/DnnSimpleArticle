namespace DotNetNuke.Modules.DnnSimpleArticle.Components
{
    public interface IGlobalsHelper
    {
        string NavigateUrl(int tabId);
        string NavigateUrl(int tabId, string controlKey, params string[] additionalParameters);
    }

    public class GlobalsHelper : IGlobalsHelper
    {
        public string NavigateUrl(int tabId)
        {
            return Common.Globals.NavigateURL(tabId);
        }

        public string NavigateUrl(int tabId, string controlKey, params string[] additionalParameters)
        {
            return Common.Globals.NavigateURL(tabId, controlKey, additionalParameters);
        }
    }
}