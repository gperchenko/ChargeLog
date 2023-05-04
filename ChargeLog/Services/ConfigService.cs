using ChargeLog.Models;

namespace ChargeLog.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IConfiguration? _config;

        public ConfigService(IConfiguration config)
        {
            _config = config;
        }

        public InterfaceConfig GetInterfaceConfig()
        {
            var interfaceConfig = new InterfaceConfig();
            _config?.GetSection(InterfaceConfig.InterfaceParams).Bind(interfaceConfig);

            return interfaceConfig;
        }

        public BackendConfig GetBackendConfig()
        {
            var backendConfig = new BackendConfig();
            _config?.GetSection(BackendConfig.BackendParams).Bind(backendConfig);

            return backendConfig;
        }
    }
}
