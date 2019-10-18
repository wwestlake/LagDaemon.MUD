namespace LagDaemon.MUD.UnitTests.Core



module PasswordTests =
    open NUnit.Framework
    open FsCheck
    open FsCheck.NUnit
    open LagDaemon.MUD.Core
    open System

    [<SetUp>]
    let Setup () =
        Arb.register<Generators>() |> ignore
    
    ()


    [<Property>]
    let ``Password Generator Check`` (password:TestValidPassword) =
        printfn "%A" password

        true

