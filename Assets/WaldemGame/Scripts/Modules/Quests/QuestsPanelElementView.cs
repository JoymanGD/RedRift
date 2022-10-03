using TMPro;
using UnityEngine;

namespace WaldemGame.Quests
{
    public class QuestsPanelElementView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI questName;
        [SerializeField]
        private TextMeshProUGUI questDescription;

        public void Set(string questName, string questDescription)
        {
            this.questName.text = questName;
            this.questDescription.text = questDescription;
        }

        public void Set(QuestData questData)
        {
            Set(questData.QuestName, questData.QuestDescription);
        }

        public virtual void Enable(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}