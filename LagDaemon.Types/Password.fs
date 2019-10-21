namespace LagDaemon.Types


type Password =
    | RawPassword of string
    | InvalidPassword of string
    | HashedPassword of string


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Password =
    open System

    let upperCase = ['A'..'Z']
    let lowerCase = ['a'..'z']
    let digits    = ['0'..'9']
    let special = "~`!@#$%^&*()_-+={[}]|\\:;\"'<,>.?/" |> Seq.toList

    let requirements = Configuration.config.System.Login.Password

    let checkLength (pw:string) =
        if pw.Length < requirements.MinLength || pw.Length > requirements.MaxLength 
        then false else true

    let private checkRequirement (pw:string) req list =
        pw |> Seq.toList 
        |> List.filter (fun x -> List.contains x list ) 
        |> List.length
        >= req

    let private checkUpperCase (pw:string) =
        checkRequirement pw requirements.MinUpper upperCase

    let private checkDigits (pw:string) =
        checkRequirement pw requirements.MinDigits digits

    let private checkSpecial (pw:string) =
        checkRequirement pw requirements.MinSpecial special

    let private _validate pwstr =
        if checkUpperCase pwstr && checkDigits pwstr && checkSpecial pwstr 
        then HashedPassword <| Hash.Hash pwstr
        else InvalidPassword pwstr

    let validate pw =
        match pw with
        | RawPassword p -> _validate p
        | InvalidPassword p -> pw
        | HashedPassword hp -> pw

    let create password = RawPassword password

    let verify hashed raw =
        match hashed, raw with
        | HashedPassword hash, RawPassword pw -> Hash.verify hash pw
        | _,_ -> false
