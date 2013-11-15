using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using EPiServer.Web.WebControls;
using System.Web.UI.WebControls;
using EPiServer.Framework.Localization;
using EPiServer.Editor;
using EPiServer.Framework.Serialization;
using EPiServer.ServiceLocation;
using System.Globalization;
using System.IO;

namespace EPiServer.Templates.Alloy.Business.WebControls
{
    /// <summary>
    /// Wraps renderers that does not implement <see cref="IBlockControl"/> and handles sizing of these
    /// with default values for widths.
    /// </summary>
    public class GenericIBlockControlWrapper : WebControl, IBlockControl
    {
        protected override void CreateChildControls()
        {
            if (InnerControl != null)
            { 
                Controls.Add(InnerControl);
            }
            else if (PageEditing.PageIsInEditMode)
            {
                //There is no renderer for this content. If we're editing, add a notification about this to the editor.
                var message = String.Format(CultureInfo.InvariantCulture,
                        LocalizationService.Service.GetString("/edit/content/missingrenderer"), CurrentData.GetOriginalType().Name);

                if (!string.IsNullOrEmpty(ContentRenderer.Tag))
                {
                    var withTagMessage = String.Format(CultureInfo.InvariantCulture,
                        LocalizationService.Service.GetString("/edit/content/withtag"), ContentRenderer.Tag);
                    message = message + " " + withTagMessage;
                }

                ApplyMissingRendererAttribute();

                Controls.Add(new LiteralControl(message));
            }
        }

        private void ApplyMissingRendererAttribute()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add(RenderSettings.MissingRenderer, "true");

            IObjectSerializer serializer = ObjectSerializerFactory.Service.GetSerializer(KnownContentTypes.Json);
            StringBuilder result = new StringBuilder();
            serializer.Serialize(new StringWriter(result), dictionary);
            Attributes[PageEditing.DataEPiBlockInfo] = result.ToString();
        }

        /// <summary>
        /// Gets or sets the localization service.
        /// </summary>
        /// <value>The localization service.</value>
        protected Injected<LocalizationService> LocalizationService { get; set; }

        /// <summary>
        /// Gets or sets the object serializer to use when serializing to Json.
        /// </summary>
        /// <value>The object serializer.</value>
        protected Injected<IObjectSerializerFactory> ObjectSerializerFactory { get; set; }

        /// <summary>
        /// We don't want to render a tag for the wrapper itself.
        /// </summary>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {}

        /// <summary>
        /// We don't want to render a tag for the wrapper itself.
        /// </summary>
        public override void RenderEndTag(HtmlTextWriter writer)
        {}

        /// <summary>
        /// Gets or sets the inner control that is responsible for the actual rendering of the content.
        /// </summary>
        /// <value>The inner control.</value>
        public Control InnerControl { get; set; }

        /// <summary>
        /// Gets a reference to the server control's parent control in the page control hierarchy.
        /// </summary>
        /// <value></value>
        /// <returns>A reference to the server control's parent control.</returns>
        public ContentRenderer ContentRenderer { get; set; }

        private int _width, _minimumWidth, _maximumWidth;

        /// <summary>
        /// Gets or sets a CSS class for this partial view's rendering container
        /// </summary>
        public virtual string ContainerCssClass { get; set; }

        /// <summary>
        /// Gets or sets the minimum width required by the block control to aid in block rendering
        /// </summary>
        public virtual int MinimumWidth
        {
            get
            {
                return _minimumWidth != 0 ? _minimumWidth : 4; // Default to 4 (one third width)
            }
            set
            {
                _minimumWidth = value;

                this.ThrowIfInvalidMinimumWidth();
            }
        }

        /// <summary>
        /// Gets or sets the maximum width offered by the block control to aid in block rendering
        /// </summary>
        public virtual int MaximumWidth
        {
            get
            {
                return _maximumWidth != 0 ? _maximumWidth : 12; // Default to 12 (full width)
            }
            set
            {
                _maximumWidth = value;

                this.ThrowIfInvalidMaximumWidth();
            }
        }

        /// <summary>
        /// Gets or sets the width of which to render the block control, for example 4 for "span4"
        /// </summary>
        public int Width
        {
            get
            {
                return _width != 0 ? _width : MinimumWidth; // Default to maximum width
            }
            set
            {
                _width = value;

                this.ThrowIfInvalidWidth();
            }
        }


        /// <summary>
        /// Gets or sets the current data.
        /// </summary>
        /// <value>The current data.</value>
        public Core.IContentData CurrentData
        {
            get; set;
        }
    }
}
