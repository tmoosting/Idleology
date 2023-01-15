using System.Collections.Generic;
using Interfaces;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class ContentUI : MonoBehaviour
    {
        [Header ("Assigns")]
        public Tab influenceTab;
        public Tab forceTab;
        public Tab techTab;
        public GameObject worldSection;
        public GameObject influenceSection;
        public GameObject forceSection;
        public GameObject techSection;
        public BubbleUI bubbleUI;
        public Transform innovationsParentInfluence;
        public Transform innovationsParentForce;
        public Transform innovationsParentTech;
        
        public List<Tab> tabsList = new List<Tab>();
        public List<GameObject> contentsList = new List<GameObject>();

   
        public Dictionary<Tab, GameObject> tabContentPairings = new Dictionary<Tab, GameObject>();


        private Tab _openTab;
        private bool _inWorldZone;
        private void Awake()
        {

            // Ugly fix for (true) not working in PurchaseManager's GetChildren() when sections are disabled in editor
            contentsList.Add(worldSection);
            contentsList.Add(influenceSection);
            contentsList.Add(forceSection);
            contentsList.Add(techSection);
            foreach (GameObject obj in contentsList)
                obj.SetActive(true);

        }
 

        public void InitializeContentSections(bool newGame)
        { 
            if (newGame)
            {
                tabsList.Add(influenceTab);
                tabsList.Add(forceTab); 
                tabsList.Add(techTab); 
                tabContentPairings.Add(influenceTab, influenceSection);
                tabContentPairings.Add(forceTab, forceSection);
                tabContentPairings.Add(techTab, techSection);
                
                foreach (Tab tab in tabsList)
                    tab.HideTab();
                foreach (GameObject obj in contentsList)
                    obj.SetActive(false);
                OpenWorldSection();
            }
            else 
            {
                //TODO: Load from save
            }
        }

        public void OpenWorldSection()
        {
            if (_inWorldZone == true)
                return;
            bubbleUI.Reopen();
            ResetContent();
            worldSection.SetActive(true);
            _inWorldZone = true;
        }
        public void OpenTab(Tab openedTab)
        {
            if (_openTab == openedTab)
                OpenWorldSection();
            else
            {
                ResetContent();
                _openTab = openedTab;
                openedTab.HighlightTab();
                tabContentPairings[openedTab].SetActive(true);
            } 
        }
        public void ScanUIUnlockables()
        {
            foreach (Tab tab in tabsList)
                tab.ValidateUnlock();
        }


        public void  UpdateContentUI()
        {
            UpdateTabs();
            UpdateContentSections();
        }

        private void UpdateTabs()
        {
            foreach (Tab tab in tabsList)
                tab.UpdateTab();
        }

        private void UpdateContentSections()
        {
             
        }


        void ResetContent()
        {
            _inWorldZone = false;
            _openTab = null;
            foreach (Tab tab in tabsList)
                tab.RemoveHighlight();
            foreach (GameObject obj in contentsList)
                obj.SetActive(false);
        }


        public bool IsWorldZoneOpen()
        {
            return _inWorldZone;
        }
        
    
    }
}
