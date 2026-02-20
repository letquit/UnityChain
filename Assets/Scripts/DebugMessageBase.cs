using System;
using System.IO;
using UnityEngine;

namespace COR
{
    /// <summary>
    /// 调试处理器接口，定义了责任链模式中的处理器行为
    /// </summary>
    public interface IDebugProcessor
    {
        /// <summary>
        /// 设置下一个处理器
        /// </summary>
        /// <param name="processor">下一个处理器</param>
        /// <returns>下一个处理器</returns>
        IDebugProcessor SetNext(IDebugProcessor processor);
        
        /// <summary>
        /// 处理调试消息
        /// </summary>
        /// <param name="message">调试消息</param>
        void Process(DebugMessageBase message);
    }

    /// <summary>
    /// 空值检查处理器，负责检查消息是否为空
    /// </summary>
    public class NullCheckProcessor : DebugProcessorBase
    {
        /// <summary>
        /// 处理调试消息，检查消息是否为空
        /// </summary>
        /// <param name="message">调试消息</param>
        public override void Process(DebugMessageBase message)
        {
            if (message == null || message.Message == null)
            {
                Debug.LogError("NullCheckProcessor: Null message detected!");
                return;
            }
            
            base.Process(message);
        }
    }

    /// <summary>
    /// 控制台日志处理器，将消息输出到Unity控制台
    /// </summary>
    public class ConsoleLogProcessor : DebugProcessorBase
    {
        /// <summary>
        /// 处理调试消息，将消息输出到控制台
        /// </summary>
        /// <param name="message">调试消息</param>
        public override void Process(DebugMessageBase message)
        {
            Debug.Log($"ConsoleLogProcessor: {message.Message}");
            base.Process(message);
        }
    }

    /// <summary>
    /// 状态保存处理器，负责将状态数据保存为JSON文件
    /// </summary>
    public class StateSaveProcessor : DebugProcessorBase
    {
        /// <summary>
        /// 处理调试消息，如果是状态保存消息则将其保存为JSON文件
        /// </summary>
        /// <param name="message">调试消息</param>
        public override void Process(DebugMessageBase message)
        {
            // 检查消息类型是否为状态保存消息
            if (message is StateSaveMessage stateMessage)
            {
                string filePath = $"{stateMessage.StateName}_state.json";
                try
                {
                    string json = JsonUtility.ToJson(stateMessage.StateData);
                    File.WriteAllText(filePath, json);
                    Debug.Log($"StateSaveProcessor: State '{stateMessage.StateName}' saved to '{filePath}'");
                }
                catch (Exception e)
                {
                    Debug.LogError($"StateSaveProcessor: Failed to save state '{stateMessage.StateName}'. Error: {e.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 文件日志处理器，将消息写入指定的日志文件
    /// </summary>
    public class FileLogProcessor : DebugProcessorBase
    {
        private string logFilePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logFilePath">日志文件路径</param>
        public FileLogProcessor(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        /// <summary>
        /// 处理调试消息，将消息写入日志文件
        /// </summary>
        /// <param name="message">调试消息</param>
        public override void Process(DebugMessageBase message)
        {
            try
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message.Message}\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"FileLogProcessor: Failed to write to log file. Error: {e.Message}");
            }
            
            base.Process(message);
        }
    }

    /// <summary>
    /// 调试处理器基类，实现了IDebugProcessor接口的基础功能
    /// </summary>
    public abstract class DebugProcessorBase : IDebugProcessor
    {
        private IDebugProcessor next;
        
        /// <summary>
        /// 设置下一个处理器
        /// </summary>
        /// <param name="processor">下一个处理器</param>
        /// <returns>下一个处理器</returns>
        public IDebugProcessor SetNext(IDebugProcessor processor)
        {
            return next = processor ?? throw new ArgumentNullException(nameof(processor));
        }
        
        /// <summary>
        /// 处理调试消息，如果存在下一个处理器则传递给下一个处理器
        /// </summary>
        /// <param name="message">调试消息</param>
        public virtual void Process(DebugMessageBase message) => next?.Process(message);

        
    }
    
    /// <summary>
    /// 调试消息基类
    /// </summary>
    public abstract class DebugMessageBase
    {
        public string Message { get; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息内容</param>
        protected DebugMessageBase(string message) => Message = message;
    }

    /// <summary>
    /// 通用调试消息类
    /// </summary>
    public class GeneralDebugMessage : DebugMessageBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息内容</param>
        public GeneralDebugMessage(string message) : base(message) { }
    }

    /// <summary>
    /// 状态保存消息类
    /// </summary>
    public class StateSaveMessage : DebugMessageBase
    {
        public string StateName { get; }
        public object StateData { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stateName">状态名称</param>
        /// <param name="stateData">状态数据</param>
        public StateSaveMessage(string stateName, object stateData) : base($"Save State: {stateName}")
        {
            StateName = stateName;
            StateData = stateData;
        }
    }
}
