using System.Threading.Tasks;

namespace GameAssets.Meta.MiniGame
{
    public interface IMiniGameModel
    {
        Task<bool> IsStartedAsync();

        void StartGame(IMiniGameView gameView);
    }
}
