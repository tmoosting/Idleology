using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "ScriptableObjects/ResourceData", order = 1)]
    public class ResourceData : ScriptableObject
    {
        public Resource.Type _type; 
        public ulong _newGame;
        public ulong _devGame;
        public ulong _minBound;    
        public ulong _maxBound;
    }
}