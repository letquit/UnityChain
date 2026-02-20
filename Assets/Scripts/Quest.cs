using System.Collections.Generic;
using UnityEngine;

namespace COR
{
    /// <summary>
    /// 任务处理器接口，定义了责任链模式中的处理器基本操作
    /// </summary>
    public interface IQuestProcessor
    {
        /// <summary>
        /// 设置下一个处理器
        /// </summary>
        /// <param name="processor">下一个处理器</param>
        /// <returns>设置后的处理器</returns>
        IQuestProcessor SetNext(IQuestProcessor processor);
        
        /// <summary>
        /// 处理任务消息
        /// </summary>
        /// <param name="message">任务消息基类</param>
        /// <param name="quests">任务字典，键为可序列化的GUID，值为任务对象</param>
        void Process(QuestMessageBase message, Dictionary<SerializableGUID, Quest> quests);
    }

    /// <summary>
    /// 任务处理器基类，实现了IQuestProcessor接口的基本功能
    /// </summary>
    public abstract class QuestProcessorBase : IQuestProcessor
    {
        IQuestProcessor next;

        /// <summary>
        /// 设置下一个处理器
        /// </summary>
        /// <param name="processor">下一个处理器</param>
        /// <returns>设置后的处理器</returns>
        public IQuestProcessor SetNext(IQuestProcessor processor) => next = processor;

        /// <summary>
        /// 处理任务消息，如果存在下一个处理器则传递给下一个处理器
        /// </summary>
        /// <param name="message">任务消息基类</param>
        /// <param name="quests">任务字典，键为可序列化的GUID，值为任务对象</param>
        public virtual void Process(QuestMessageBase message, Dictionary<SerializableGUID, Quest> quests) =>
            next?.Process(message, quests);
    }

    /// <summary>
    /// 泛型任务处理器，继承自QuestProcessorBase，用于处理特定类型的消息
    /// </summary>
    /// <typeparam name="TMessage">消息类型，必须继承自QuestMessageBase</typeparam>
    public class GenericQuestProcessor<TMessage> : QuestProcessorBase where TMessage : QuestMessageBase
    {
        
    }
    
    /// <summary>
    /// 失败任务处理器，负责处理任务失败的消息
    /// </summary>
    public class FailQuestProcessor : QuestProcessorBase
    {
        /// <summary>
        /// 处理任务消息，专门处理任务失败的情况
        /// </summary>
        /// <param name="message">任务消息基类</param>
        /// <param name="quests">任务字典，键为可序列化的GUID，值为任务对象</param>
        public override void Process(QuestMessageBase message, Dictionary<SerializableGUID, Quest> quests)
        {
            Debug.Log($"{GetType().Name}: Processing message of type {message.GetType().Name}");

            // 检查消息是否为失败任务消息，并尝试从任务字典中获取对应的任务
            if (message is FailQuestMessage failMessage && quests.TryGetValue(failMessage.QuestId, out var quest))
            {
                // 只有当任务状态为进行中时才将其标记为失败
                if (quest.State == QuestState.InProgress)
                {
                    quest.State = QuestState.Failed;
                    Debug.Log($"Quest '{quest.Name}' failed.");
                }
                
                return;
            }
            
            base.Process(message, quests);
        }
    }
    
    /// <summary>
    /// 完成任务处理器，负责处理任务完成的消息
    /// </summary>
    public class CompleteQuestProcessor : QuestProcessorBase
    {
        /// <summary>
        /// 处理任务消息，专门处理任务完成的情况
        /// </summary>
        /// <param name="message">任务消息基类</param>
        /// <param name="quests">任务字典，键为可序列化的GUID，值为任务对象</param>
        public override void Process(QuestMessageBase message, Dictionary<SerializableGUID, Quest> quests)
        {
            Debug.Log($"{GetType().Name}: Processing message of type {message.GetType().Name}");

            // 检查消息是否为完成任务消息，并尝试从任务字典中获取对应的任务
            if (message is CompleteQuestMessage completeMessage &&
                quests.TryGetValue(completeMessage.QuestId, out var quest))
            {
                // 只有当任务状态为进行中时才将其标记为完成
                if (quest.State == QuestState.InProgress)
                {
                    quest.State = QuestState.Completed;
                    Debug.Log($"Quest '{quest.Name}' completed.");
                }
                
                return;
            }
            
            base.Process(message, quests);
        }
    }
    
    /// <summary>
    /// 开始任务处理器，负责处理任务开始的消息
    /// </summary>
    public class StartQuestProcessor : QuestProcessorBase
    {
        /// <summary>
        /// 处理任务消息，专门处理任务开始的情况
        /// </summary>
        /// <param name="message">任务消息基类</param>
        /// <param name="quests">任务字典，键为可序列化的GUID，值为任务对象</param>
        public override void Process(QuestMessageBase message, Dictionary<SerializableGUID, Quest> quests)
        {
            Debug.Log($"{GetType().Name}: Processing message of type {message.GetType().Name}");

            // 检查消息是否为开始任务消息，并尝试从任务字典中获取对应的任务
            if (message is StartQuestMessage startMessage && quests.TryGetValue(startMessage.QuestId, out var quest))
            {
                // 只有当任务状态为未开始时才将其标记为进行中
                if (quest.State == QuestState.NotStarted)
                {
                    quest.State = QuestState.InProgress;
                    Debug.Log($"Quest '{quest.Name}' started.");
                }
                
                return;
            }
            
            base.Process(message, quests);
        }
    }
    
    /// <summary>
    /// 任务消息基类，所有任务相关消息的父类
    /// </summary>
    public abstract class QuestMessageBase
    {
        public SerializableGUID QuestId;
    }
    
    /// <summary>
    /// 开始任务消息类
    /// </summary>
    public class StartQuestMessage : QuestMessageBase { }
    
    /// <summary>
    /// 完成任务消息类
    /// </summary>
    public class CompleteQuestMessage : QuestMessageBase { }
    
    /// <summary>
    /// 失败任务消息类
    /// </summary>
    public class FailQuestMessage : QuestMessageBase { }
}
