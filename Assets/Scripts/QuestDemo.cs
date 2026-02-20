using System;
using UnityEngine;

namespace COR
{
    /// <summary>
    /// 任务系统演示类，用于展示任务管理器的基本功能和使用方法
    /// </summary>
    public class QuestDemo : MonoBehaviour
    {
        [SerializeField] private QuestManager questManager;

        /// <summary>
        /// Unity生命周期方法，在对象初始化时执行
        /// </summary>
        private void Start()
        {
            // 创建新的任务ID并注册任务到任务管理器
            SerializableGUID questId = new SerializableGUID();
            questManager.RegisterQuest(new Quest { Id = questId, Name = "Find the treasure" });

            // 演示任务状态更新流程：开始 -> 完成 -> 失败
            questManager.UpdateQuest(new StartQuestMessage { QuestId = questId });
            questManager.UpdateQuest(new CompleteQuestMessage { QuestId = questId });
            questManager.UpdateQuest(new FailQuestMessage { QuestId = questId });
        }
    }
}
