using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows.Media;

using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace ILSupport.Intellisense
{
    internal class LanguageCompletionSource : ICompletionSource
    {
        private LanguageCompletionSourceProvider provider;
        private ITextBuffer                      textBuffer;

        public LanguageCompletionSource ( LanguageCompletionSourceProvider provider, ITextBuffer textBuffer )
        {
            this.provider   = provider;
            this.textBuffer = textBuffer;
        }

        void ICompletionSource.AugmentCompletionSession ( ICompletionSession session, IList < CompletionSet > completionSets )
        {
            var instructionSet = new CompletionSet ( "il.instruction",
                                                     "Instructions",   // TODO: Localize this...
                                                     FindTokenSpanAtPosition ( session.GetTriggerPoint ( textBuffer ), session ),
                                                     Instructions,
                                                     null );

            completionSets.Add ( instructionSet );
        }

        private ITrackingSpan FindTokenSpanAtPosition ( ITrackingPoint point, ICompletionSession session )
        {
            var currentPoint = session.TextView.Caret.Position.BufferPosition - 1;
            var navigator    = provider.NavigatorService.GetTextStructureNavigator ( textBuffer );
            var extent       = navigator.GetExtentOfWord ( currentPoint );

            return currentPoint.Snapshot.CreateTrackingSpan ( extent.Span, SpanTrackingMode.EdgeInclusive );
        }

        private bool disposed = false;
        public void Dispose ( )
        {
            if ( ! disposed )
            {
                GC.SuppressFinalize ( this );
                disposed = true;
            }
        }

        private IEnumerable < Completion > instructions = null;
        private IEnumerable < Completion > Instructions
        {
            get { return instructions ?? ( instructions = GenerateInstructions ( ) ); }
        }

        private IEnumerable < Completion > GenerateInstructions ( )
        {
            foreach ( var word in ILParser.GetWords ( "il.instruction" ).OrderBy ( word => word ) )
                yield return new Completion ( word,
                                              word,
                                              Descriptions.GetString ( word.ToLowerInvariant ( ) ),
                                              word.EndsWith ( "." ) ? PrefixInstructionIcon : InstructionIcon,
                                              word );
        }

        private ImageSource instructionIcon = null;
        private ImageSource InstructionIcon
        {
            get
            {
                if ( instructionIcon == null )
                    instructionIcon = provider.GlyphService.GetGlyph ( StandardGlyphGroup.GlyphGroupMethod,
                                                                       StandardGlyphItem .GlyphItemPublic );

                return instructionIcon;
            }
        }

        private ImageSource prefixInstructionIcon = null;
        private ImageSource PrefixInstructionIcon
        {
            get
            {
                if ( prefixInstructionIcon == null )
                    prefixInstructionIcon = provider.GlyphService.GetGlyph ( StandardGlyphGroup.GlyphGroupIntrinsic,
                                                                             StandardGlyphItem .GlyphItemPublic );

                return prefixInstructionIcon;
            }
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