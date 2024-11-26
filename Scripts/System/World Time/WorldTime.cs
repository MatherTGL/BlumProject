using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameAssets.Scripts.Service.Time
{
    public static class WorldTime
    {
        private static readonly HttpClient client = new HttpClient();

        private static DateTime? cachedTime;

        private static DateTime lastFetchTime;

        //TODO сделать возможность получать время без кеша, если это нужно
        private static TimeSpan cacheDuration = TimeSpan.FromSeconds(3); //TODO увеличить минимум до 30 сек

        private static readonly SemaphoreSlim semaphore = new(1, 1);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<DateTime> GetAsync()
        {
            if (cachedTime.HasValue && DateTime.Now - lastFetchTime < cacheDuration)
                return cachedTime.Value;

            await semaphore.WaitAsync();

            try
            {
                var time = await SendRequestWithRetriesAsync();
                return time;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private static IObservable<DateTime> SendRequestWithRetriesAsync()
        {
            return Observable.Defer(() => SendAsync().ToObservable()).Retry(3).Delay(TimeSpan.FromSeconds(2)).Do(
                _ => Debug.Log("Request succeeded"),
                ex => Debug.LogError($"Request failed after retries: {ex.Message}")
            );
        }

        private static async UniTask<DateTime> SendAsync()
        {
            client.Timeout = TimeSpan.FromSeconds(20);

            var tasks = new List<UniTask<HttpResponseMessage>>
            {
                client.GetAsync("http://www.microsoft.com").AsUniTask(),
                client.GetAsync("http://www.google.com").AsUniTask()
            };

            var response = await UniTask.WhenAny(tasks);
            response.result.EnsureSuccessStatusCode();

            if (response.result.Headers.TryGetValues("date", out var values))
            {
                string todaysDate = values.First();

                DateTime extract = DateTime.ParseExact(todaysDate,
                                            "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                            CultureInfo.InvariantCulture.DateTimeFormat,
                                            DateTimeStyles.AssumeUniversal);

                lastFetchTime = extract;
                cachedTime = lastFetchTime;
                return extract;
            }
            else
            {
                return GetLocal();
            }
        }

        private static DateTime GetLocal()
            => DateTime.Now;
    }
}
