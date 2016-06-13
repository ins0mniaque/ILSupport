using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace ILSupport.Intellisense
{
    internal class LanguageCompletionSource : ICompletionSource
    {
        private LanguageCompletionSourceProvider m_sourceProvider;
        private ITextBuffer m_textBuffer;

        public LanguageCompletionSource ( LanguageCompletionSourceProvider sourceProvider, ITextBuffer textBuffer )
        {
            m_sourceProvider = sourceProvider;
            m_textBuffer     = textBuffer;
        }

        void ICompletionSource.AugmentCompletionSession ( ICompletionSession session, IList < CompletionSet > completionSets )
        {
            var instructionSet = new CompletionSet ( "il.instruction",
                                                     "Instructions",   // TODO: Localize this...
                                                     FindTokenSpanAtPosition ( session.GetTriggerPoint ( m_textBuffer ), session ),
                                                     Instructions,
                                                     null );

            completionSets.Add ( instructionSet );
        }

        private ITrackingSpan FindTokenSpanAtPosition ( ITrackingPoint point, ICompletionSession session )
        {
            var currentPoint = session.TextView.Caret.Position.BufferPosition - 1;
            var navigator    = m_sourceProvider.NavigatorService.GetTextStructureNavigator ( m_textBuffer );
            var extent       = navigator.GetExtentOfWord ( currentPoint );

            return currentPoint.Snapshot.CreateTrackingSpan ( extent.Span, SpanTrackingMode.EdgeInclusive );
        }

        private bool m_isDisposed;
        public void Dispose ( )
        {
            if ( ! m_isDisposed )
            {
                GC.SuppressFinalize ( this );
                m_isDisposed = true;
            }
        }

        private IEnumerable < Completion > instructions = null;
        private IEnumerable < Completion > Instructions
        {
            get { return instructions ?? ( instructions = GenerateInstructions ( ) ); }
        }

        private static IEnumerable < Completion > GenerateInstructions ( )
        {
            foreach ( var word in ILParser.GetWords ( "il.instruction" ).OrderBy ( word => word ) )
                yield return new Completion ( word, word, Descriptions.GetString ( word.ToLowerInvariant ( ) ), null, word );
        }

        private static ResourceManager descriptions = null;
        private static ResourceManager Descriptions
        {
            get
            {
                if ( descriptions == null )
                    descriptions = new ResourceManager ( "ILSupport.Properties.Intellisense",
                                                         typeof ( LanguageCompletionSource ).Assembly );

                return descriptions;
            }
        }
    }
}