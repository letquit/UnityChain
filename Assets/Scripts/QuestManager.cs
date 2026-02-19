using System;
using System.Collections.Generic;
using UnityEngine;

namespace COR
{
    public class QuestManager : MonoBehaviour
    {
        private Dictionary<SerializableGUID, Quest> quests = new Dictionary<SerializableGUID, Quest>();
        private IQuestProcessor chain;

        private void Awake()
        {
            chain = new StartQuestProcessor();
            chain.SetNext(new CompleteQuestProcessor()).SetNext(new FailQuestProcessor());
        }

        public void RegisterQuest(Quest quest) => quests.Add(quest.Id, quest);
        
        public void UpdateQuest(QuestMessageBase message) => chain.Process(message, quests);

        /*
        private List<Quest> quests = new List<Quest>();

        public void UpdateQuest(SerializableGUID questId, QuestEvent questEvent)
        {
            foreach (Quest quest in quests)
            {
                if (quest.id == questId)
                {
                    if (questEvent == QuestEvent.Start)
                    {
                        if (quest.State == QuestState.NotStarted)
                        {
                            quest.State = QuestState.InProgress;
                            Debug.Log($"Quest '{quest.name}' started.");
                        }
                        else
                        {
                            Debug.Log($"Quest '{quest.name}' cannot be started. Current state: {quest.State}");
                        }
                    }
                    else if (questEvent == QuestEvent.Complete)
                    {
                        if (quest.State == QuestState.InProgress)
                        {
                            quest.State = QuestState.Completed;
                            Debug.Log($"Quest '{quest.name}' completed.");
                        }
                        else
                        {
                            Debug.Log($"Quest '{quest.name}' cannot be completed. Current state: {quest.State}");
                        }
                    }
                    else if (questEvent == QuestEvent.Fail)
                    {
                        if (quest.State == QuestState.InProgress)
                        {
                            quest.State = QuestState.Failed;
                            Debug.Log($"Quest '{quest.name}' failed.");
                        }
                        else
                        {
                            Debug.Log($"Quest '{quest.name}' cannot be failed. Current state: {quest.State}");
                        }
                    }

                    return;
                }
            }
            Debug.Log($"Quest with id '{questId}' not found.");
        }

        public void RegisterQuest(Quest quest) => quests.Add(quest);
        */
    }

    [Serializable]
    public class Quest
    {
        public SerializableGUID Id;
        public string Name;
        public QuestState State = QuestState.NotStarted;
    }
    
    public enum QuestState
    {
        NotStarted,
        InProgress,
        Completed,
        Failed
    }

    public enum QuestEvent
    {
        Start,
        Complete,
        Fail,
    }
}