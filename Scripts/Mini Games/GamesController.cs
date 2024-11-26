using Sirenix.OdinInspector;
using TNRD;
using UnityEngine;

namespace GameAssets.Meta.MiniGame
{
    // The task of the class is to load and initialize the selected games
    public sealed class GamesController : MonoBehaviour
    {
        [SerializeField, BoxGroup("Parameters"), Required]
        private SerializableInterface<IMiniGameView> _miniGameView;

        [ShowInInspector, ReadOnly, BoxGroup("Parameters")]
        private IMiniGameModel _miniGameModel = new SaperModel();


        private GamesController() { }

        [Button("Start Game", ButtonSizes.Large), BoxGroup("Parameters/Control")]
        public async void StartGame()
        {
            if (await _miniGameModel.IsStartedAsync())
            {
                var view = Instantiate(_miniGameView.Value.prefab, transform.position, Quaternion.identity);
                _miniGameModel.StartGame(view.GetComponent<IMiniGameView>());
            }
            else
                Debug.LogWarning("The game cannot be started!");
        }
    }
}
