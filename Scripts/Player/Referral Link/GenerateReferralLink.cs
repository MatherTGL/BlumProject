using System;
using FunkySheep.QrCode;
using GameAssets.General.Server;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace GameAssets.Player.Referral
{
    public static class GenerateReferralLink
    {
        private static string botUsername = "";

        private static string userID;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Init()
        {
            await PlayfabController.LoginAsync();

            var request = new GetTitleDataRequest { };
            PlayFabClientAPI.GetTitleData(request,
                result =>
                {
                    if (result.Data != null && result.Data.ContainsKey("BotUsername"))
                    {
                        botUsername = result.Data["BotUsername"];
                        Debug.Log($"botUsername: {botUsername}");
                    }
                },
                error =>
                {
                    Debug.LogException(new Exception(error.ErrorMessage));
                }
            );

            userID = PlayFabSettings.staticPlayer.PlayFabId;
            Debug.Log($"userID: {userID}");
        }

        public static void CopyUserReferralLink()
        {
            WebGLCopyAndPasteAPI.CopyText($"https://t.me/{botUsername}?start={userID}");
            Debug.Log("Copy Referral Link is success");
        }

        public static void ShareReferralLink()
        {
            string telegramUrl = "https://t.me/share/url?url=" + Uri.EscapeUriString($"https://t.me/{botUsername}?start=refID:{userID}");
            //TODO мб стоит сохраниться
            Application.OpenURL(telegramUrl);
        }

        public static Texture2D GetQR(string textForQR)
            => QRManager.Generate(textForQR);
    }
}
