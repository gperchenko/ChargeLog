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

        public InterfaceConfig GetConfig()
        {
            var interfaceConfig = new InterfaceConfig();
            _config?.GetSection(InterfaceConfig.InterfaceParams).Bind(interfaceConfig);

            return interfaceConfig;
        }
    }
}
