using ChargeLog.Models;

namespace ChargeLog.Services
{
    public interface IConfigService
    {
        InterfaceConfig GetInterfaceConfig();
        BackendConfig GetBackendConfig();
    }
}
