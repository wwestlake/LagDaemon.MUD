namespace LagDaemon.MUD.Core


type Token

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Token =
        
        [<CompiledName("Create")>]
        val create : unit -> Token

        
        [<CompiledName("Value")>]
        val value : token:Token -> string


