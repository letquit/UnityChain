using System;
using UnityEngine;

namespace COR
{
    /// <summary>
    /// 调试演示类，用于展示DebugToolKit的使用方法
    /// </summary>
    public class DebugDemo : MonoBehaviour
    {
        [SerializeField] private DebugToolKit debugToolKit;

        /// <summary>
        /// Unity生命周期方法，在对象初始化时调用
        /// </summary>
        private void Start()
        {
            // 记录应用程序启动信息
            debugToolKit.Log(new GeneralDebugMessage("Application started."));
            
            // 记录玩家状态保存信息
            debugToolKit.Log(new StateSaveMessage("player_state", new PlayerData(100, Vector3.zero)));
            
            // 测试空值记录功能
            debugToolKit.Log(null);
        }
    }

    /// <summary>
    /// 表示玩家数据的结构体
    /// </summary>
    [Serializable]
    public struct PlayerData
    {
        public int health;
        public Vector3 position;
        
        /// <summary>
        /// 初始化PlayerData实例
        /// </summary>
        /// <param name="health">玩家生命值</param>
        /// <param name="position">玩家位置坐标</param>
        public PlayerData(int health, Vector3 position)
        {
            this.health = health;
            this.position = position;
        }
    }
}
