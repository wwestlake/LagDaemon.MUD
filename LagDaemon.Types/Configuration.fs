
namespace LagDaemon.Types



[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]

module Configuration =
    open FSharp.Configuration

    type SystemConfiguration = YamlConfig<"TypesConfig.yml">


    let config = new SystemConfiguration()




