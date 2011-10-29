namespace DnnSimpleArticle.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;
    using DotNetNuke.Entities.Content.Taxonomy;
    using DotNetNuke.Modules.DnnSimpleArticle.Components;
    using DotNetNuke.Modules.DnnSimpleArticle.Components.Models;
    using DotNetNuke.Modules.DnnSimpleArticle.Components.Presenters;
    using DotNetNuke.Modules.DnnSimpleArticle.Components.Views;
    using MbUnit.Framework;
    using Moq;

    [TestFixture]
    public class EditPresenterTests
    {
        private Mock<IEditView> view;
        private Mock<IArticleController> articleController;
        private Mock<HttpContextBase> context;
        private Mock<IGlobalsHelper> globalsHelper;
        private Mock<HttpResponseBase> response;

        [SetUp]
        public void SetUp()
        {
            view = new Mock<IEditView>();
            response = new Mock<HttpResponseBase>();
            globalsHelper = new Mock<IGlobalsHelper>();
            articleController = new Mock<IArticleController>();
            context = new Mock<HttpContextBase>();
            view.Setup(x => x.Model).Returns(new EditModel());
            globalsHelper.Setup(g => g.NavigateUrl(It.IsAny<int>())).Returns("http://url.to/somewhere");
            context.SetupGet(c => c.Response).Returns(response.Object);
        }

        [Test]
        public void ViewLoad_ValidArticleId_SetsArticleModel()
        {
            // arrange
            articleController.Setup(dp => dp.GetArticle(1)).Returns(GetArticle());
            context = GetMockContext(new NameValueCollection { { "aid", "1" } });
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object)
                { HttpContext = context.Object };

            // act
            view.Raise(x => x.Load += null, null, null);

            // assert
            Assert.IsNotNull(view.Object.Model.Article.Title);
        }

        [Test]
        public void ViewLoad_NoArticleId_SetsNewArticleOnModel()
        {
            // arrange
            articleController.Setup(dp => dp.GetArticle(1)).Returns(GetArticle());
            context = GetMockContext(new NameValueCollection());
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object)
                { HttpContext = context.Object };

            // act
            view.Raise(x => x.Load += null, null, null);

            // assert
            Assert.IsNotNull(view.Object.Model.Article);
        }

        [Test]
        public void ViewLoad_InvalidIntegerArticleId_ResultsInNullArticle()
        {
            // arrange
            articleController.Setup(dp => dp.GetArticle(1)).Returns(GetArticle());
            context = GetMockContext(new NameValueCollection { { "aid", "2" } });
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object)
                { HttpContext = context.Object };

            // act
            view.Raise(x => x.Load += null, null, null);

            // assert
            Assert.IsNull(view.Object.Model.Article);
        }

        [Test]
        public void ViewLoad_NonIntegerArticleId_ResultsInNullArticle()
        {
            // arrange
            articleController.Setup(dp => dp.GetArticle(1)).Returns(GetArticle());
            context = GetMockContext(new NameValueCollection { { "aid", "Hammertime" } });
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object)
                { HttpContext = context.Object };

            // act
            view.Raise(x => x.Load += null, null, null);

            // assert
            Assert.IsNull(view.Object.Model.Article);
        }

        [Test]
        [ExpectedException(typeof (NullReferenceException))]
        public void Save_NoArticleId_Throws()
        {
            // arrange
            var saveClickEventArgs = new SaveClickEventArgs();
            context = GetMockContext(new NameValueCollection());
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object)
                { HttpContext = context.Object };

            // act
            view.Raise(x => x.SaveClick += null, saveClickEventArgs);

            // assertion is made in the form of a test method attribute
        }

        [Test]
        [ExpectedException(typeof (NullReferenceException))]
        public void Save_InvalidArticleId_Throws()
        {
            // arrange
            var saveClickEventArgs = new SaveClickEventArgs { ArticleId = -1 };
            context = GetMockContext(new NameValueCollection());
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object)
                { HttpContext = context.Object };

            // act
            view.Raise(x => x.SaveClick += null, saveClickEventArgs);

            // assertion is made in the form of a test method attribute
        }

        [Test]
        public void Save_ValidArticleId_CallsSave()
        {
            // arrange
            var saveClickEventArgs = new SaveClickEventArgs { ArticleId = 1 };
            context = GetMockContext(new NameValueCollection());
            articleController.Setup(dp => dp.GetArticle(1)).Returns(GetArticle());
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object)
                { HttpContext = context.Object };

            // act
            view.Raise(x => x.SaveClick += null, saveClickEventArgs);

            // assert
            articleController.Verify(x => x.SaveArticle(It.IsAny<Article>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void Save_ValidArgs_RedirectsToCleanTabUrl()
        {
            // arrange
            var saveClickEventArgs = new SaveClickEventArgs { ArticleId = 1 };
            context = GetMockContext(new NameValueCollection());
            articleController.Setup(dp => dp.GetArticle(1)).Returns(GetArticle());
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object) { HttpContext = context.Object };

            // act
            view.Raise(x => x.SaveClick += null, saveClickEventArgs);

            // assert
            response.Verify(r => r.Redirect("http://url.to/somewhere"));
        }

        [Test]
        public void Save_ArgsWithTerms_CallsSaveWithTermsOnArticle()
        {
            // arrange
            var term = new Term { TermId = 1, Name = "Awesome", KeyID = 1, Description = "Great"};
            var saveClickEventArgs = new SaveClickEventArgs { ArticleId = 1, Terms = new List<Term>{ term } };
            context = GetMockContext(new NameValueCollection());
            articleController.Setup(dp => dp.GetArticle(1)).Returns(GetArticle());
            new EditPresenter(view.Object, articleController.Object, globalsHelper.Object) { HttpContext = context.Object };

            var article = new Article { ArticleId = 1 };
            article.Terms.AddRange(new List<Term> { term });
            
            // act
            view.Raise(x => x.SaveClick += null, saveClickEventArgs);

            // assert
            articleController.Verify(x => x.SaveArticle(It.Is<Article>(a => a.Terms != null), It.IsAny<int>()), Times.Once());

        }
        
        private Mock<HttpContextBase> GetMockContext(NameValueCollection values)
        {
            context.Setup(x => x.Request.QueryString).Returns(values);
            return context;
        }

        private static Article GetArticle()
        {
            return new Article
                       {
                           ArticleId = 1,
                           ModuleId = 50,
                           Title = "How to Make Enemies and Irritate People",
                           Description = "Lorem Ipsum",
                           Body = "Be Yourself",
                           CreatedByUserId = 1,
                           LastModifiedByUserId = 1,
                           CreatedOnDate = DateTime.Now,
                           LastModifiedOnDate = DateTime.Now
                       };
        }
    }

    public class RedirectException : Exception
    {
    }
}