using System;
using WaldemGame.Reward;
using UnityEngine;

namespace WaldemGame.Quests
{
    public abstract class AQuest : ScriptableObject
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private string questName;
        [SerializeField]
        [TextArea(3, 10)]
        private string questDescription;
        [SerializeField]
        private AReward reward;

        public int Id;
        public string QuestName => questName;
        public string QuestDescription => questDescription;

        public QuestData GetQuestData()
        {
            QuestData questData = new QuestData(id, questName, questDescription);

            return questData;
        }

        public abstract bool IsCompleted();

        public virtual void QuestCompletedAction()
        {
            var questsManager = App.GetManager<QuestsManager>();
            questsManager.FinishQuest(Id);

            reward?.ReleaseReward();
        }
    }

    [Serializable]
    public struct YesNoDialoguesID
    {
        public int Yes;
        public int No;
    }
}