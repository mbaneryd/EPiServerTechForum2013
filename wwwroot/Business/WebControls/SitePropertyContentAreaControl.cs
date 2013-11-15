using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.Templates.Alloy.Views.Blocks;
using EPiServer.Web.PropertyControls;
using EPiServer.Web.WebControls;
using log4net;

namespace EPiServer.Templates.Alloy.Business.WebControls
{
    /// <summary>
    /// Provides custom rendering of content areas to include automatic rows based on the Bootstrap HTML framework
    /// </summary>
    public class SitePropertyContentAreaControl : PropertyContentAreaControl
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof (SitePropertyContentAreaControl));
        private int _rowWidth;

        /// <summary>
        /// Applies CSS classes to each block rendering container
        /// </summary>
        /// <param name="contentRendererGroups"></param>
        private void SetCssClasses(List<List<ContentRenderer>> contentRendererGroups)
        {
            foreach (var group in contentRendererGroups)
            {
                foreach (var contentRenderer in group)
                {
                    var blockControl = contentRenderer.CurrentControl as IBlockControl;

                    if (blockControl == null)
                    {
                        throw new NotSupportedException("Only block controls implementing the IBlockControl interface are supported");
                    }

                    // Add container CSS classes defined by the block control
                    contentRenderer.CssClass = blockControl.GetBlockContainerCssClasses();
                }
            }
        }

        /// <summary>
        /// Composes groups where each group represents a Bootstrap row of blocks
        /// </summary>
        /// <param name="contentRenderers"></param>
        /// <returns></returns>
        private List<List<ContentRenderer>> GetBlockGroups(IList<ContentRenderer> contentRenderers)
        {
            var groups = new List<List<ContentRenderer>>();

            var group = new List<ContentRenderer>();

            var minimumWidthOfBlockControlsAddedToCurrentGroup = 0;

            for (int i = 0; i < contentRenderers.Count; i++)
            {
                var contentRenderer = contentRenderers[i];

                // Add block controls to the current group

                var blockControl = GetIBlockControl(contentRenderer);

                // There is no point in showing the default block control in preview mode, or if the current user isn't an editor
                if (blockControl is DefaultBlockControl)
                {
                    if (Page is BlockPreview || !CurrentPage.QueryDistinctAccess(AccessLevel.Edit))
                    {
                        continue;
                    }
                }

                if (blockControl.MinimumWidth > RowWidth)
                {
                    _logger.WarnFormat("Block control '{0}' has a minimum width of {1} and won't properly fit within the maximum row width which is {2}", blockControl.GetType().Name, blockControl.MinimumWidth, RowWidth);

                    if (Page is BlockPreview) // In block preview mode we skip block controls that won't fit within the current row width
                    {
                        continue;
                    }
                }

                blockControl.Width = blockControl.MinimumWidth;

                group.Add(contentRenderer);

                minimumWidthOfBlockControlsAddedToCurrentGroup += blockControl.MinimumWidth;

                if (i == contentRenderers.Count - 1) // The last block has been added, so we're done creating groups
                {
                    // Fill out the last row
                    AdjustWidthOfBlocksToFillRow(group);
                    groups.Add(group);
                    continue;
                }

                var nextContentRenderer = contentRenderers[i + 1];

                if (nextContentRenderer != null)
                {
                    var nextBlockControl = GetIBlockControl(nextContentRenderer);

                    if (minimumWidthOfBlockControlsAddedToCurrentGroup + nextBlockControl.MinimumWidth <= RowWidth)
                    {
                        continue;
                    }

                    // The next block won't fit in this group, try to fill the group by increasing the size of already added blocks
                    AdjustWidthOfBlocksToFillRow(group);
                }

                groups.Add(group);

                group = new List<ContentRenderer>();

                minimumWidthOfBlockControlsAddedToCurrentGroup = 0;
            }

            if (group.Count > 0)
            {
                //If we somehow escaped the loop without adding the last group lets do it now.
                AdjustWidthOfBlocksToFillRow(group);
                groups.Add(group);
            }

            return groups;
        }

        /// <summary>
        /// Gets the control responsible for rendering content and tries to cast it to <see cref="IBlockControl"/>.
        /// If the control does not implement this <see cref="IBlockControl"/> we create a wrapper that handles this for us.
        /// </summary>
        private static IBlockControl GetIBlockControl(ContentRenderer contentRenderer)
        {
            var blockControl = contentRenderer.CurrentControl as IBlockControl;

            if (blockControl != null)
            {
                return blockControl;
            }

            var genericIBlockControlWrapper = new GenericIBlockControlWrapper();

            genericIBlockControlWrapper.InnerControl = contentRenderer.CurrentControl;
            genericIBlockControlWrapper.CurrentData = contentRenderer.CurrentData;
            genericIBlockControlWrapper.ContentRenderer = contentRenderer;
            contentRenderer.CurrentControl = genericIBlockControlWrapper;

            return genericIBlockControlWrapper;            
        }

        /// <summary>
        /// Loops through block controls and expands their width until they fill an entire row
        /// </summary>
        /// <param name="group"></param>
        private void AdjustWidthOfBlocksToFillRow(IList<ContentRenderer> group)
        {
            while (RowWidth - group.Sum(b => ((IBlockControl)b.CurrentControl).Width) > 0) // While there is space left
            {
                foreach (var block in group.Select(g => g.CurrentControl).Cast<IBlockControl>()) // Increase size of blocks in the row
                {
                    if (block.Width + 1 <= block.MaximumWidth) // Do not exceed block maximum size
                    {
                        block.Width++;
                    }

                    if (RowWidth - group.Sum(b => ((IBlockControl)b.CurrentControl).Width) == 0)
                    {
                        // Row is filled

                        break;
                    }
                }
            }   
        }
        
        /// <summary>
        /// Creates block controls for the blocks in the block area. Used when the property is in view mode or in "on page edit"
        /// mode and the PropertyDataControl does not support on page editing.
        /// </summary>
        public override void CreateDefaultControls()
        {
            CreateContentAreaControls(false);
        }

        /// <summary>
        /// Creates the "on page edit" controls with the blocks. If no block exist, this method will do nothing.
        /// </summary>
        public override void CreateOnPageEditControls()
        {
            CreateContentAreaControls(PropertyIsEditableForCurrentLanguage());
        }

        private void CreateContentAreaControls(bool enableEditFeatures)
        {
            // Get list of block renderers

            var controlList = GetContentRenderers(EnableEditFeaturesForChildren || enableEditFeatures);

            Control container = this;

            if (enableEditFeatures)
            {
                container = CreateMainContainer(enableEditFeatures, "div");
                
                // Remove any existing 'class' attribute to avoid duplicates between the container and the block rows
                ((HtmlGenericControl)container).Attributes.Remove("class");

                // We need to be able to style block containers in edit mode for overlays to handle when an editor has floated images inside editorial content
                ((HtmlGenericControl)container).Attributes.Add("class","clearfix");
            }

            var blockGroups = GetBlockGroups(controlList.OfType<ContentRenderer>().ToList());

            SetCssClasses(blockGroups);

            var containerTagName = !String.IsNullOrEmpty(CustomTagName) ? CustomTagName : "div";

            // Each block group is rendered in a separate container
            foreach (var blockGroup in blockGroups.Where(g => g.Count > 0))
            {
                var rowContainer = new HtmlGenericControl(containerTagName);

                CopyWebAttributes(rowContainer);

                // The layout property shouldn't be rendered
                rowContainer.Attributes.Remove("layout");

                container.Controls.Add(rowContainer);

                foreach (var block in blockGroup)
                {
                    rowContainer.Controls.Add(block);

                    block.EnsureChildControlsCreated();
                }
            }
        }


        /// <summary>
        /// Gets the width (in Bootstrap units) of each row being rendered
        /// </summary>
        /// <remarks>Based on any render setting supplied, either through the Layout property or through the Tag render setting</remarks>
        protected virtual int RowWidth
        {
            get
            {
                if (_rowWidth > 0)
                {
                    return _rowWidth;
                }

                // Determine row width based on Tag, otherwise default to full width
                var renderSettingsTag = RenderSettings.ContainsKey("tag")
                                        ? RenderSettings["tag"] as string ?? Global.ContentAreaTags.FullWidth
                                        : Global.ContentAreaTags.FullWidth;

                // If the tag starts with "span*" we try to parse the rows width from the tag value

                _rowWidth = Global.ContentAreaWidths.FullWidth;

                if (renderSettingsTag.StartsWith("span", StringComparison.OrdinalIgnoreCase) && renderSettingsTag.Length > "span".Length)
                {
                    if (!int.TryParse(renderSettingsTag.Substring("span".Length), out _rowWidth))
                    {
                        return Global.ContentAreaWidths.FullWidth;
                    }
                }

                return _rowWidth;
            }
        }

        /// <remarks>The default implementation simply checks the IsNull property</remarks>
        protected override bool ShouldCreateDefaultControls()
        {
            return PropertyData != null && !PropertyData.IsNull;
        }
    }
}
