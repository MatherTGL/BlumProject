using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameAssets.Scripts.Service.Time;
using GameAssets.System.GameEvents;
using GameAssets.System.SaveSystem;
using UnityEngine.AddressableAssets;
using static GameAssets.Meta.DailyReward.DailyRewardConfig;
using Debug = UnityEngine.Debug;

namespace GameAssets.Meta.DailyReward
{
    public sealed class DailyRewardsModel : IDailyRewards
    {
        private DailyRewardConfig _dailyConfig;


        async void IDailyRewards.Init()
        {
            //TODO обновить вьюшку
            _dailyConfig ??= await GetDailyConfigAsync();
            LoadData();
            CheckAvailableReward();

            GameEventsController.endGame += Quit;
            //CoroutineManager.Instance.StartCoroutine(CheckAvailableDailyReward());
        }

        private void Quit() => SaveData();

        async void IDailyRewards.Claim()
        {
            var worldTime = await WorldTime.GetAsync();

            if (CanClaimReward(worldTime))
            {
                if (_dailyConfig.dayInSequence >= _dailyConfig.maxDaysInSequence)
                    _dailyConfig.dayInSequence = 1;

                CreditTheAward();
                _dailyConfig.lastClaimReward = worldTime;
                _dailyConfig.isCollect = true;
#if UNITY_EDITOR
                var tempInEditor = _dailyConfig.dayInSequence;
#endif
                _dailyConfig.dayInSequence++;

                //TODO пересмотреть вопрос о сохранении данных в этом классе + при начислении ежед.наград
                SaveData();

                Debug.Log($"temp: {tempInEditor} / current: {_dailyConfig.dayInSequence}");
                Debug.Log($"Награда собрана и время последней награды обновлено: {_dailyConfig.lastClaimReward}");
            }
            else
            {
                Debug.Log("Награду нельзя забрать, тк рано");
            }
        }

        private async void LoadData()
            => await ((ISaveable<DailyRewardConfig>)_dailyConfig).AsyncLoad();

        private void SaveData()
            => ((ISaveable<DailyRewardConfig>)_dailyConfig).Save();

        private async Task<DailyRewardConfig> GetDailyConfigAsync()
            => await Addressables.LoadAssetAsync<DailyRewardConfig>("DailyReward").Task;

        private bool CanClaimReward(in DateTime worldTime)
        {
            if (_dailyConfig.lastClaimReward.HasValue == false)
                return true;
            else
            {
                TimeSpan difference = worldTime - _dailyConfig.lastClaimReward.Value;

                if (difference.TotalDays > 1 && difference.TotalDays < 2)
                {
                    _dailyConfig.isCollect = false;
                    return true;
                }
                else if (difference.TotalDays < 1 && _dailyConfig.isCollect == false)
                {
                    return true;
                }
                else if (difference.TotalDays > 2)
                {
                    _dailyConfig.isCollect = false;
                    _dailyConfig.lastClaimReward = null;
                    _dailyConfig.dayInSequence = 0;
                    return true;
                }
            }

            return false;
        }

        private void CreditTheAward()
        {
            List<IReward> rewards = new();

            foreach (var reward in _dailyConfig.days[_dailyConfig.dayInSequence].reward.Dictionary.Keys)
            {
                rewards.Add(GetReward(reward));
                rewards[^1].Complete(_dailyConfig.days[_dailyConfig.dayInSequence].reward.Dictionary[reward]);
            }
        }

        private IReward GetReward(in TypeReward typeReward)
        {
            if (typeReward == TypeReward.Coin)
                return new RewardCoin();
            else
                return new RewardTicket();
        }

        private async void CheckAvailableReward()
        {
            var time = await WorldTime.GetAsync();

            if (_dailyConfig.lastClaimReward.HasValue)
            {
                TimeSpan difference = time - _dailyConfig.lastClaimReward.Value;
                Debug.Log($"{CanClaimReward(time)} / {difference}");
            }
        }
    }
}
