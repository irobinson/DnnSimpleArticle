namespace DotNetNuke.Modules.DnnSimpleArticle.Components.Presenters
{
    using System;
    using Models;
    using Views;
    using Web.Mvp;

    public class EditPresenter : ModulePresenter<IEditView, EditModel>
    {
        private readonly IArticleController controller;
        private readonly IGlobalsHelper globals;

        public EditPresenter(IEditView view) : this(view, new ArticleController(), new GlobalsHelper())
        {
        }

        public EditPresenter(IEditView view, IArticleController controller, IGlobalsHelper globals) : base(view)
        {
            this.controller = controller;
            this.globals = globals;
            this.View.Load += Load;
            this.View.SaveClick += Save;
        }

        void Load(object sender, EventArgs eventArgs)
        {
            Article article;
            var qs = Request.QueryString["aid"];
            if (qs == null)
            {
                this.View.Model.Article = new Article();
                return;
            };

            int articleId;
            if (int.TryParse(qs, out articleId))
            {
                this.View.Model.Article = controller.GetArticle(articleId);
            }
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

            controller.SaveArticle(article, this.PortalId);
            Response.Redirect(globals.NavigateUrl(this.TabId));
        }
    }
}