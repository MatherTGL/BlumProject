namespace GameAssets.Meta.Quests
{
    public interface IControlQuestsView
    {
        void StartQuest(string guid);

        void TakeReward(string guid);
    }
}
