using System;
using UnityEngine;

namespace COR
{
    public class DebugDemo : MonoBehaviour
    {
        [SerializeField] private DebugToolKit debugToolKit;

        private void Start()
        {
            debugToolKit.Log(new GeneralDebugMessage("Application started."));
            debugToolKit.Log(new StateSaveMessage("player_state", new PlayerData(100, Vector3.zero)));
            debugToolKit.Log(null);
        }
    }

    [Serializable]
    public struct PlayerData
    {
        public int health;
        public Vector3 position;
        
        public PlayerData(int health, Vector3 position)
        {
            this.health = health;
            this.position = position;
        }
    }
}
