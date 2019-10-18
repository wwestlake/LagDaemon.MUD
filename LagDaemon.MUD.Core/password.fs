namespace LagDaemon.MUD.Core


type Password =
    private
    | RawPassword of string
    | InvalidPassword of string
    | HashedPassword of string



[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module password =
    open System

    let create password pw = RawPassword pw



