namespace LagDaemon.MUD.Core



[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]

module configuration =
    open FSharp.Configuration

    type SystemConfiguration = YamlConfig<"MudCoreConfig.yml">

    let config = new SystemConfiguration()


