namespace LagDaemon.MongoDB


[<RequireQualifiedAccess>]
module Configuration =
    open FSharp.Configuration

    type SystemConfiguration = YamlConfig<"MongoConfig.yml">
    
    let config = new SystemConfiguration()


