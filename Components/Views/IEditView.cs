namespace DotNetNuke.Modules.DnnSimpleArticle.Components.Views
{
    using System;
    using System.Collections.Generic;
    using Entities.Content.Taxonomy;
    using Web.Mvp;

    public interface IEditView : IModuleView<Models.EditModel>
    {
        event EventHandler<SaveClickEventArgs> SaveClick;
    }

    public class SaveClickEventArgs : EventArgs
    {
        public int ArticleId;
        public string Title;
        public string Description;
        public string Body;
        public List<Term> Terms;
    }
}