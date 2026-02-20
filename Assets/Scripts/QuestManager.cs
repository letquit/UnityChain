using System;
using System.Collections.Generic;
using UnityEngine;

namespace COR
{
    /// <summary>
    /// 任务管理器，使用责任链模式处理任务状态更新
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        /// <summary>
        /// 存储所有任务的字典，键为任务ID，值为任务对象
        /// </summary>
        private Dictionary<SerializableGUID, Quest> quests = new Dictionary<SerializableGUID, Quest>();
        
        /// <summary>
        /// 责任链处理器
        /// </summary>
        private IQuestProcessor chain;

        /// <summary>
        /// 初始化责任链处理器
        /// </summary>
        private void Awake()
        {
            chain = new StartQuestProcessor();
            chain.SetNext(new CompleteQuestProcessor()).SetNext(new FailQuestProcessor());
        }

        /// <summary>
        /// 注册新任务到管理器中
        /// </summary>
        /// <param name="quest">要注册的任务对象</param>
        public void RegisterQuest(Quest quest) => quests.Add(quest.Id, quest);
        
        /// <summary>
        /// 更新任务状态，通过责任链处理器处理消息
        /// </summary>
        /// <param name="message">任务消息基类，包含任务更新信息</param>
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

    /// <summary>
    /// 任务数据类，存储任务的基本信息和状态
    /// </summary>
    [Serializable]
    public class Quest
    {
        /// <summary>
        /// 任务唯一标识符
        /// </summary>
        public SerializableGUID Id;
        
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name;
        
        /// <summary>
        /// 任务当前状态，默认为未开始
        /// </summary>
        public QuestState State = QuestState.NotStarted;
    }
    
    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum QuestState
    {
        NotStarted,
        InProgress,
        Completed,
        Failed
    }

    /// <summary>
    /// 任务事件枚举
    /// </summary>
    public enum QuestEvent
    {
        Start,
        Complete,
        Fail,
    }
}
