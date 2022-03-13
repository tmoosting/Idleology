using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 3)]
    public class Resource : ScriptableObject
    {
        public enum Type
        {
            Credit,
            Happiness,
            Influence,
            Force
        }


        public Type _type;
        public ulong _amount; 
        public ulong _newGame;
        public ulong _devGame;
        public ulong _minBound;    
        public ulong _maxBound;
    }
}
