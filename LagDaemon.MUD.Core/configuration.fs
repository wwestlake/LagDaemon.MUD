namespace LagDaemon.MUD.Core



[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]

module Configuration =
    open FSharp.Configuration

    type SystemConfiguration = YamlConfig<"MudCoreConfig.yml">


    let config = new SystemConfiguration()

