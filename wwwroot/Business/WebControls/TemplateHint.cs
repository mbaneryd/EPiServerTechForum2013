using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace EPiServer.Templates.Alloy.Business.WebControls
{
    /// <summary>
    /// Used to insert a hint or help text for editors in a template
    /// </summary>
    [ToolboxData("<{0}:TemplateHint runat=server></{0}:TemplateHint>")]
    public class TemplateHint : HtmlGenericControl
    {
        // Defaults to a <p> element
        public TemplateHint(string tag) : base(tag.ToLower().Contains("templatehint") || string.IsNullOrWhiteSpace(tag) ? "p" : tag)
        {
            Attributes.Add("class", "alert alert-info");
        }

        public TemplateHint() : this("p")
        {
            
        }
    }
}