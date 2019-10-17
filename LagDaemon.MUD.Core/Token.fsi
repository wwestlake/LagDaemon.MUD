namespace LagDaemon.MUD.Core


type Token

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Token =
        
        [<CompiledName("Create")>]
        val create : () -> Token

        
        [<CompiledName("Value")>]
        val value : token:Token -> string


