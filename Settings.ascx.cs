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
    using Components;
    using Entities.Modules;
    using Services.Exceptions;
    using Services.Localization;
    using UI.Utilities;

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : ModuleSettingsBase
    {
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    txtPageSize.Text = PageSize.ToString();
                    chkShowCategories.Checked = ShowCategories;

                    ClientAPI.AddButtonConfirm(lnkDeleteAll, Localization.GetString("ConfirmDelete", LocalResourceFile));
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                PageSize = Convert.ToInt32(txtPageSize.Text);
                ShowCategories = Convert.ToBoolean(chkShowCategories.Checked);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void DeleteAllClick(object sender, EventArgs e)
        {
            new ArticleController().DeleteArticles(ModuleId);
        }

        private int PageSize
        {
            get
            {
                return Settings.Contains("PageSize") ? Convert.ToInt32(Settings["PageSize"]) : 10;
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "PageSize", value.ToString());
            }
        }

        private bool ShowCategories
        {
            get
            {
                return !Settings.Contains("ShowCategories") || Convert.ToBoolean(Settings["ShowCategories"]);
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ShowCategories", value.ToString());
            }
        }

        private bool EnableRichDescriptions
        {
            get
            {
                return Settings.Contains("EnableRichDescriptions") && Convert.ToBoolean(Settings["EnableRichDescriptions"]);
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "EnableRichDescriptions", value.ToString());
            }
        }
    }
}