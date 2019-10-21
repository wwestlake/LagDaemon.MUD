namespace LagDaemon.Types

type Token = Token of string
    with override x.ToString() = let (Token value) = x in value



[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Token =

    open System


    let private rand = new Random(int DateTime.Now.Ticks)

    [<CompiledName("Create")>]
    let create () =
        let toUpper (x:string) = x.ToUpper()
        let rand = new Random(int DateTime.Now.Ticks)
        let tokenVals = ['a'..'z'] @ ['A'..'Z'] @ ['0'..'9']
        let max = List.length tokenVals - 1
        let rec make acc count =
            if count > 0 then make (tokenVals.[rand.Next(max)] :: acc) (count - 1)
            else acc
        (make [] 4) @ ['-'] @ (make [] 4) @ ['-'] @ (make [] 4) @ ['-'] @ (make [] 4)
        |> List.toArray |> String.Concat |> toUpper |> Token

    [<CompiledName("CreateTokenFromString")>]
    let createTokenFromString s = Token s

    [<CompiledName("Value")>]
    let value token = let (Token v) = token in v
        
