using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class ResourceManager : MonoBehaviour
    {
        public List<Resource> resourceList = new List<Resource>();
        
        public void InitializeResources(bool newGame)
        { 
            if (newGame == true)
            {
           
            }
            else 
            {
                //TODO: Load from save
            }
        }

    }
}
