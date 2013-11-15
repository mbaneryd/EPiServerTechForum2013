using System;
using System.Web;
using System.Web.UI;

namespace EPiServer.Templates.Alloy.Business.WebControls
{
    // TODO Make it easy to have multiple stylesheets linked when in debug mode, but a single minified one in release mode

    /// <summary>
    /// Used to render a CSS link which links to a minified/non-minified stylesheet depending on site debugging configuration
    /// </summary>
    [ToolboxData("<{0}:StylesheetLink Href='/Static/css/styles.css' runat=\"server\" />")]
    public class StylesheetLink : System.Web.UI.HtmlControls.HtmlLink
    {
        public StylesheetLink()
        {
            Attributes.Add("rel", "stylesheet");
            Attributes.Add("type", "text/css");
        }

        /// <summary>
        /// Gets or sets the path of the non-minified stylesheet
        /// </summary>
        /// <remarks>When debugging is disabled, the Href property returns a path to a minified file version</remarks>
        public override string Href
        {
            get
            {
                return HttpContext.Current.IsDebuggingEnabled
                           ? base.Href
                           : ModifyHref(base.Href);
            }
            set
            {
                base.Href = value;
            }
        }

        /// <summary>
        /// Modifies the Href attribute to append ".min" in front of file extension to link to minified file version
        /// </summary>
        protected virtual string ModifyHref(string originalHref)
        {
            if(!originalHref.EndsWith(".css"))
            {
                throw new ArgumentException("The original Href path must end with '.css'", "originalHref");
            }

            // Inject ".min" before the file extension, unless already present
            var href = originalHref.EndsWith(".min.css") 
                       ? originalHref 
                       : originalHref.Insert(originalHref.IndexOf(".css"), ".min");

            // Ensure leading tilde to make URLs app relative in case a virtual directory is used
            if (href.StartsWith("/"))
            {
                href = href.Insert(0, "~");
            }

            return href.StartsWith("~/") ? Page.ResolveUrl(href) : href;
        }
    }
}