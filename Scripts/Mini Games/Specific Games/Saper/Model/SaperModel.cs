using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private uint _totalReward;

        private byte _sessionTime;

        private bool _inGame;


        async Task<bool> IMiniGameModel.IsStartedAsync()
        {
            _config = await LoadConfigAsync();
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

        void ISaperModel.ClickedCell(Cell.TypeCell typeCell, uint reward)
        {
            if (_openableCellsCount >= GetCountPrizeCells())
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
                if (_playTime.TotalSeconds > _sessionTime)
                {
                    Finish();
                    yield break;
                }

                yield return new WaitForSecondsRealtime(1);

                //BUG если игрок выйдет из игры и после через пару сек зайдет, то время будет считаться с момента выхода дальше
                _playTime = _playTime.Add(TimeSpan.FromSeconds(1));
                _gameView.SetTimerText(_sessionTime - _playTime.TotalSeconds);
                Debug.Log($"playTime: {_playTime} / Countdown: {_sessionTime - _playTime.TotalSeconds}");
            }

            Finish();
        }

        private int GetCountPrizeCells()
            => _config.cells.Where(item => item.typeCell == Cell.TypeCell.Prize)
                .Sum(item => item.elementsCount);

        private async void CalculatePlayTime()
        {
            if (_startGameTime.HasValue)
                _playTime = await WorldTime.GetAsync() - _startGameTime.Value;
        }

        private void Finish()
        {
            _inGame = false;
            _gameView.Finish();

            DataContoller.Imodel.AddCoins(_totalReward);
            Addressables.Release(_config);

            Debug.Log("Game is finished!");
        }
    }
}
