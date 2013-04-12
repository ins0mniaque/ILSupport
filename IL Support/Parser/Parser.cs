using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace ILSupport
{
    public static partial class ILParser
    {
        public class Token
        {
            public Token ( string @class, int start, int length )
            {
                Class  = @class;
                Start  = start;
                Length = length;
            }

            public readonly string Class;
            public readonly int    Start;
            public readonly int    Length;
        }

        public static IEnumerable < Token > Parse ( string text )
        {
            if ( text == null || text.Length == 0 )
                yield break;

            var  inWord       = false;
            var  inWhiteSpace = false;
            Span span         = null;

            var length     = text.Length;
            var tokenStart = 0;
            var position   = 0;
            while ( position < length )
            {
                if ( span != null )
                {
                    position = text.IndexOf ( span.End, position );
                    if ( position < 0                           ||
                         span.Escape == null                    ||
                         position + span.Escape.Length > length ||
                         text.Substring ( position, span.Escape.Length ) != span.Escape )
                    {
                        if ( position < 0 ) position = length;
                        else                position += span.End.Length;

                        yield return new Token ( span.Class, tokenStart, position - tokenStart );

                        span = null;
                    }
                    else
                        position += span.Escape.Length;
                }
                else
                {
                    char character   = text [ position ];
                    var isWord       = char.IsLetterOrDigit ( character ) || character == '.' || character == '_';
                    var isWhiteSpace = char.IsWhiteSpace    ( character );

                    if ( inWhiteSpace && ! isWhiteSpace )
                        yield return new Token ( PredefinedClassificationTypeNames.WhiteSpace,
                                                 tokenStart,
                                                 position - 1 - tokenStart );
                    else if ( inWord && ! isWord )
                    {
                        var wordClass = IdentifyWordClass ( text.Substring ( tokenStart, position - tokenStart ) );
                        if ( wordClass != null )
                            yield return new Token ( wordClass, tokenStart, position - tokenStart );
                    }

                    span = IdentifySpan ( text, position );
                    if ( span != null )
                    {
                        tokenStart   = position;
                        inWord       = false;
                        inWhiteSpace = false;
                        position    += span.Start.Length;
                        continue;
                    }

                    if ( ! inWord       && isWord ||
                         ! inWhiteSpace && isWhiteSpace )
                        tokenStart = position;

                    inWord       = isWord;
                    inWhiteSpace = isWhiteSpace;
                    if ( ! inWord && ! inWhiteSpace )
                    {
                        var symbolClass = IdentifySymbolClass ( character );
                        if ( symbolClass != null )
                            yield return new Token ( symbolClass, position, 1 );
                    }

                    position++;
                }
            }

            yield break;
        }
    }
}