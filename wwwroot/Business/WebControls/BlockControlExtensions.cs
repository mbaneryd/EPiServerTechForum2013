using System;
using System.Collections.Generic;
using System.Web.UI;
using EPiServer.Core;
using EPiServer.Web.PropertyControls;
using EPiServer.Web.WebControls;

namespace EPiServer.Templates.Alloy.Business.WebControls
{
    /// <summary>
    /// Provides extension methods for classes implementing the <see cref="IBlockControl"/> interface, such as block and partial page controls
    /// </summary>
    public static class BlockControlExtensions
    {
        /// <summary>
        /// Throws an exception if the block control's MinimumWidth property value is invalid
        /// </summary>
        /// <param name="blockControl"></param>
        public static void ThrowIfInvalidMinimumWidth(this IBlockControl blockControl)
        {
            if (blockControl.MinimumWidth < 4 || blockControl.MinimumWidth > 12)
            {
                throw new InvalidPropertyValueException("MinimumWidth must be set to a value between 4 and 12 and be divisible by 2");
            }
        }

        /// <summary>
        /// Throws an exception if the block control's MaximumWidth property value is invalid
        /// </summary>
        /// <param name="blockControl"></param>
        public static void ThrowIfInvalidMaximumWidth(this IBlockControl blockControl)
        {
            if (blockControl.MaximumWidth < 4 || blockControl.MaximumWidth > 12)
            {
                throw new InvalidPropertyValueException("MaximumWidth must be set to a value between 4 and 12 and be divisible by 2");
            }
        }

        /// <summary>
        /// Throws an exception if the block control's Width property value is invalid
        /// </summary>
        /// <param name="blockControl"></param>
        public static void ThrowIfInvalidWidth(this IBlockControl blockControl)
        {
            if (blockControl.Width < blockControl.MinimumWidth || blockControl.Width > blockControl.MaximumWidth)
            {
                throw new InvalidPropertyValueException(string.Format("Width must be set to a value between {0} and {1}",
                    blockControl.MinimumWidth, 
                    blockControl.MaximumWidth));
            }
        }


        /// <summary>
        /// Gets the number of blocks or partial pages rendered inside a content area
        /// </summary>
        public static int NumberOfBlocks(this IBlockControl contentDataControl)
        {
            var contentAreaControl = contentDataControl.ContentAreaControl();

            // Not rendered inside a content area
            if (contentAreaControl == null || contentAreaControl.PropertyData == null)
            {
                return 0;
            }

            var contentArea = contentAreaControl.PropertyData.Value as ContentArea;

            return contentArea != null ? contentArea.Count : 0;
        }

        /// <summary>
        /// Gets the content area control in which this partial view is rendered
        /// </summary>
        /// <returns>The PropertyContentAreaControl in which this control is rendered, otherwise null</returns>
        public static PropertyContentAreaControl ContentAreaControl(this IBlockControl contentDataControl)
        {
            var ctrl = contentDataControl as Control;

            if (ctrl != null && ctrl.Parent != null && ctrl.Parent.BindingContainer != null)
            {
                return ctrl.Parent.BindingContainer as PropertyContentAreaControl;
            }

            return null;
        }

        /// <summary>
        /// Gets common CSS classes for a block or partial page when rendered inside a content area
        /// </summary>
        /// <param name="contentDataControl"></param>
        /// <returns></returns>
        /// <remarks>The CSS class names and rules derive from the Bootstrap HTML framework</remarks>
        public static string GetBlockContainerCssClasses(this IBlockControl contentDataControl)
        {
            /* Block styling is based on a combination of a Bootstrap CSS class and a site styling CSS class.
             * The site styling CSS class name can be retrieved using the GetCssClassForTag method.
             * No site styling CSS class is needed for the default block size, ie 'span4' */

            var blockTypeName = contentDataControl.CurrentData is PageData
                                ? ((PageData) contentDataControl.CurrentData).PageTypeName.ToLower().Replace("proxy", string.Empty) // The 'proxy' suffix is added to type names at runtime
                                : contentDataControl.CurrentData.GetType().Name.ToLower().Replace("proxy", string.Empty);

            var cssClasses = new List<string>(3)
            { 
                "block", // Common for all blocks and partial page views
                blockTypeName, // For styling based on the block type
                string.Format("span{0}", contentDataControl.Width), // Sets a size CSS class based on Bootstrap
                contentDataControl.ContainerCssClass // Any CSS classes specified explicitly by the block control
            };
            
            // Add additional styling CSS class if required
            switch (contentDataControl.Width)
            {
                case Global.ContentAreaWidths.HalfWidth:
                    cssClasses.Add(GetCssClassForTag(Global.ContentAreaTags.HalfWidth));
                    break;
                case Global.ContentAreaWidths.TwoThirdsWidth:
                    cssClasses.Add(GetCssClassForTag(Global.ContentAreaTags.TwoThirdsWidth));
                    break;
                case Global.ContentAreaWidths.FullWidth:
                    cssClasses.Add(GetCssClassForTag(Global.ContentAreaTags.FullWidth));
                    break;
            }

            return string.Join(" ", cssClasses).Trim();
        }

        /// <summary>
        /// Adds a CSS class to the parent content control
        /// </summary>
        /// <param name="contentDataControl"></param>
        /// <param name="cssClass"></param>
        /// <remarks>Equivalent of setting the ChildrenCssClass render setting for a Property control rendering a ContentArea, but only affects the block container for this particular block instance</remarks>
        public static void AddParentControlControlCssClass(this IBlockControl contentDataControl, string cssClass)
        {
            if (string.IsNullOrWhiteSpace(cssClass))
            {
                return;
            }

            var control = contentDataControl as Control;

            if (control == null)
            {
                throw new NotSupportedException("The specified IContentDataControl was not a Control");
            }

            var parent = control.Parent as ContentRenderer;

            if (parent == null)
            {
                return;
            }

            parent.CssClass = string.Join(" ", parent.CssClass, cssClass).Trim();
        }

        /// <summary>
        /// Gets a CSS class used for styling based on a tag name (ie a Bootstrap class name)
        /// </summary>
        /// <param name="tagName">Any tag name available, see <see cref="Global.ContentAreaTags"/></param>
        /// <returns></returns>
        private static string GetCssClassForTag(string tagName)
        {
            switch (tagName.ToLower())
            {
                case "span12":
                    return "full";
                case "span8":
                    return "wide";
                case "span6":
                    return "half";
                default:
                    return string.Empty;
            }
        }
    }
}