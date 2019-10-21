namespace LagDaemon.Types


type Token =
  | Token of string
  with
    override ToString : unit -> string
  end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Token =
        
        [<CompiledName("Create")>]
        val create : unit -> Token

        
        [<CompiledName("Value")>]
        val value : token:Token -> string

        [<CompiledName("CreateTokenFromString")>]
        val createTokenFromString : s:string -> Token
