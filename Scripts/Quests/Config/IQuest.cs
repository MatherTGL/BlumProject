namespace GameAssets.Meta.Quests
{
    public interface IQuest
    {
        BaseQuest.TypeQuest typeQuest { get; } 
        
        BaseQuest.JobSubtype jobSubtype { get; }
        
        bool isProgress { get; }

        bool isDone { get; }

        bool canTake { get; }

        string guid { get; }


        void Init(bool isProgress, bool isDone, bool canTake);

        void StartQuest();

        bool IsStarted() => isProgress;

        void Complete();
        
        bool IsComplete() => isDone;

        void TakeReward();

        bool IsTakedReward() => canTake;

        bool IsCompleteConditions();
    }
}
