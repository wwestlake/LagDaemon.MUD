namespace LagDaemon.MUD.UnitTests.Core



module EmailTests =

    open LagDaemon.Types
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
    let ``Commutative property of email address equality`` (TestValidEmail email)  =
        let e1 = EmailAddress.create email |> EmailAddress.validate
        let e2 = EmailAddress.create email |> EmailAddress.validate
        printfn "%A --- %A" e1 e2
        e1 = e2 && e2 = e1
    
    [<Property>]
    let ``EmailAddress validates valid email addresses`` (TestValidEmail email) =
        let raw = EmailAddress.create email
        let v = EmailAddress.validate raw
        printfn "%A" v
    
        EmailAddress.select 
            (fun em -> true) 
            (fun _ -> false)
            (fun _ -> false)
            (fun _ -> false)
            (fun _ -> false)
            v
       
    [<Property>]
    let ``EmailAddress verifies token on email with valid token`` (TestValidEmail email) =
        let raw = EmailAddress.create email
        let v = EmailAddress.validate raw

        let verified = v |> EmailAddress.select (fun (_,t) -> 
                                EmailAddress.verify v t
                        ) (fun _ -> v) (fun _ -> v) (fun _ -> v) (fun _ -> v) 

        printfn "%A" verified


        verified |> EmailAddress.select 
                            (fun _ -> false) 
                            (fun _ -> false) 
                            (fun _ -> true)
                            (fun _ -> false)
                            (fun _ -> false)

        

