namespace LagDaemon.MUD.Core




[<RequireQualifiedAccess>]
module Mailbox =

    type Handler<'U,'M> = Handler of ('U -> 'M -> 'U)

    type private Mailbox<'U, 'M> = {
        name: string
        handler: Handler<'U,'M>
        mailbox: MailboxProcessor<'M>
    }
    
    type RegistryMessage =
        | RegisterMailbox of string * MailboxProcessor<RegistryMessage> * AsyncReplyChannel<RegistryMessage>
        | Registered of string

    type private RegistryMap<'U,'M> = RegistryMap of Map<string, Mailbox<'U,'M>>
        
    let private runHandler handler u m = let (Handler h ) = handler in h u m

    let createMailbox<'U,'M> (initState:'U) handler = 
        
        MailboxProcessor<'M>.Start(fun inbox ->
            let rec loop state = async {
                let! msg = inbox.Receive()
                let newState = runHandler handler state msg
                return! loop newState
            }
            loop initState
        )


    let registryHandler (map:Map<string, MailboxProcessor<RegistryMessage>>) (message:RegistryMessage) =
        match message with
        | RegisterMailbox (name,mailbox,reply) ->
            let newState = Map.add name mailbox map
            reply.Reply(Registered name)
            newState
        | Registered _ -> map

    let registry<'U,'M> (name:string) (handler:Handler<'U,'M>) =
        let map = Map.empty
        let registry = createMailbox map handler
        let reply = registry.Post <| RegisterMailbox ("Registry", registry, registry )
        registry

