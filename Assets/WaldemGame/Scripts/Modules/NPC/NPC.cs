using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaldemGame.Dialogues;
using WaldemGame.Quests;
using UnityEngine;
using WaldemGame.Abstract;

namespace WaldemGame
{
    public class NPC : UsableObject, IInitable
    {
        [SerializeField]
        private string npcName;
        [SerializeField]
        private List<SimpleDialogue> dialogues;
        [SerializeField]
        private List<NPCQuestData> quests;

        private NPCQuestData activeQuest;
        private int currentDialogueIndex = 0;

        public bool Inited { get; set; }

        public void Init()
        {
            if(!Inited)
            {
                // foreach (var item in dialogues)
                // {
                //     item.OnDialogueEnded.RemoveListener(NextDialogue);
                //     item.OnDialogueEnded.AddListener(NextDialogue);
                // }

                Inited = true;
            }
        }

        public void Reinit()
        {
            Inited = false;
            Init();
        }

        public void NextDialogue()
        {
            if(currentDialogueIndex < dialogues.Count - 1)
            {
                currentDialogueIndex++;
            }
        }

        public void LastDialogue()
        {
            if(dialogues.Count > 0)
            {
                currentDialogueIndex = dialogues.Count - 1;
            }
        }

        protected override void UsableAction()
        {
            int dialogueId = -1;

            if(activeQuest != null)
            {
                if(activeQuest.Quest.IsCompleted())
                {
                    dialogueId = activeQuest.YesNoDialogues.Yes;
                    
                    activeQuest.Quest.QuestCompletedAction();
                    activeQuest = null;
                }
                else
                {
                    dialogueId = activeQuest.YesNoDialogues.No;
                }
            }
            else if(currentDialogueIndex < dialogues.Count)
            {
                dialogueId = currentDialogueIndex;
            }

            if(dialogueId > -1 && dialogueId < dialogues.Count)
            {
                var dialoguesManager = App.GetManager<DialoguesManager>();
                dialoguesManager.StartDialogue(npcName, dialogues[dialogueId]);
            }
                
            StopUsing();
        }

        public void StartFirstQuest()
        {
            var npcQuestData = quests.First();

            if(npcQuestData.Quest)
            {
                var questData = npcQuestData.Quest.GetQuestData();

                var questsManager = App.GetManager<QuestsManager>();
                questsManager.StartQuest(questData);

                activeQuest = npcQuestData;

                quests.Remove(npcQuestData);
            }
        }

        public void AsyncInit()
        {
        }
    }

    [Serializable]
    public class NPCQuestData
    {
        public AQuest Quest;
        public YesNoDialoguesID YesNoDialogues;
    }
}