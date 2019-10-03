using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Windows.Threading;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace ILSupport.Intellisense
{
    [ Export ( typeof ( IVsTextViewCreationListener ) ),
      Name ( "Token completion handler" ),
      ContentType ( "il" ),
      TextViewRole ( PredefinedTextViewRoles.Editable ) ]
    internal class LanguageCompletionHandlerProvider : IVsTextViewCreationListener
    {
        [ Import ]
        internal IVsEditorAdaptersFactoryService AdapterService = null;
        [ Import ]
        internal ICompletionBroker CompletionBroker { get; set; }
        [ Import ]
        internal SVsServiceProvider ServiceProvider { get; set; }

        public void VsTextViewCreated ( IVsTextView textViewAdapter )
        {
            var textView = AdapterService.GetWpfTextView ( textViewAdapter );
            if ( textView == null )
                return;

            textView.Properties.GetOrCreateSingletonProperty ( ( ) => new LanguageCompletionCommandHandler ( textViewAdapter, textView, this ) );
        }
    }

    internal class LanguageCompletionCommandHandler : IOleCommandTarget
    {
        private IOleCommandTarget m_nextCommandHandler;
        private ITextView m_textView;
        private LanguageCompletionHandlerProvider m_provider;
        private ICompletionSession m_session;

        internal LanguageCompletionCommandHandler ( IVsTextView textViewAdapter, ITextView textView, LanguageCompletionHandlerProvider provider )
        {
            m_textView = textView;
            m_provider = provider;

            // Add the command to the command chain
            textViewAdapter.AddCommandFilter ( this, out m_nextCommandHandler );
        }

        public int QueryStatus ( ref Guid pguidCmdGroup, uint cCmds, OLECMD [ ] prgCmds, IntPtr pCmdText )
        {
            Dispatcher.CurrentDispatcher.VerifyAccess ( );

            return m_nextCommandHandler.QueryStatus ( ref pguidCmdGroup, cCmds, prgCmds, pCmdText );
        }

        public int Exec ( ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut )
        {
            Dispatcher.CurrentDispatcher.VerifyAccess ( );

            if ( VsShellUtilities.IsInAutomationFunction ( m_provider.ServiceProvider ) )
                return m_nextCommandHandler.Exec ( ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut );

            // Make a copy of this so we can look at it after forwarding some commands
            uint commandID = nCmdID;
            char typedChar = char.MinValue;
            // Make sure the input is a char before getting it
            if ( pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint) VSConstants.VSStd2KCmdID.TYPECHAR )
                typedChar = (char) (ushort) Marshal.GetObjectForNativeVariant ( pvaIn );

            // Check for a commit character
            if ( nCmdID == (uint) VSConstants.VSStd2KCmdID.RETURN ||
                 nCmdID == (uint) VSConstants.VSStd2KCmdID.TAB    ||
                 char.IsWhiteSpace ( typedChar ) )
            {
                // Check for a a selection
                if ( m_session != null && ! m_session.IsDismissed )
                {
                    // If the selection is fully selected, commit the current session
                    if ( m_session.SelectedCompletionSet.SelectionStatus.IsSelected )
                    {
                        m_session.Commit ( );
                        // Also, don't add the character to the buffer
                        return VSConstants.S_OK;
                    }
                    else
                    {
                        // If there is no selection, dismiss the session
                        m_session.Dismiss ( );
                    }
                }
            }

            //pass along the command so the char is added to the buffer 
            int retVal = m_nextCommandHandler.Exec ( ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut );
            bool handled = false;
            if ( ! typedChar.Equals ( char.MinValue ) && ( char.IsLetterOrDigit ( typedChar ) || typedChar == '.' ) )
            {
                if ( m_session == null || m_session.IsDismissed ) // If there is no active session, bring up completion
                {
                    if ( TriggerCompletion ( ) )
                    {
                        if ( m_session != null )
                            m_session.Filter ( );
                    }
                }
                else // The completion session is already active, so just filter
                {
                    m_session.Filter ( );
                }

                handled = true;
            }
            else if ( commandID == (uint) VSConstants.VSStd2KCmdID.BACKSPACE || // Redo the filter if there is a deletion
                      commandID == (uint) VSConstants.VSStd2KCmdID.DELETE )
            {
                if ( m_session != null && ! m_session.IsDismissed )
                    m_session.Filter ( );

                handled = true;
            }

            return handled ? VSConstants.S_OK : retVal;
        }

        private bool TriggerCompletion ( )
        {
            // The caret must be in a non-projection location
            var caretPoint = m_textView.Caret.Position.Point.GetPoint ( textBuffer => ! textBuffer.ContentType.IsOfType ( "projection" ),
                                                                        PositionAffinity.Predecessor );
            if ( ! caretPoint.HasValue )
                return false;

            var trackingPoint = caretPoint.Value.Snapshot.CreateTrackingPoint ( caretPoint.Value.Position, PointTrackingMode.Positive );
            m_session = m_provider.CompletionBroker.CreateCompletionSession ( m_textView, trackingPoint, true );

            // Subscribe to the Dismissed event on the session
            m_session.Dismissed += OnSessionDismissed;

            m_session.Start ( );

            // Wait to start
            while ( ! m_session.IsStarted && ! m_session.IsDismissed )
            {

            }

            return ! m_session.IsStarted && ! m_session.IsDismissed;
        }

        private void OnSessionDismissed ( object sender, EventArgs e )
        {
            m_session.Dismissed -= OnSessionDismissed;
            m_session = null;
        }
    }
}