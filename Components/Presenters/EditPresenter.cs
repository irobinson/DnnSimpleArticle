namespace DotNetNuke.Modules.DnnSimpleArticle.Components.Presenters
{
    using System;
    using Models;
    using Views;
    using Web.Mvp;

    public class EditPresenter : ModulePresenter<IEditView, EditModel>
    {
        private readonly ArticleController controller;

        public EditPresenter(IEditView view) : this(view, new ArticleController())
        {
        }

        public EditPresenter(IEditView view, ArticleController controller) : base(view)
        {
            this.controller = controller;
            this.View.Load += Load;
            this.View.SaveClick += Save;
        }

        void Load(object sender, EventArgs eventArgs)
        {
            var qs = Request.QueryString["aid"];
            this.View.Model.Article = qs != null ? new ArticleController().GetArticle(Convert.ToInt32(qs)) : new Article();
        }

        void Save(object sender, SaveClickEventArgs args)
        {
            var article = controller.GetArticle(args.ArticleId);

            article.Title = args.Title;
            article.Description = args.Description;
            article.Body = args.Body;
            article.LastModifiedOnDate = DateTime.Now;
            article.LastModifiedByUserId = this.UserId;
            article.ModuleId = this.ModuleId;

            if (args.Terms != null && args.Terms.Count > 0)
            {
                article.Terms.Clear();
                article.Terms.AddRange(args.Terms);
            }

            controller.SaveArticle(article, this.ModuleContext.PortalId);
            Response.Redirect(Common.Globals.NavigateURL(TabId));
        }
    }
}