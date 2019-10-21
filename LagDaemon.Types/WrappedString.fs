namespace LagDaemon.Types


[<RequireQualifiedAccess>]
module WrappedString =

    type IWrappedString =
        abstract Value : string


    let create canonicalize validate construct (s:string) =
        if s = null 
        then Result.fail "String may not be null"
        else
            let s' = s |> canonicalize |> validate
            Result.either (construct) (sprintf "string '%s' is invalid" >> Result.fail) s'

    let apply f (s:IWrappedString) =
        s.Value |> f

    let value s = apply id s

    let equals left right =
        (value left) = (value right)

    let compareTo left right =
        (value left).CompareTo (value right)

