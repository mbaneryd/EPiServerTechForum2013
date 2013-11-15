using EPiServer.Core;
using EPiServer.Web;

namespace EPiServer.Templates.Alloy.Business.WebControls
{
    /// <summary>
    /// Interface to be implemented by block and partial page controls
    /// </summary>
    public interface IBlockControl<T> : IBlockControl, IContentDataControl<T> where T : IContentData
    {

    }

    public interface IBlockControl : IContentDataControl
    {
        /// <summary>
        /// Gets or sets the minimum width required by a block control, such as 12 for "span12" (a full width block control)
        /// </summary>
        int MinimumWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum width allowed for this block control, such as 6 for "span6" (may take up to half the width of a full-sized page)
        /// </summary>
        int MaximumWidth { get; set; }

        /// <summary>
        /// Gets or sets the width of the block control, such as 4 for "span4"
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets CSS classes to use for the rendering container
        /// </summary>
        string ContainerCssClass { get; set; }
    }
}
