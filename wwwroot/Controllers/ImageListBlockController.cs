using EPiServer.Core;
using EPiServer.Templates.Alloy.Models.Blocks;
using EPiServer.Templates.Alloy.Models.Media;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace EPiServer.Templates.Alloy.Controllers
{
    public class ImageListBlockController : BlockController<ImageListBlock>
    {
        private readonly IContentLoader _contentLoader;

        public ImageListBlockController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;

            // Add connections between the model and the block
            ViewData.GetEditHints<ImageListModel, ImageListBlock>()
                 .AddFullRefreshFor(x => x.Settings);
        }

        public override ActionResult Index(ImageListBlock currentContent)
        {
            return this.PartialView(this.GetViewName(), new ImageListModel
            {
                Heading = currentContent.Heading,
                Image = currentContent.Image,
                Images = this.LoadImages(currentContent.Settings.Root).Take(currentContent.Settings.MaxCount)
            });
        }

        /// <summary>
        /// Get the view name from the render settings provided
        /// </summary>
        /// <returns></returns>
        protected virtual string GetViewName()
        {
            //Get the RenderSettings from the routedata
            var renderSettings = this.RouteData.Values["renderSettings"] as IDictionary<string, object>;
            if (renderSettings == null || !renderSettings.ContainsKey("tag"))
                return "span12";

            return renderSettings["tag"] as string;
        }

        protected virtual IEnumerable<ImageFile> LoadImages(ContentReference root)
        {
            if (root == null)
                return Enumerable.Empty<ImageFile>();

            return _contentLoader.GetChildren<ImageFile>(root);
        }
    }
    
    public class ImageListModel
    {
        public string Heading { get; set; }
     
        [UIHint(UIHint.Image)]
        public ContentReference Image { get; set; }
        
        public IEnumerable<ImageFile> Images { get; set; }
    }
}