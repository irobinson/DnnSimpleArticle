namespace DotNetNuke.Modules.DnnSimpleArticle.Components
{
    using System.Collections.Generic;

    public interface IArticleController
    {
        Article GetArticle(int articleId);

        ///<summary>
        /// Get a list of articles for a moduleid, 1000 of them
        ///</summary>
        ///<param name="moduleId"></param>
        ///<returns></returns>
        List<Article> GetArticles(int moduleId);

        ///<summary>
        /// Get a list of articles for a moduleid
        ///</summary>
        ///<param name="moduleId"></param>
        ///<returns></returns>
        List<Article> GetArticles(int moduleId, int pageSize, int pageNumber);

        ///<summary>
        /// Get a list of articles for a portal
        ///</summary>
        ///<param name="portalId"></param>
        ///<returns></returns>
        List<Article> GetAllArticles(int portalId);

        ///<summary>Save the article, checks if we are creating new, or updating an existing
        ///</summary>
        ///<param name="a"></param>
        ///<param name="tabId"></param>
        ///<returns></returns>
        int SaveArticle(Article a, int tabId);

        ///<summary>Delete an article based on ID
        ///</summary>
        ///<param name="articleId"></param>
        void DeleteArticle(int articleId);

        ///<summary>Delete all articles based on a moduleid
        ///</summary>
        ///<param name="moduleId"></param>
        void DeleteArticles(int moduleId);
    }
}