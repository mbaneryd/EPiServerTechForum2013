using System;
using System.Web;
using System.Web.UI;
using EPiServer.Web;

namespace EPiServer.Templates.Alloy.Business.WebControls
{
    /// <summary>
    /// Used to render a JavaScript link which links to a minified/non-minified JavaScript file depending on site debugging configuration
    /// </summary>
    [ToolboxData("<{0}:ScriptLink Src='/Static/js/main.js' runat=\"server\" />")]
    public class ScriptLink : System.Web.UI.HtmlControls.HtmlGenericControl
    {
        private string _src = string.Empty;

        public ScriptLink(string tag) : this() { }

        public ScriptLink() : base("script")
        {
            Attributes.Add("type", "text/javascript");
        }

        /// <summary>
        /// Gets or sets the path of the non-minified stylesheet
        /// </summary>
        /// <remarks>When debugging is disabled, the Href property returns a path to a minified file version</remarks>
        public string Src
        {
            get
            {
                return HttpContext.Current.IsDebuggingEnabled
                           ? VirtualPathUtilityEx.ToAbsolute(_src)
                           : ModifySrc(_src);
            }
            set
            {
                _src = value;
            }
        }

        /// <summary>
        /// Modifies the Src attribute to append ".min" in front of file extension to link to minified file version
        /// </summary>
        protected virtual string ModifySrc(string originalSrc)
        {
            if (!originalSrc.EndsWith(".js"))
            {
                throw new ArgumentException("The original Src path must end with '.js'", "originalSrc");
            }

            // Inject ".min" before the file extension, unless already present
            var src = originalSrc.EndsWith(".min.js")
                      ? originalSrc
                      : originalSrc.Insert(originalSrc.Length - ".js".Length, ".min");

            // Ensure leading tilde to make URLs app relative in case a virtual directory is used
            if (src.StartsWith("/"))
            {
                src = src.Insert(0, "~");
            }

            return src.StartsWith("~/") ? Page.ResolveUrl(src) : src;
        }

        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            Attributes.Add("src", Src);

            base.RenderAttributes(writer);
        }
    }
}