using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameAssets.General.Server;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameAssets.Scripts.Service.Time
{
    public static class WorldTime
    {
        private static DateTime currentTime;
        
        private static DateTime? cachedTime;

        private static DateTime lastFetchTime;

        private static TimeSpan cacheDuration = TimeSpan.FromSeconds(30);

        private static TaskCompletionSource<DateTime> currentTimeTaskCS = null;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<DateTime> GetAsync()
        {
            if (cachedTime.HasValue && DateTime.Now - lastFetchTime < cacheDuration)
                return cachedTime.Value;

            if (currentTimeTaskCS == null)
            {
                await PlayfabController.LoginAsync();
                SendRequestWithRetriesAsync().Subscribe();
            }
            
            currentTime = await currentTimeTaskCS.Task;
            Debug.Log($"Get time: {currentTime}");
            currentTimeTaskCS = null;
            return currentTime;
        }

        private static IObservable<Unit> SendRequestWithRetriesAsync()
        {
            currentTimeTaskCS ??= new();
            
            return Observable.Defer(() => SendAsync()).Retry(5).Delay(TimeSpan.FromSeconds(2)).Do(
                _ => Debug.Log("Request succeeded"),
                ex => Debug.LogError($"Request failed after retries: {ex.Message}")
            );
        }

        private static IObservable<Unit> SendAsync()
        {
            return Observable.Create<Unit>(observer =>
            {
                DateTime time = DateTime.UtcNow;
                
                var request = new ExecuteCloudScriptRequest
                {
                    FunctionName = "getCurrentWorldTime",
                    GeneratePlayStreamEvent = true
                };
                
                PlayFabClientAPI.ExecuteCloudScript(request, result =>
                {
                    if (result.FunctionResult != null)
                    {
                        if (result.FunctionResult is Dictionary<string, object> jsonResult && jsonResult.TryGetValue("currentTime", out var value))
                        {
                            if (DateTime.TryParse(value.ToString(), out time))
                                Debug.Log("Parsed DateTime: " + time);
                        }
                        else
                        {
                            Debug.LogError("Result does not contain currentTime. Repeat...");
                        }

                        currentTimeTaskCS?.SetResult(time);
                        observer.OnCompleted();
                    }
                }, error =>
                {
                    var ex = new Exception(error.ErrorMessage);
                    currentTimeTaskCS.SetException(ex);
                    observer.OnError(ex);
                });
                
                return Disposable.Empty;
            });
        }
    }
}
