using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "ScriptableObjects/ResourceData", order = 1)]
    public class ResourceData : ScriptableObject
    {
        public Resource.Type _type; 
        public int _newGame;
        public int _devGame;
        public int _minBound;    
        public int _maxBound;
    }
}