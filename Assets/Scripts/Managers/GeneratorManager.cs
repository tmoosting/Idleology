using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class GeneratorManager : MonoBehaviour
    {
     //   Initializing  purchasing   Tracking / value setting 

     public static GeneratorManager Instance;

     private void Awake()
     {
         Instance = this;
     }

     public List<Generator> generatorList = new List<Generator>();

     
     
     public void InitializeGenerators(bool newGame)
     { 
         if (newGame == true)
         {
             foreach (Generator generator in generatorList)
             {
                 generator.SetLevel(0);
                 generator.isGenerator = true;
             }
         }
         else 
         {
             //TODO: Load from save
         }
     }
     
   
     
     
     
     
     

     public Generator GetGenerator(Generator.Type type)
     {
         foreach (Generator gen in generatorList)
             if (gen._type == type)
                 return gen;
         Debug.LogWarning(("Did not find generator for type: " + type.ToString()+"..!"));
         return null;
     }
  
     public Generator GetGenerator(string type)
     {
         foreach (Generator gen in generatorList)
             if (gen._type.ToString()  == type)
                 return gen;
        // Debug.LogWarning(("Did not find generator for type: " + type +"..!"));
         return null;
     }


    }
}
