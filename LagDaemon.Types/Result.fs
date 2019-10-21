namespace LagDaemon.Types

type Result<'S,'F> =
    private
    | Success of 'S
    | Failure of 'F


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Result =
    

    /// Returns x as a Success x
    [<CompiledName("Succeed")>]
    let succeed x = Success x

    /// Retuns x as a Failure x
    [<CompiledName("Fail")>]
    let fail x = Failure x

    /// calls either success or failure depending on result
    [<CompiledName("Either")>]
    let either success failure result =
        match result with
        | Success s -> success s
        | Failure f -> failure f


    /// binds a result to the input of a Result function
    [<CompiledName("Bind")>]
    let bind f =
        either f fail

    /// bind operator
    let ( >>= ) x f = bind f x

    /// Kleisli bind (compose to Result functions)
    let ( >=> ) f1 f2 =
        f1 >> bind f2

    /// Converts an ordinary function to a Result function
    [<CompiledName("Raise")>]
    let raise f =
        f >> succeed

    /// maps a function to a result function
    [<CompiledName("Map")>]
    let map f =
        either (f >> succeed) fail

    /// Converts 2 Result functions into 1 Result->Result function
    [<CompiledName("Map2")>]
    let map2 success failure =
        either (success >> succeed) (failure >> fail)


    /// Run to Result functions in parallel
    [<CompiledName("Plus")>]
    let plus success failure f1 f2 x =
        match (f1 x), (f2 x) with
        | Success x1, Success x2 -> Success (success x1 x2)
        | Failure m1, Success _ -> Failure m1
        | Success _, Failure m2 -> Failure m2
        | Failure m1, Failure m2 -> Failure (failure f1 f2)

    /// Convers a function returning unit into an identify function while 
    /// carying out the side-effect of f
    [<CompiledName("Tee")>]
    let tee f x = f x; x

    /// Converts a function that throws exceptions to a Result function
    [<CompiledName("TryCatch")>]
    let tryCatch handler f x =
        try
            f x |> succeed
        with
        | ex -> handler ex |> fail









