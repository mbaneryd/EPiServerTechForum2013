using System;
using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Templates.Alloy.Models.Media;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;

namespace EPiServer.Templates.Alloy.Controllers
{
    [TemplateDescriptor(
        Default = true,
        Inherited = true,
        AvailableWithoutTag = false,
        TemplateTypeCategory = TemplateTypeCategories.MvcController,
        Tags = new[] { RenderingTags.Edit, RenderingTags.Preview })]
    public class DocumentController : PartialContentController<Document>
    {
        private readonly UrlResolver _urlResolver;
        private readonly SiteDefinition _siteDefinition;

        public DocumentController(UrlResolver urlResolver, SiteDefinition siteDefinition)
        {
            _urlResolver = urlResolver;
            _siteDefinition = siteDefinition;
        }

        /// <summary>
        /// The index action for the image file. Creates the view model and renders the view.
        /// </summary>
        /// <param name="currentContent">The current image file.</param>
        public override ActionResult Index(Document currentContent)
        {  
            //Generate external url
            var uri = new Uri(_siteDefinition.SiteUrl, _urlResolver.GetUrl(currentContent, new VirtualPathArguments
            {
                ContextMode = ContextMode.Default
            }));
            
            return View(new DocumentModel
            {
                Url = uri.ToString(),
                Name = currentContent.Name
            });
        }
    }

    public class DocumentModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }
}