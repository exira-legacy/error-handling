namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Exira.ErrorHandling")>]
[<assembly: AssemblyProductAttribute("Exira.ErrorHandling")>]
[<assembly: AssemblyDescriptionAttribute("F# Railway Oriented Programming Helpers")>]
[<assembly: AssemblyVersionAttribute("0.0.5")>]
[<assembly: AssemblyFileVersionAttribute("0.0.5")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.5"
