using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WaldemGame.Managers;

namespace WaldemGame.Quests
{
    public class QuestsManager : AManager
    {
        [SerializeField]
        private QuestsPanelView questsPanelView;
        [SerializeField]
        private float initPanelDelay = 2;

        private List<QuestData> quests;

        public override void Init()
        {
            quests = new List<QuestData>();
            questsPanelView.ShowPanel(initPanelDelay);
        }

        public void StartQuest(QuestData questData)
        {
            quests.Add(questData);

            questsPanelView.SyncQuestsView(quests);

        }

        public void FinishQuest(QuestData questData)
        {
            quests.Remove(questData);

            questsPanelView.SyncQuestsView(quests);
        }

        public void FinishQuest(int id)
        {
            var quest = quests.FirstOrDefault(q=> q.Id == id);
            quests.Remove(quest);

            questsPanelView.SyncQuestsView(quests);
        }
    }

    public struct QuestData
    {
        public int Id;
        public string QuestName;
        public string QuestDescription;

        public QuestData(int id, string questName, string questDescription)
        {
            Id = id;
            QuestName = questName;
            QuestDescription = questDescription;
        }
    }
}