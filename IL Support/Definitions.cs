using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ILSupport
{
    namespace Text.Classification
    {
        internal static class ILDefinitions
        {
            [ Export, Name ( "il" ), BaseDefinition ( "code" ) ]
            internal static ContentTypeDefinition contentTypeDefinition = null;

            [ Export, FileExtension ( ".il" ), ContentType ( "il" ) ]
            internal static FileExtensionToContentTypeDefinition fileExtensionDefinition = null;

            [ Export, Name ( "il" ) ]
            internal static ClassificationTypeDefinition ilClassificationDefinition = null;

            [ Export, Name ( "il.instruction" ), BaseDefinition ( "il" ) ]
            internal static ClassificationTypeDefinition ilInstructionDefinition = null;

            [ Export ( typeof ( EditorFormatDefinition ) ),
              ClassificationType ( ClassificationTypeNames = "il.instruction" ),
              Name ( "Instruction" ),
              UserVisible ( true ) ]
            internal sealed class ILInstructionFormat : ClassificationFormatDefinition
            {
                public ILInstructionFormat ( ) { ForegroundColor = Colors.Purple; }
            }

            [ Export, Name ( "il.directive" ), BaseDefinition ( "il" ) ]
            internal static ClassificationTypeDefinition ilDirectiveDefinition = null;

            [ Export ( typeof ( EditorFormatDefinition ) ),
              ClassificationType ( ClassificationTypeNames = "il.directive" ),
              Name ( "Directive" ),
              UserVisible ( true ) ]
            internal sealed class ILDirectiveFormat : ClassificationFormatDefinition
            {
                public ILDirectiveFormat ( ) { ForegroundColor = Color.FromRgb ( 105, 105, 105 ); }
            }

            [ Export, Name ( "il.security" ), BaseDefinition ( "il" ) ]
            internal static ClassificationTypeDefinition ilSecurityDefinition = null;

            [ Export ( typeof ( EditorFormatDefinition ) ),
              ClassificationType ( ClassificationTypeNames = "il.security" ),
              Name ( "Directive (Security)" ),
              UserVisible ( true ) ]
            internal sealed class ILSecurityFormat : ClassificationFormatDefinition
            {
                public ILSecurityFormat ( ) { ForegroundColor = Colors.Crimson; }
            }
        }
    }
}