namespace GameAssets.Meta.Quests
{
    public interface ISpecificQuest
    {
        void Init(string guid, IQuest config);

        bool IsComplete();
    }
}
