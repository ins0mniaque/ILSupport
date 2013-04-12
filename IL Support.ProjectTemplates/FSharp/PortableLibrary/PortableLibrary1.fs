// Learn more about F# at http://fsharp.net

module Library1

    open System.Runtime.CompilerServices

    // [<MethodImplAttribute(MethodImplOptions.ForwardRef)>]
    [<MethodImplAttribute(enum<MethodImplOptions> 16)>]
    extern int Square(int number)
