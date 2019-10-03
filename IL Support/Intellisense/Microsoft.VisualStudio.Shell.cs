using System;
using System.Windows.Threading;

using EnvDTE;

namespace ILSupport.Intellisense
{
    internal static class VSConstants
    {
        public const int S_OK = 0;

        public static readonly Guid VSStd2K = new Guid ( "{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}" );

        public enum VSStd2KCmdID
        {
            TYPECHAR = 1,
            BACKSPACE,
            RETURN,
            TAB,
            BACKTAB,
            DELETE
        }
    }

    internal static class VsShellUtilities
    {
        public static bool IsInAutomationFunction ( IServiceProvider serviceProvider )
        {
            if ( serviceProvider == null )
                throw new ArgumentException ( "serviceProvider" );

            Dispatcher.CurrentDispatcher.VerifyAccess ( );

            var vsExtensibility = serviceProvider.GetService ( typeof ( IVsExtensibility ) ) as IVsExtensibility;
            if ( vsExtensibility == null )
                throw new InvalidOperationException ( );

            return vsExtensibility.IsInAutomationFunction ( ) != 0;
        }
    }
}