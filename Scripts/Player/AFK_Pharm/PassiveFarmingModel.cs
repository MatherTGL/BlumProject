using System;
using System.Collections;
using System.Threading.Tasks;
using GameAssets.Player.Data;
using GameAssets.Scripts.Service.Time;
using GameAssets.System.GameEvents;
using GameAssets.System.SaveSystem;
using GameSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameAssets.Player.Pharm
{
    public sealed class PassiveFarmingModel : IPassiveModel
    {
        private readonly PassiveFarmingController controller;

        private ConfigPassiveFarming data;

        public Action UpdatedData { get; set; }

        private DateTime newGameTime;

        private TimeSpan pharmTime;


        public PassiveFarmingModel(in PassiveFarmingController controller)
        {
            this.controller = controller;
            Init();
        }

        private async void Init()
        {
            newGameTime = await WorldTime.GetAsync();
            data = await Addressables.LoadAssetAsync<ConfigPassiveFarming>("PassiveFarming").Task;
            await ((ISaveable<ConfigPassiveFarming>)data).AsyncLoad();
            
            UpdatedData += SaveData;
            GameEventsController.endGame += EndGame;

            InitTime();
        }

        private void InitTime()
        {
            if ((data.lastAddedTime.HasValue && data.startPharmTime.HasValue) == false)
                return;

            CalcAFKPharmMoney();

            if ((data.lastAddedTime.Value.Minute - data.startPharmTime.Value.Minute) >= data.maxPharmTime)
            {
                data.isActivePharm = false;
                AccrueMoney();
            }

            if (data.isActivePharm)
                CoroutineManager.Instance.StartCoroutine(PharmRoutine());
        }

        private async void CalcAFKPharmMoney()
            => data.currentPharmMoney += Mathf.Abs(await GetAFKTimeDifferenceAsync()) * data.incomeInTick;

        private void EndGame() => SaveData();

        private void SaveData()
            => ((ISaveable<ConfigPassiveFarming>)data).Save();

        private IEnumerator PharmRoutine()
        {
            SetStartAndEndPharmTime();

            while (data.isActivePharm)
            {
                yield return new WaitForSecondsRealtime(data.pharmTick);
                UpdatePharmTime();

                //TODO заменить минуты на часы
                if (pharmTime.TotalMinutes >= data.maxPharmTime)
                {
                    AccrueMoney();
                    StopPharm();
                }

                data.currentPharmMoney = (float)(pharmTime.TotalSeconds * data.incomeInTick);

                Debug.Log($"data.currentPharmMoney: {data.currentPharmMoney}");
                UpdatedData?.Invoke();
            }
        }

        private async void SetStartAndEndPharmTime()
        {
            data.startPharmTime ??= await WorldTime.GetAsync();
            data.lastAddedTime ??= data.startPharmTime.Value;
            data.lastAddedTime ??= data.lastAddedTime.Value.AddMinutes(data.maxPharmTime);
        }

        private void AccrueMoney()
        {
            DataContoller.Imodel.AddCoins(data.currentPharmMoney);

            data.lastAddedTime = null;
            data.startPharmTime = null;
        }

        private async void UpdatePharmTime()
        {
            if (data.startPharmTime.HasValue)
                pharmTime = await WorldTime.GetAsync() - data.startPharmTime.Value;
        }

        private async Task<ushort> GetAFKTimeDifferenceAsync()
        {
            TimeSpan difference;
            var dataConfig = await DataContoller.Imodel.GetConfigAsync();

            if (data.isActivePharm == false)
                difference = data.lastAddedTime.Value - dataConfig.lastGameTime;
            else
                difference = newGameTime - dataConfig.lastGameTime;

            return (ushort)difference.TotalSeconds;
        }

        private void StopPharm()
        {
            data.isActivePharm = false;
            CoroutineManager.Instance.StopCoroutine(PharmRoutine());
            UpdatedData?.Invoke();
        }

        public void StartPharm()
        {
            if (data.isActivePharm)
                return;

            data.isActivePharm = true;
            CoroutineManager.Instance.StartCoroutine(PharmRoutine());
            UpdatedData?.Invoke();
        }
    }
}
