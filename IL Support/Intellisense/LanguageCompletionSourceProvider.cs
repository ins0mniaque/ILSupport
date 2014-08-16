using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSupport.Intellisense
{
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("il")]
    [Name("token completion")]

    internal class LanguageCompletionSourceProvider :ICompletionSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }


        public ICompletionSource TryCreateCompletionSource(Microsoft.VisualStudio.Text.ITextBuffer textBuffer)
        {
            return new LanguageCompletionSource(this, textBuffer);
        }
    }
}
