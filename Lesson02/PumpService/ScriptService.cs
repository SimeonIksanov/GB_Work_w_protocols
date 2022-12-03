using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace PumpService
{
    public class ScriptService : IScriptService
    {
        private readonly IStatisticsService _statisticsService;
        private readonly IServiceSettings _serviceSettings;
        private readonly IPumpServiceCallback _callback;
        private CompilerResults _compileResult;


        public ScriptService(
            IStatisticsService statisticsService,
            IServiceSettings serviceSettings,
            IPumpServiceCallback callback)
        {
            _statisticsService = statisticsService;
            _serviceSettings = serviceSettings;
            _callback = callback;
        }

        public bool Compile()
        {
            try
            {
                var compilerParameters = CreateCompilerParameters();
                var scriptContent = ReadScriptContent();

                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                _compileResult = codeProvider
                    .CompileAssemblyFromSource(compilerParameters, scriptContent);

                if (_compileResult.Errors != null && _compileResult.Errors.Count != 0)
                {
                    string compileErrors = string.Join(Environment.NewLine, _compileResult.Errors);
                    // save or send
                    return false;
                }
                return true;
            }
            catch
            {

                return false;
            }
        }

        private string ReadScriptContent()
        {
            string scriptContent;
            using (StreamReader streamReader = new StreamReader(_serviceSettings.FileName))
            {
                scriptContent = streamReader.ReadToEnd();
            }

            return scriptContent;
        }

        private static CompilerParameters CreateCompilerParameters()
        {
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.GenerateInMemory = true;
            compilerParameters.ReferencedAssemblies.AddRange(new string[]
            {
                "System.dll",
                "System.Core.dll",
                "System.Data.dll",
                "Microsoft.CSharp.dll",
                Assembly.GetExecutingAssembly().Location
            });

            return compilerParameters;
        }

        public void Run(int count)
        {
            if (_compileResult == null || (_compileResult != null && _compileResult.Errors != null && _compileResult.Errors.Count > 0))
            {
                if (Compile() == false)
                {
                    return;
                }
            }

            Type t = _compileResult.CompiledAssembly.GetType("Sample.SampleScript");
            if (t == null)
            {
                return;
            }

            MethodInfo entryPointMethod = t.GetMethod("EntryPoint");
            if (entryPointMethod == null)
            {
                return;
            }

            Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    if ((bool)entryPointMethod.Invoke(Activator.CreateInstance(t), null))
                    {
                        _statisticsService.SuccessTacts++;
                    }
                    else
                    {
                        _statisticsService.ErrorTacts++;
                    }

                    _statisticsService.AllTacts++;
                    _callback.UpdateStatistics((StatisticsService)_statisticsService);

                    Thread.Sleep(1000);
                }
            });
        }
    }
}
