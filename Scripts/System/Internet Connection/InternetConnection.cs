using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

namespace GameAssets.General
{
    public static class InternetConnection
    {
        public enum DisconnectType : byte
        {
            Internet, Server
        }

        public static Action<DisconnectType> connectionBroken;


        public static async Task<bool> IsSuccessConnection()
            => IsInternetConnection() && IsServerConnection();

        public static bool IsInternetConnection()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply pingReply = ping.Send("8.8.8.8", 3000);
                    return pingReply.Status == IPStatus.Success; //BUG в теории может выпасть false даже при наличии интернета
                }
            }
            catch (TimeoutException ex)
            {
                Debug.Log($"No internet connection! {ex}");
                SetDisconnectInternet();
                return false;
            }
        }

        //TODO доработать соединение с сервером
        public static bool IsServerConnection()
        {
            try
            {
                //await GameServerManager.Api.Account.GetData();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log($"The server is unavailable! {ex}");
                SetDisconnectServer();
                return false;
            }
        }

        //TODO мб стоит добавить bool если уже вызвана панель
        private static void SetDisconnectInternet()
        {
            Debug.LogWarning("SetDisconnectInternet() invoked");
            connectionBroken?.Invoke(DisconnectType.Internet);
        }

        private static void SetDisconnectServer()
        {
            Debug.LogWarning("SetDisconnectServer() invoked.");
            connectionBroken?.Invoke(DisconnectType.Server);
        }
    }
}
