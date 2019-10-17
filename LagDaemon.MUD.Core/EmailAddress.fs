namespace LagDaemon.MUD.Core


type EmailAddress =
    private
    | RawEmailAddress of string
    | ValidEmail of string * Token
    | InvalidEmail of string
    | VerifiedEmailAddress of string
    | FailedEmailAddress of string


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module EmailAddress =
    open System
    open System.Text.RegularExpressions
    
    
    let create (str:string) = RawEmailAddress str
    
    
    
    let regex = @"^[a-zA-Z0-9][a-zA-Z0-9._%+-]{0,63}@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,62}[a-zA-Z0-9])?\.){1,8}[a-zA-Z]{2,63}$"
    
    
    let internal select valid invalid verified failed raw x =
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
    
    





        





