using System.Threading.Tasks;

namespace GameAssets.System.SaveSystem
{
    public interface IPreloadable
    {
        public Task AsyncPreload();
    }
}
