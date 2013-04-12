using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace ILSupport.Text.Classification
{
    [ Export ( typeof ( IClassifierProvider ) ), ContentType ( "il" ) ]
    internal class ILClassifierProvider : IClassifierProvider
    {
        [ Import ]
        internal IClassificationTypeRegistryService classificationTypeRegistry = null;

        private static ILClassifier classifier = null;
        public IClassifier GetClassifier ( ITextBuffer buffer )
        {
            return classifier ?? ( classifier = new ILClassifier ( classificationTypeRegistry ) );
        }
    }
}