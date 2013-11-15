using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Shell;

namespace EPiServer.Templates.Alloy.Models.Media
{
    [ContentType(GUID = "E1D862F6-9E8D-4F0C-822F-028FD7FAD33E")]
    [MediaDescriptor(ExtensionString = "docx,pdf,txt,pptx,xlsx")]
    public class Document : MediaData 
    {
        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        public virtual string Copyright { get; set; }
    }

    [UIDescriptorRegistration]
    public class DocumentUIDescriptor : UIDescriptor<Document>
    {
        public DocumentUIDescriptor()
        {
            DefaultView = CmsViewNames.OnPageEditView;
        }
    }
}