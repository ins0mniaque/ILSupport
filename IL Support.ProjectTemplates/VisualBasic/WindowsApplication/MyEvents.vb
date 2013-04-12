Namespace My

    'The following events are available for MyApplication
    '
    'Startup: Raised when the application starts, before the startup form is created.
    'Shutdown: Raised after all application forms are closed.  This event is not raised if the application is terminating abnormally.
    'UnhandledException: Raised if the application encounters an unhandled exception.
    'StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    'NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Class MyApplication

#If _MyType = "WindowsForms" Then
        'OnInitialize is used for advanced customization of the My Application Model (MyApplication).
        'Startup code for your specific application should be placed in a Startup event handler.
        <Global.System.Diagnostics.DebuggerStepThrough()> _
        Protected Overrides Function OnInitialize(ByVal commandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean
            'Set the splash screen timeout. 
            Me.MinimumSplashScreenDisplayTime = 2000
            Return MyBase.OnInitialize(commandLineArgs)
        End Function
#End If

    End Class

End Namespace
