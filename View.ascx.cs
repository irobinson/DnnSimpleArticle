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

using System;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;

namespace DotNetNuke.Modules.DnnSimpleArticle
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : DnnSimpleArticleModuleBase, IActionable
    {
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            Load += PageLoad;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// PageLoad runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void PageLoad(object sender, EventArgs e)
        {
            try
            {
                var controlToLoad = "Controls/ArticleList.ascx";
                if (ArticleId > 0)
                {
                    controlToLoad = "Controls/ArticleView.ascx";
                }

                var mbl = (DnnSimpleArticleModuleBase)LoadControl(controlToLoad);
                mbl.ModuleConfiguration = ModuleConfiguration;
                mbl.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
                phViewControl.Controls.Add(mbl);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        ///<summary>
        /// Implementing IActionable
        ///</summary>
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions;
                if (ArticleId > 0)
                {
                    actions = new ModuleActionCollection
                                  {
                                      {
                                          GetNextActionID(), Localization.GetString("EditArticle", LocalResourceFile),
                                          "", "", "", EditUrl(string.Empty, string.Empty, "Edit", "aid=" + ArticleId),
                                          false, SecurityAccessLevel.Edit, true, false
                                          }
                                  };
                }
                else
                {
                    actions = new ModuleActionCollection
                                  {
                                      {
                                          GetNextActionID(), Localization.GetString("AddArticle", LocalResourceFile),
                                          "", "", "", EditUrl(), false, SecurityAccessLevel.Edit, true, false
                                          }
                                  };
                }

                return actions;
            }
        }
    }
}