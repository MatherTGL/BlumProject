using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameAssets.Player.Data;
using GameAssets.Scripts.Service.Time;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameAssets.Meta.MiniGame
{
    public sealed class SaperModel : IMiniGameModel, ISaperModel
    {
        private SaperConfig _config;

        private readonly SaperFieldGenerator _fieldGenerator = new();

        private IMiniGameView _gameView;

        private TimeSpan _playTime;

        private DateTime? _startGameTime;

        private byte _openableCellsCount;

        private static uint _totalReward;

        private byte _sessionTime;

        private bool _inGame;


        async Task<bool> IMiniGameModel.IsStartedAsync()
        {
            _totalReward = 0;
            _config = await LoadConfigAsync();
            Debug.Log($"config: {_config}");
            return DataContoller.Imodel.IsSpendTickets(_config.ticketCost, true);
        }

        async void IMiniGameModel.StartGame(IMiniGameView gameView)
        {
            _startGameTime = await WorldTime.GetAsync();
            _sessionTime = _config.playTimeInSeconds;

            List<SpecificCell> readyField = _fieldGenerator.GetGeneratedField(_config.cells);

            _gameView = gameView;
            _gameView.Start(ref readyField, this);
            _inGame = true;

            Observable.FromCoroutine(TimerRoutine).Subscribe();
        }

        async void ISaperModel.ClickedCell(Cell.TypeCell typeCell, uint reward)
        {
            if (_openableCellsCount >= await GetCountPrizeCellsAsync())
            {
                _inGame = false;
                Debug.Log("All cages with prizes are already open! Game finishing");
                return;
            }

            _openableCellsCount++;
            _totalReward = typeCell == Cell.TypeCell.Bomb ? 0 : _totalReward + reward;
            Debug.Log($"_totalReward after Clicked Cell: {_totalReward}");
        }

        private async Task<SaperConfig> LoadConfigAsync()
            => await Addressables.LoadAssetAsync<SaperConfig>("SaperGameCFG").Task;

        private IEnumerator TimerRoutine()
        {
            CalculatePlayTime();

            while (_inGame)
            {
                Debug.Log($"money: {_totalReward}");
                if (_playTime.TotalSeconds > _sessionTime)
                {
                    Debug.Log($"money1: {_totalReward}");
                    Finish();
                    yield break;
                }

                yield return new WaitForSecondsRealtime(1);

                _playTime = _playTime.Add(TimeSpan.FromSeconds(1));
                _gameView.SetTimerText(_sessionTime - _playTime.TotalSeconds);
                Debug.Log($"playTime: {_playTime} / Countdown: {_sessionTime - _playTime.TotalSeconds}");
            }

            Finish();
        }

        private async UniTask<int> GetCountPrizeCellsAsync()
        {
            Debug.Log($"config in GetCountPrizeCells: {_config}");
            _config ??= await LoadConfigAsync();
            
            return _config.cells.Where(item => item.typeCell == Cell.TypeCell.Prize)
                .Sum(item => item.elementsCount);
        }

        private async void CalculatePlayTime()
        {
            if (_startGameTime.HasValue)
                _playTime = await WorldTime.GetAsync() - _startGameTime.Value;
        }

        private void Finish()
        {
            Debug.Log($"Finished & totalRew: {_totalReward}");
            DataContoller.Imodel.AddCoins(_totalReward);
            Debug.Log($"totalReward: {_totalReward}");
            _inGame = false;
            _gameView.Finish();
            
            Addressables.Release(_config);
            Debug.Log("Game is finished!");
        }
    }
}
