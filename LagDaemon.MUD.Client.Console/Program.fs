// Learn more about F# at http://fsharp.org

open System
open System.Linq
open LagDaemon.MUD.Core
open LagDaemon.MongoDB
open LagDaemon.Types
open MongoDB.FSharp
open MongoDB.Bson
open MongoDB.Driver
open MongoDB.Driver.Linq
open Microsoft.FSharp.Linq


[<EntryPoint>]
let main argv =

    //let user = User.Create "Bill" "wwestlake@email.com" "1234!@#$AAbbcc"

    //Result.either (fun (x:User) -> x.Save()) (fun x -> printfn "Failed: %A" x)   user
    
    let user = User.Find (EmailAddress.create "wwestlake@email.com" |> EmailAddress.validate) |> List.head
    let verifiedEmail = user.registeredEmail |> EmailAddress.verify <| Token.createTokenFromString "44BI-JV8T-QF44-FEQ8"

    let update = {
        user
            with registeredEmail = verifiedEmail
    }

    update.Update()

    printfn "%A" user

    0 // return an integer exit code
    