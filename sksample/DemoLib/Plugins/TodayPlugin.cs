using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLib.Plugins
{

    public class TodayPlugin
    {
        private readonly ILogger<TodayPlugin> _logger;
        public TodayPlugin(
            ILogger<TodayPlugin> logger
        ) {
            _logger = logger;
        }

        [KernelFunction("get_date")]
        [Description("本日の日付を取得します")]
        [return: Description("本日の日付を返却します")]
        public string GetDate()
        {
            _logger.LogInformation("プラグイン実行（本日の日付取得）");
            return DateTime.Now.ToString("yyyy/MM/dd");
        }
    }
}
