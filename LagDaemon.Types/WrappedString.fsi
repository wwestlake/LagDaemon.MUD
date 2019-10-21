namespace LagDaemon.Types


[<RequireQualifiedAccess>]
module WrappedString =

    type IWrappedString =
        interface
            abstract member Value : string
    end
    val create :
      canonicalize:(string -> 'a) ->
        validate:('a -> Result<'b,string>) ->
          construct:('b -> Result<'c,string>) -> s:string -> Result<'c,string>
    val apply : f:(string -> 'a) -> s:IWrappedString -> 'a
    val value : s:IWrappedString -> string
    val equals : left:IWrappedString -> right:IWrappedString -> bool
    val compareTo : left:IWrappedString -> right:IWrappedString -> int

    
