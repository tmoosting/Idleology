using System;
using System.Collections.Generic;
using System.IO;
using Managers;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace UI
{
     //TODO:
     // Create a callback system ingame ticks and user actions, that includes checking for triggers on all available NarrPops
     // Create NarrPop placement system

    
    
    
    public class NarrativeUI : MonoBehaviour
    {
        private List<NarrativeEvent> narrativeEventList = new List<NarrativeEvent>();
        
        private void Awake()
        {
        // TODO: logic for start-popup, which is not loaded through IdleDB 
        }

       



        public List<NarrativeEvent> GetNarrativeEvents()
        {
            List<NarrativeEvent> returnList = new List<NarrativeEvent>();


            return returnList;
        }
        public void DeleteNarrativeObjects()
        {
            if (narrativeEventList.Count != 0)
            {
                foreach (NarrativeEvent narrEvent in narrativeEventList)
                {
                    string path = AssetDatabase.GetAssetPath(narrEvent);
                    AssetDatabase.DeleteAsset(path);
                    narrativeEventList.Remove(narrEvent);
                }
            }
        }


        public void LoadNarrativeObjects()
        { 
            narrativeEventList.Clear();
            string[] allPaths = Directory.GetFiles("Assets/ScriptableObjects/NarrativeEvents", "*.asset", 
                SearchOption.AllDirectories);
            foreach (string path in allPaths)
            {
                string cleanedPath = path.Replace("\\", "/");
                narrativeEventList.Add((NarrativeEvent)AssetDatabase.LoadAssetAtPath(cleanedPath,  typeof(NarrativeEvent)));
            }
        }

        public void ReceiveNewNarrativeEvent(NarrativeEvent narrEvent)
        {
            narrativeEventList.Add(narrEvent);
        }
    }
}
