using System;
using PumpClient.PumpServiceReference;

namespace PumpClient
{
    internal class CallbackHandler : IPumpServiceCallback
    {
        public void UpdateStatistics(StatisticsService statistics)
        {
            Console.Clear();
            Console.WriteLine(
                $"Обновление по статистике выполнения скрипта{Environment.NewLine}" +
                $"Всего     тактов: {statistics.AllTacts}{Environment.NewLine}" +
                $"Успешных  тактов: {statistics.SuccessTacts}{Environment.NewLine}" +
                $"Ошибочных тактов: {statistics.ErrorTacts}{Environment.NewLine}");
        }
    }
}
