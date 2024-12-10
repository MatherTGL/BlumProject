using Telegram.WebApp.Services;

namespace GameAssets.General.Server
{
    public static class TelegramManager 
    {
        public static WebAppService webAppService { get; private set; }

        public static void Init()
        {
            webAppService = new WebAppService();
            webAppService.Initialize();
        }
    }
}
