using DemoLib.Model;
using DemoLib.Services;
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
    public class CalendarPlugin
    {
        private readonly CalendarService _calendarService;
        private readonly ILogger<CalendarPlugin> _logger;

        public CalendarPlugin(
            CalendarService calendarService,
            ILogger<CalendarPlugin> logger
        )
        {
            _calendarService = calendarService;
            _logger = logger;
        }

        [KernelFunction("post_event")]
        [Description("カレンダーに予定を登録します")]
        [return: Description("予定登録の結果を返却します")]
        public async Task<string> PostEvent(
            [Description("予定の構造体")] Event eventdata
        )
        {
            _logger.LogInformation("プラグイン実行（カレンダーの予定登録）");
            var res = await _calendarService.PostEvent(eventdata);
            return res;
        }

    }
}
