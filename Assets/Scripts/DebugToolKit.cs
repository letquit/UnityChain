using System;
using System.IO;
using UnityEngine;

namespace COR
{
    public class DebugToolKit : MonoBehaviour
    {
        [SerializeField] private string logFilePath = "debug_log.txt";

        private IDebugProcessor chain;

        private void Awake()
        {
            chain = new NullCheckProcessor();
            chain.SetNext(new ConsoleLogProcessor())
                .SetNext(new FileLogProcessor(logFilePath))
                .SetNext(new StateSaveProcessor());
        }
        
        public void Log(DebugMessageBase message) => chain.Process(message);

        /*
        public void DebugMessage(object data)
        {
            // Check for null references
            if (data == null)
            {
                Debug.LogError("DebugToolkit: Null reference detected!");
                return;
            }

            // Log the message to the console
            Debug.Log($"DebugToolkit: {data}");

            // Save the message to a log file
            try
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {data}\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"DebugToolkit: Failed to write to log file. Error: {e.Message}");
            }
        }

        public void SaveState(string stateName, object stateData)
        {
            // Serialize and save the state to a file
            string filePath = $"{stateName}_state.json";
            try
            {
                string json = JsonUtility.ToJson(stateData);
                File.WriteAllText(filePath, json);
                Debug.Log($"DebugToolkit: State '{stateName}' saved to '{filePath}'");
            }
            catch (Exception e)
            {
                Debug.LogError($"DebugToolkit: Failed to save '{stateName}'. Error: {e.Message}");
            }
        }
        */
    }
}
