namespace LagDaemon.MUD.Core

open System
open LagDaemon.Types
open LagDaemon.MongoDB

type User = {
    Id: Guid
    name: string
    registeredEmail: EmailAddress
    email: string
    password: Password
}




    type User 
        with
            static member Find (email:EmailAddress) =
                let em = email.Value
                let db = MongoDB.MongoCollection<User>("User")
                db.Find(fun x -> x.email = em)

            static member Create name email password =
                let em = EmailAddress.create email |> EmailAddress.validate
                let pw = Password.create password |> Password.validate
                match em, pw with
                | InvalidEmail _, InvalidPassword _ -> Result.fail ["Invalid email address"; "Invalid password"]
                | _, InvalidPassword _ -> Result.fail ["Invalid password"]
                | InvalidEmail _, _ -> Result.fail ["Invalid email address"]
                | ValidEmail (_,_), HashedPassword _ ->
                    Result.succeed {
                        Id = Guid.NewGuid()
                        name = name
                        registeredEmail = em
                        password = pw
                        email = email
                    }
                | _,_ -> Result.fail ["Unknown user registration error"]

            member x.Save () =
                let db = MongoDB.MongoCollection<User>("User")
                db.Create(x)

            member x.Update() =
                let db = MongoDB.MongoCollection<User>("User")
                db.Replace(x)

            
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module User =
    
    open LagDaemon.MongoDB

