using System.Collections.Generic;
using WaldemGame.UI;
using UnityEngine;

namespace WaldemGame.Quests
{
    public class QuestsPanelView : APanelView
    {
        [SerializeField]
        private Transform elementsContainer;
        [SerializeField]
        private QuestsPanelElementView questsPanelElementPrefab;

        private List<QuestsPanelElementView> currentQuestElements = new List<QuestsPanelElementView>();

        public void SyncQuestsView(List<QuestData> quests)
        {
            if(currentQuestElements.Count == quests.Count)
            {
                for (int i = 0; i < currentQuestElements.Count; i++)
                {
                    var element = currentQuestElements[i];
                    element.Set(quests[i]);
                }

                return;
            }
            else
            {
                TurnAllElements(false);

                for (int i = 0; i < quests.Count; i++)
                {
                    var questData = quests[i];

                    if(currentQuestElements.Count <= i)
                    {
                        var newPanelElementView = Instantiate(questsPanelElementPrefab, elementsContainer);
                        currentQuestElements.Add(newPanelElementView);
                    }

                    var panelElementView = currentQuestElements[i];

                    panelElementView.Set(questData);

                    panelElementView.Enable(true);
                }
            }
        }

        private void TurnAllElements(bool value)
        {
            foreach (var item in currentQuestElements)
            {
                item.Enable(value);
            }
        }
    }
}