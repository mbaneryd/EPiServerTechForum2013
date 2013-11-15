using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPiServer.Templates.Alloy.Models.Blocks
{
    [SiteContentType(GUID = "63A68ACE-94E8-46AD-B5CA-AA356C14F7EB")]
    [SiteImageUrl]
    public class ImageListBlock : SiteBlockData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 1)]
        [Required]
        public virtual string Heading { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 1)]
        [UIHint(UIHint.Image)]
        [Required]
        public virtual ContentReference Image { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 2)]
        public virtual ImageListSettings Settings { get; set; }
    }


    [SiteContentType(GUID = "71DF111A-07EB-427E-856E-F932164F7458", AvailableInEditMode = false)]
    [SiteImageUrl]
    public class ImageListSettings : SiteBlockData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 1)]
        [Required]
        [UIHint("GalleryFolderSelector")]
        public virtual ContentReference Root { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 2)]
        public virtual int MaxCount { get; set; }

        #region IInitializableContent

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            MaxCount = 20;
        }

        #endregion
    }

    [EditorDescriptorRegistration(TargetType = typeof(ContentReference), UIHint = "GalleryFolderSelector")]
    public class GalleryFolderSelector : EditorDescriptor
    {
        public GalleryFolderSelector()
        {
            //Use the Default Content Selector widget
            ClientEditingClass = "epi-cms/widget/ContentSelector";

            //We only want to select folders
            AllowedTypes = new[] {typeof (ContentFolder)};

            //We only want to get the reference back, not the actual folder
            AllowedTypesFormatSuffix = "reference";
        }

        public override void ModifyMetadata(Shell.ObjectEditing.ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);

            // We only want to select folders beneath the Gallery folder (163)
            metadata.EditorConfiguration["roots"] = new[] { new ContentReference(163) };
        }
    }
}