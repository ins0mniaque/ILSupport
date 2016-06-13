using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace ILSupport.Intellisense
{
    [ Export ( typeof ( ICompletionSourceProvider ) ),
      Name ( "Token completion" ),
      ContentType ( "il" ) ]
    internal class LanguageCompletionSourceProvider : ICompletionSourceProvider
    {
        [ Import ]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public ICompletionSource TryCreateCompletionSource ( ITextBuffer textBuffer )
        {
            return new LanguageCompletionSource ( this, textBuffer );
        }
    }
}