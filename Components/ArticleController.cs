//
// DotNetNuke - http://www.dotnetnuke.com
// Copyright (c) 2002-2011
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

namespace DotNetNuke.Modules.DnnSimpleArticle.Components
{
    using System;
    using System.Collections.Generic;
    using Common.Utilities;
    using Data;
    using Taxonomy;

    ///<summary>
    /// ArticleController provides the implementation of methods for our article
    ///</summary>
    public class ArticleController : IArticleController
    {
        private readonly DataProvider dataProvider;
        private readonly IContentProvider contentProviderProvider;
        private readonly IGlobalsHelper globals;

        public ArticleController() : this(new SqlDataProvider(), new ContentProvider(), new GlobalsHelper())
        {
        }

        public ArticleController(DataProvider dataProvider, IContentProvider contentProviderProvider, IGlobalsHelper globals)
        {
            this.dataProvider = dataProvider;
            this.contentProviderProvider = contentProviderProvider;
            this.globals = globals;
        }

        ///<summary>
        /// Get an individual article
        ///</summary>
        ///<param name="articleId"></param>
        ///<returns></returns>
        public Article GetArticle(int articleId)
        {
            var article = dataProvider.GetArticle(articleId);
            return article != null ? CBO.FillObject<Article>(article) : null;
        }

        ///<summary>
        /// Get a list of articles for a moduleid, 1000 of them
        ///</summary>
        ///<param name="moduleId"></param>
        ///<returns></returns>
        public List<Article> GetArticles(int moduleId)
        {
            return GetArticles(moduleId, 1000,0);
        }

        ///<summary>
        /// Get a list of articles for a moduleid
        ///</summary>
        ///<param name="moduleId"></param>
        ///<returns></returns>
        public List<Article> GetArticles(int moduleId, int pageSize, int pageNumber)
        {
            return CBO.FillCollection<Article>(dataProvider.GetArticles(moduleId, pageSize, pageNumber));
        }

        ///<summary>
        /// Get a list of articles for a portal
        ///</summary>
        ///<param name="portalId"></param>
        ///<returns></returns>
        public List<Article> GetAllArticles(int portalId)
        {
            return CBO.FillCollection<Article>(dataProvider.GetAllArticles(portalId));
        }

        ///<summary>Save the article, checks if we are creating new, or updating an existing
        ///</summary>
        ///<param name="a"></param>
        ///<param name="tabId"></param>
        ///<returns></returns>
        public int SaveArticle(Article a, int tabId)
        {
            if (a.ArticleId < 1)
            {
                a.ArticleId = dataProvider.AddArticle(a);

                var objContentItem = this.contentProviderProvider.CreateContentItem(a, tabId);

                a.ContentItemId = objContentItem.ContentItemId;
                SaveArticle(a, tabId);
            }
            else
            {
                dataProvider.UpdateArticle(a);
                this.contentProviderProvider.UpdateContentItem(a, tabId);
            }
            return a.ArticleId;
        }
        
        ///<summary>Delete an article based on ID
        ///</summary>
        ///<param name="articleId"></param>
        public void DeleteArticle(int articleId)
        {
            dataProvider.DeleteArticle(articleId);
        }

        ///<summary>Delete all articles based on a moduleid
        ///</summary>
        ///<param name="moduleId"></param>
        public void DeleteArticles(int moduleId)
        {
            dataProvider.DeleteArticles(moduleId);
        }

        public string GetArticleLink(int tabId, int articleId)
        {
            return globals.NavigateUrl(tabId, String.Empty, "aid=" + articleId);
        }
    }
}