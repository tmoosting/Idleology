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
        public int _amount; 
        public int _newGame;
        public int _devGame;
        public int _minBound;    
        public int _maxBound;
    }
}
