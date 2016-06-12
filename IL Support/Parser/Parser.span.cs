using System;
using System.Linq;

using Microsoft.VisualStudio.Language.StandardClassification;

namespace ILSupport
{
    public static partial class ILParser
    {
        private class Span
        {
            public Span ( string @class, string start, string end, string escape = null )
            {
                Class  = @class;
                Start  = start;
                End    = end;
                Escape = escape;
            }

            public readonly string Class;
            public readonly string Start;
            public readonly string End;
            public readonly string Escape;
        }

        private static readonly int spanStartMaxLength = spans.Max ( s => s.Start.Length );
        private static Span IdentifySpan ( string text, int position )
        {
            var part = text.Substring ( position, Math.Min ( spanStartMaxLength, text.Length - position ) );
            foreach ( var span in spans )
                if ( part.StartsWith ( span.Start ) )
                    return span;

            return null;
        }

        private static readonly Span [ ] spans = { new Span ( PredefinedClassificationTypeNames.Comment,   "/*",  "*/", null   ),
                                                   new Span ( PredefinedClassificationTypeNames.Comment,   "//",  "\n", null   ),
                                                   new Span ( PredefinedClassificationTypeNames.String,    "@\"", "\"", "\"\"" ),
                                                   new Span ( PredefinedClassificationTypeNames.String,    "\"",  "\"", "\\\"" ),
                                                   new Span ( PredefinedClassificationTypeNames.Character, "'",   "'",  "\\\'" ) };
    }
}