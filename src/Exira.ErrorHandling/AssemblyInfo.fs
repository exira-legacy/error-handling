namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Exira.ErrorHandling")>]
[<assembly: AssemblyProductAttribute("Exira.ErrorHandling")>]
[<assembly: AssemblyDescriptionAttribute("F# Railway Oriented Programming Helpers")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
