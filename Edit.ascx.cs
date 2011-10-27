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

namespace DotNetNuke.Modules.DnnSimpleArticle
{
    using System;
    using Common;
    using Components.Models;
    using Components.Presenters;
    using Components.Views;
    using Services.Exceptions;
    using Web.Mvp;
    using WebFormsMvp;

    [PresenterBinding(typeof(EditPresenter))] 
    public partial class Edit : ModuleView<EditModel>, IEditView
    {
        public event EventHandler<SaveClickEventArgs> SaveClick;

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    var article = this.Model.Article;
                    if (article != null)
                    {
                        txtTitle.Text = article.Title;
                        txtDescription.Text = article.Description;
                        txtBody.Text = article.Body;
                        tsTerms.Terms = article.Terms;
                    }
                    else
                    {
                        tsTerms.Terms = null;
                    }

                    tsTerms.PortalId = this.ModuleContext.PortalId;
                    tsTerms.DataBind();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        
        protected void LbSaveClick(object sender, EventArgs e)
        {
            var args = new SaveClickEventArgs
                           {
                               Title = txtTitle.Text.Trim(),
                               Description = txtDescription.Text.Trim(),
                               Body = txtBody.Text
                           };
            
            var articleId = Request.QueryString["aid"];
            if (articleId != null)
            {
                args.ArticleId = Convert.ToInt32(articleId);
            }

            if (tsTerms.Terms != null && tsTerms.Terms.Count > 0)
            {
                args.Terms.AddRange(tsTerms.Terms);
            }

            if (SaveClick != null)
            {
                SaveClick(this, args);
            }
        }

        protected void LbCancelClick(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(this.ModuleContext.TabId));
        }
    }
}