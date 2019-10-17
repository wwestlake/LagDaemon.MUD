// Learn more about F# at http://fsharp.org

open System
open LagDaemon.MUD.Core

[<EntryPoint>]
let main argv =


    let rec loop () =
        let result = io {
            let! _ = putStr "Lobby ->"
            let! cmd = getLine
            let! _ = putStrLn cmd

            return match cmd.Trim().ToLower() <> "quit" with
                    | true -> Result.succeed true
                    | false -> Result.fail "done!"
        }
        match result with
        | Action r -> r |> Result.either 
                                (fun r -> loop () |> ignore; ()) 
                                (fun msg -> printf "%s" msg) 


    loop () |> ignore

    0 // return an integer exit code
