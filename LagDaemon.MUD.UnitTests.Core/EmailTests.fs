namespace LagDaemon.MUD.UnitTests.Core



module EmailTests =

    open NUnit.Framework
    open FsCheck
    open FsCheck.NUnit
    open LagDaemon.MUD.Core
    open EmailTestData
    open System
    
    
    
    [<SetUp>]
    let Setup () =
        Arb.register<Generators>() |> ignore
        
        ()
    
    [<Test>]
    let ``EmailAddress module creates araw email address`` () = 
        let email = EmailAddress.create "this.is.a.valid@email.address.com"
        match email with
        | RawEmailAddress _ -> Assert.Pass()
        | _ -> Assert.Fail()
    
    [<Property>]
    let ``Commutative property of email address equality`` s1  =
        let e1 = EmailAddress.create s1 |> EmailAddress.validate
        let e2 = EmailAddress.create s1 |> EmailAddress.validate
        printfn "%A --- %A" e1 e2
        e1 = e2 && e2 = e1
    
    [<Property>]
    let ``EmailAddress validates valid email addresses`` (e: EmailAddress) =
        let v = EmailAddress.validate e
        printfn "%A" v
    
        match v with
        | ValidEmailAddress _ -> true
        | _ -> false
    
