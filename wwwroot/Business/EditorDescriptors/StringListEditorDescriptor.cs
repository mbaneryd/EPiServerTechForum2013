using System;
using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace EPiServer.Templates.Alloy.Business.EditorDescriptors
{
    /// <summary>
    /// Register an editor for StringList properties
    /// </summary>
    [EditorDescriptorRegistration(TargetType = typeof(String[]), UIHint = Global.SiteUIHints.Strings)]
    public class StringListEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(Shell.ObjectEditing.ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            ClientEditingClass = "alloy/editors/StringList";

            base.ModifyMetadata(metadata, attributes);
        }
    }
}