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
    using System.Web;
    using System.Xml;
    using Entities.Modules;
    using Services.Search;

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for DnnSimpleArticle
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class FeatureController : IPortable, ISearchable
    {
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="moduleId">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        public string ExportModule(int moduleId)
        {
            string strXml = "";

            List<Article> colDnnSimpleArticles = new ArticleController().GetArticles(moduleId);
            if (colDnnSimpleArticles.Count != 0)
            {
                strXml += "<articles>";

                foreach (Article objArticle in colDnnSimpleArticles)
                {
                    strXml += "<article>";
                    strXml += "<title>" + Common.Utilities.XmlUtils.XMLEncode(objArticle.Title) + "</title>";
                    strXml += "<description>" + Common.Utilities.XmlUtils.XMLEncode(objArticle.Description) + "</description>";
                    strXml += "<body>" + Common.Utilities.XmlUtils.XMLEncode(objArticle.Body) + "</body>";
                    strXml += "</article>";
                }
                strXml += "</articles>";
            }

            return strXml;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="moduleId">The Id of the module to be imported</param>
        /// <param name="content">The content to be imported</param>
        /// <param name="version">The version of the module to be imported</param>
        /// <param name="userId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int moduleId, string content, string version, int userId)
        {
            var mc = new ModuleController();
            var mi = mc.GetModule(moduleId);

            XmlNode xmlDnnSimpleArticles = Common.Globals.GetContent(content, "articles");


            if (xmlDnnSimpleArticles != null)
// ReSharper disable PossibleNullReferenceException
                foreach (XmlNode xmlDnnSimpleArticle in xmlDnnSimpleArticles.SelectNodes("article"))
// ReSharper restore PossibleNullReferenceException
                {
                    var objDnnSimpleArticle = new Article
                                                  {
                                                      ModuleId = moduleId,
// ReSharper disable PossibleNullReferenceException
                                                      Title = xmlDnnSimpleArticle.SelectSingleNode("title").InnerText,
// ReSharper restore PossibleNullReferenceException
// ReSharper disable PossibleNullReferenceException
                                                      Description = xmlDnnSimpleArticle.SelectSingleNode("description").InnerText,
// ReSharper restore PossibleNullReferenceException
// ReSharper disable PossibleNullReferenceException
                                                      Body = xmlDnnSimpleArticle.SelectSingleNode("body").InnerText,
// ReSharper restore PossibleNullReferenceException
                                                      CreatedByUserId = userId,
                                                      CreatedOnDate = DateTime.Now,
                                                      LastModifiedByUserId = userId,
                                                      LastModifiedOnDate = DateTime.Now
                                                  };

                    objDnnSimpleArticle.Save(mi.TabID);
                }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="modInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            var searchItemCollection = new SearchItemInfoCollection();

            List<Article> colArticles = new ArticleController().GetArticles(modInfo.ModuleID);

            foreach (Article objArticle in colArticles)
            {
                var searchItem = new SearchItemInfo(objArticle.Title, Common.Utilities.HtmlUtils.StripTags(HttpUtility.HtmlDecode(objArticle.Description),false), objArticle.CreatedByUserID, objArticle.LastModifiedOnDate, modInfo.ModuleID, objArticle.ArticleId.ToString(), Common.Utilities.HtmlUtils.StripTags(HttpUtility.HtmlDecode(objArticle.Body),false), "aid=" + objArticle.ArticleId);
                searchItemCollection.Add(searchItem);
            }

            return searchItemCollection;
        }
    }
}