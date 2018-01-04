using System.Threading.Tasks;
using Stormancer.Plugins;
using Stormancer.Server.Components;

namespace Stormancer.Server.Management
{
    public class Startup
    {
        public void Run(IAppBuilder builder)
        {
            builder.AddPlugin(new ManagementPlugin());
        }
    }
    class ManagementPlugin : IHostPlugin
    {
        public void Build(HostPluginBuildContext ctx)
        {
            ctx.HostDependenciesRegistration += (IDependencyBuilder b) =>
              {
                  b.Register<ManagementClientAccessor>();
              };
           
        }

    }

    public class ManagementClientAccessor
    {
        private readonly IEnvironment _environment;
        public ManagementClientAccessor(IEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<Stormancer.Management.Client.ApplicationClient> GetApplicationClient()
        {
            var infos = await _environment.GetApplicationInfos();

            var result = Stormancer.Management.Client.ApplicationClient.ForApi(infos.AccountId, infos.ApplicationName, infos.PrimaryKey);
            result.Endpoint = infos.ApiEndpoint;
            return result;
        }
    }
}
