namespace LagDaemon.MUD.Core

[<CustomEquality>]
[<CustomComparison>]
type EmailAddress =
    private
    | RawEmailAddress of string
    | ValidEmail of string * Token
    | InvalidEmail of string
    | VerifiedEmailAddress of string
    | FailedEmailAddress of string
    with 
        override x.Equals(o) =
            match o with
            | :? EmailAddress as em ->
                match em, x with
                | RawEmailAddress s1, RawEmailAddress s2 -> s1 = s2
                | ValidEmail (s1,_), ValidEmail (s2,_) -> s1 = s2
                | InvalidEmail s1, InvalidEmail s2 -> s1 = s2
                | VerifiedEmailAddress s1, VerifiedEmailAddress s2 -> s1 = s2
                | FailedEmailAddress s1, FailedEmailAddress s2 -> s1 = s2
                | _,_ -> false
            | _ -> false

        interface System.IComparable with
            member x.CompareTo yobj =
                match yobj with
                | :? EmailAddress as em ->
                    match x, em with
                    | RawEmailAddress s1, RawEmailAddress s2 -> compare s1 s2
                    | ValidEmail (s1,_), ValidEmail (s2,_) -> compare s1 s2
                    | InvalidEmail s1, InvalidEmail s2 -> compare s1 s2
                    | VerifiedEmailAddress s1, VerifiedEmailAddress s2 -> compare s1 s2
                    | FailedEmailAddress s1, FailedEmailAddress s2 -> compare s1 s2
                    | _,_ -> invalidArg "yobj" "cannot compare value of different types"
                | _ -> invalidArg "yobj" "cannot compare value of different types"

            

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module EmailAddress =
    open System
    open System.Text.RegularExpressions
    
    
    let create (str:string) = RawEmailAddress str
    
    
    
    let regex = @"^[a-zA-Z0-9][a-zA-Z0-9._%+-]{0,63}@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,62}[a-zA-Z0-9])?\.){1,8}[a-zA-Z]{2,63}$"
    
    
    let select valid invalid verified failed raw x =
        match x with
        | ValidEmail (s,t) -> valid (s,t)
        | InvalidEmail s -> invalid s
        | VerifiedEmailAddress s -> verified s
        | FailedEmailAddress s -> failed s
        | RawEmailAddress s -> raw s


    let validate em =
        match em with
        | RawEmailAddress s -> 
            match Regex.IsMatch(s, regex) with
            | true -> ValidEmail (s, Token.create ())
            | false -> InvalidEmail s
        | InvalidEmail s -> FailedEmailAddress s
        | ValidEmail _ | VerifiedEmailAddress _ | FailedEmailAddress _ -> em
    
    
    let verify em (tok:Token) =
        match em with
        | ValidEmail (s,t) -> let issued = Token.value t
                              let supplied = Token.value tok
                              if issued = supplied then VerifiedEmailAddress s
                              else FailedEmailAddress s
        | InvalidEmail s -> FailedEmailAddress s
        | RawEmailAddress _ | VerifiedEmailAddress _ | FailedEmailAddress _ -> em
    
    





        





