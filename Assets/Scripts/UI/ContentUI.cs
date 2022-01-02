using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ContentUI : MonoBehaviour
    {

        public List<Tab> tabsList = new List<Tab>();
        public void InitializeContentSections(bool newGame)
        { 
            if (newGame == true)
            {
           
            }
            else 
            {
                //TODO: Load from save
            }
        }

        public void ScanUnlockables()
        { 
        }
    }
}
