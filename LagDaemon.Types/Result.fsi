namespace LagDaemon.Types

type Result<'S,'F>

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Result =


    [<CompiledName("Succeed")>]
    val succeed : x:'a -> Result<'a,'b>

    [<CompiledName("Fail")>]
    val fail : x:'a -> Result<'b,'a>

    [<CompiledName("Either")>]
    val either :
      success:('a -> 'b) -> failure:('c -> 'b) -> result:Result<'a,'c> -> 'b

    [<CompiledName("Bind")>]
    val bind : f:('a -> Result<'b,'c>) -> (Result<'a,'c> -> Result<'b,'c>)
    val ( >>= ) : x:Result<'a,'b> -> f:('a -> Result<'c,'b>) -> Result<'c,'b>
    val ( >=> ) :
      f1:('a -> Result<'b,'c>) ->
        f2:('b -> Result<'d,'c>) -> ('a -> Result<'d,'c>)

    [<CompiledName("Raise")>]
    val raise : f:('a -> 'b) -> ('a -> Result<'b,'c>)

    [<CompiledName("Map")>]
    val map : f:('a -> 'b) -> (Result<'a,'c> -> Result<'b,'c>)

    [<CompiledName("Map2")>]
    val map2 :
      success:('a -> 'b) -> failure:('c -> 'd) -> (Result<'a,'c> -> Result<'b,'d>)

    [<CompiledName("Plus")>]
    val plus :
      success:('a -> 'b -> 'c) ->
        failure:(('d -> Result<'a,'e>) -> ('d -> Result<'b,'e>) -> 'e) ->
          f1:('d -> Result<'a,'e>) ->
            f2:('d -> Result<'b,'e>) -> x:'d -> Result<'c,'e>

    [<CompiledName("Tee")>]
    val tee : f:('a -> unit) -> x:'a -> 'a

    [<CompiledName("TryCatch")>]
    val tryCatch : handler:(exn -> 'a) -> f:('b -> 'c) -> x:'b -> Result<'c,'a>
    

    

