using System.ServiceModel;

namespace PumpService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PumpService : IPumpService
    {
        private readonly IScriptService _scriptService;
        private readonly IStatisticsService _statisticsService;
        private readonly IServiceSettings _serviceSettings;

        public PumpService()
        {
            _statisticsService = new StatisticsService();
            _serviceSettings = new ServiceSettings();
            _scriptService = new ScriptService(_statisticsService, _serviceSettings, Callback);
        }

        public void RunScript()
        {
            _scriptService.Run(10);
        }

        public void UpdateAndCompile(string fileName)
        {
            _serviceSettings.FileName = fileName;
            _scriptService.Compile();
        }

        IPumpServiceCallback Callback
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    return OperationContext.Current.GetCallbackChannel<IPumpServiceCallback>();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
