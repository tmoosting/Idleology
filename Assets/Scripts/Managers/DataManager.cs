using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System; 
using System.IO;
using System.Linq;
using ScriptableObjects;
using UI; 
using UnityEditor; 


namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        string loadedDatabaseName;
        string conn;
        IDbConnection dbconn;
        IDbCommand dbcmd;
        IDataReader reader;  
        
        [Header("Assigns")]  
        public List<ResourceData> resourceDataList = new List<ResourceData>();
        public List<GeneratorData> generatorDataList = new List<GeneratorData>();
        public List<ModifierData> modifierDataList = new List<ModifierData>();
    
        
        
        
        [Header("Settings")] 
        public bool forceCloseDatabase;
        public bool loadDatabase;
        public bool loadDataObjects;
        public bool loadNarrativeEvents;

        
        // Called from GameStateManager's Start()
        public void LoadDataObjects()
        {
            if (forceCloseDatabase == true)
            {
                GetTableNames();
                CloseDatabase();
            }
            else
            {
                if (loadDatabase == true)
                    ImportDatabase();
                if (loadDataObjects == true)
                {
                    foreach (Resource  resource in GetComponent<ResourceManager>().resourceList)
                        LoadResourceFromData(resource);
                    foreach (Generator generator in GetComponent<GeneratorManager>().generatorList) 
                        LoadGeneratorFromData(generator);  
                    foreach (Modifier modifier in GetComponent<ModifierManager>().modifierList) 
                        LoadModifierFromData(modifier);  
                }
            }
        
        }

        private void Awake()
        {
            loadedDatabaseName = "IdleDB";
            conn = "URI=file:" + Application.dataPath + "/" + loadedDatabaseName + ".db";
            if ((File.Exists(Application.dataPath + "/" + loadedDatabaseName + ".db"))  == false  )
                Debug.LogWarning("Failed to load database!"); 
        }


        private void ImportDatabase()
        { 
            if (GetTableNames().Contains("Resources"))
            {
                foreach (string str in GetFieldValuesForTable("Resources", "Type")) 
                    LoadResourceDataObject(str);
            }
            else
                Debug.LogWarning("NO Resources Table Found..!");
            if (GetTableNames().Contains("Generators"))
            {
                foreach (string str in GetFieldValuesForTable("Generators", "Type")) 
                    LoadGeneratorDataObject(str);
            }
            else
                Debug.LogWarning("NO Generator Table Found..!");
            if (GetTableNames().Contains("Modifiers"))
            {
                foreach (string str in GetFieldValuesForTable("Modifiers", "Type")) 
                    LoadModifierDataObject(str);
            }
            else
                Debug.LogWarning("NO Modifier Table Found..!");
            if (loadNarrativeEvents == true)
            {
                if (GetTableNames().Contains("Narrative"))
                    ImportNarrativeData();
                else
                    Debug.LogWarning("NO Narrative Table Found..!");
            }
        }

        private void ImportNarrativeData()
        {
            // Delete previous Narrative objects
            GameStateManager.Instance.UIManager.GetComponent<NarrativeUI>().DeleteNarrativeObjects();
            
            // Create new objects
            foreach (string str in GetFieldValuesForTable("Narrative", "ID")) 
                if (!String.IsNullOrEmpty(str))
                    CreateNarrativeEventFromData(str);
            
            // Load them in the list
            GameStateManager.Instance.UIManager.GetComponent<NarrativeUI>().LoadNarrativeObjects();

        }

        private void CreateNarrativeEventFromData(string id )
        {
            NarrativeEvent narrEvent = ScriptableObject.CreateInstance<NarrativeEvent>();
            narrEvent.ID = id;
            narrEvent.eventName =  GetEntryForTableAndFieldWithID("Narrative", "Name", id);
            narrEvent.eventText =  GetEntryForTableAndFieldWithID("Narrative", "Text", id);

            List<string> generatorStrings = new List<string>();
            List<int> generatorTriggers = new List<int>();   
            List<string> modifierStrings = new List<string>();
            List<int> modifierTriggers = new List<int>();
            List<string> resourceStrings = new List<string>();
            List<int> resourceTriggers = new List<int>();
            foreach (string generator in GetEntryForTableAndFieldWithID("Narrative", "Generators", id).Split(','))
                generatorStrings.Add(generator);
            foreach (string generatorTrigger in GetEntryForTableAndFieldWithID("Narrative", "GeneratorTriggers", id).Split(','))
                if (generatorTrigger != "" && generatorTrigger != "NotFound")
                {
                    generatorTriggers.Add(int.Parse(generatorTrigger));

                }
            foreach (string modifier in GetEntryForTableAndFieldWithID("Narrative", "Modifiers", id).Split(','))
                modifierStrings.Add(modifier);
            foreach (string modifierTrigger in GetEntryForTableAndFieldWithID("Narrative", "ModifierTriggers", id).Split(','))
                if (modifierTrigger != ""&& modifierTrigger != "NotFound")
                    modifierTriggers.Add(int.Parse(modifierTrigger));
            foreach (string resource in GetEntryForTableAndFieldWithID("Narrative", "Resources", id).Split(','))
                resourceStrings.Add(resource);
            foreach (string resourceTrigger in GetEntryForTableAndFieldWithID("Narrative", "ResourceTriggers", id).Split(','))
                if (resourceTrigger != ""&& resourceTrigger != "NotFound")
                    resourceTriggers.Add(int.Parse(resourceTrigger));

            if (generatorStrings.Count != 0)
                foreach (string str in generatorStrings)
                {
                    if (str != "")
                    {
                        Generator.Type genType = (Generator.Type) System.Enum.Parse(typeof(Generator.Type),str); 
                        narrEvent.generatorTriggermap.Add(genType, generatorTriggers[generatorStrings.IndexOf(str)]);
                    } 
                }
            if (modifierStrings.Count != 0)
                foreach (string str in modifierStrings)
                {
                    if (str != "")
                    {
                        Modifier.Type modType = (Modifier.Type) System.Enum.Parse(typeof(Modifier.Type),str); 
                        narrEvent.modifierTriggermap.Add(modType, modifierTriggers[modifierStrings.IndexOf(str)]);
                    } 
                }
            if (resourceStrings.Count != 0)
                foreach (string str in resourceStrings)
                {
                    if (str != "")
                    {
                        Resource.Type resType = (Resource.Type) System.Enum.Parse(typeof(Resource.Type),str); 
                        narrEvent.resourceTriggermap.Add(resType, resourceTriggers[resourceStrings.IndexOf(str)]);
                    }  
                }
            AssetDatabase.CreateAsset(narrEvent, $"Assets/ScriptableObjects/NarrativeEvents/{narrEvent.ID}.asset");
            GameStateManager.Instance.UIManager.GetComponent<NarrativeUI>().ReceiveNewNarrativeEvent(narrEvent);
        }


        private void LoadResourceDataObject(string type)
        {
            ResourceData resourceData = GetResourceData(type);
            
            resourceData._newGame = int.Parse( GetEntryForTableAndFieldWithType("Resources", "NewGame", type));
            resourceData._devGame = int.Parse( GetEntryForTableAndFieldWithType("Resources", "DevGame", type));
            resourceData._minBound = int.Parse( GetEntryForTableAndFieldWithType("Resources", "MinBound", type));
            resourceData._maxBound= int.Parse( GetEntryForTableAndFieldWithType("Resources", "MaxBound", type));

        }
        private  void LoadResourceFromData(Resource resource)
        {
            ResourceData resourceData = GetResourceData(resource._type);

            resource._newGame = resourceData._newGame;
            resource._devGame = resourceData._devGame;
            resource._minBound = resourceData._minBound;
            resource._maxBound = resourceData._maxBound;
        }
        

        private void LoadGeneratorDataObject(string type)
        {
            GeneratorData generatorData = GetGeneratorData(type);
            
            generatorData._resource = (Resource.Type) System.Enum.Parse(typeof(Resource.Type),
                GetEntryForTableAndFieldWithType("Generators", "Resource", type)); 
            generatorData._costResource = (Resource.Type) System.Enum.Parse(typeof(Resource.Type),
                GetEntryForTableAndFieldWithType("Generators", "CostResource", type)); 
            generatorData._purchaseCost = int.Parse( GetEntryForTableAndFieldWithType("Generators", "PurchaseCost", type));
            generatorData._levelCost = int.Parse( GetEntryForTableAndFieldWithType("Generators", "LevelCost", type));
            generatorData._production= int.Parse( GetEntryForTableAndFieldWithType("Generators", "Production", type));
            string requiredGenerator = GetEntryForTableAndFieldWithType("Generators", "RequiresGenerator", type);
            if (requiredGenerator == "")
                generatorData._requiresGenerator = false;
            else
            {
                generatorData._requiredGenerator = (Generator.Type) System.Enum.Parse(typeof(Generator.Type),requiredGenerator); 
                generatorData._requiresGenerator = true;
            }
            string requiredModifier = GetEntryForTableAndFieldWithType("Generators", "RequiresModifier", type);
            if (requiredModifier == "")
                generatorData._requiresModifier = false;
         
            else
            {
                generatorData._requiredModifier = (Modifier.Type) System.Enum.Parse(typeof(Modifier.Type),requiredModifier); 
                generatorData._requiresModifier = true;
            }
            generatorData._requiredLevel =    int.Parse( GetEntryForTableAndFieldWithType("Generators", "RequiresLevel", type));
        }
        private  void LoadGeneratorFromData(Generator generator)
        {
            GeneratorData generatorData = GetGeneratorData((generator._type)); 
            generator._resource = generatorData._resource;
            generator._costResource = generatorData._costResource;
            generator._purchaseCost = generatorData._purchaseCost;
            generator._levelCost = generatorData._levelCost;
            generator._production = generatorData._production;
            generator._requiresGenerator = generatorData._requiresGenerator;
            generator._requiredGenerator = generatorData._requiredGenerator;
            generator._requiresModifier = generatorData._requiresModifier;
            generator._requiredModifier = generatorData._requiredModifier;
            generator._requiredLevel = generatorData._requiredLevel;
        }
 
 
        
         private void LoadModifierDataObject(string type)
        {
            ModifierData modifierData = GetModifierData(type);
             
            modifierData._costResource = (Resource.Type) System.Enum.Parse(typeof(Resource.Type),
                GetEntryForTableAndFieldWithType("Modifiers", "CostResource", type)); 
            modifierData._purchaseCost = int.Parse( GetEntryForTableAndFieldWithType("Modifiers", "PurchaseCost", type));
            modifierData._levelCost = int.Parse( GetEntryForTableAndFieldWithType("Modifiers", "LevelCost", type)); 
            string requiredGenerator = GetEntryForTableAndFieldWithType("Modifiers", "RequiresGenerator", type);
            if (requiredGenerator == "")
                modifierData._requiresGenerator = false;
            else
            {
                modifierData._requiredGenerator = (Generator.Type) System.Enum.Parse(typeof(Generator.Type),requiredGenerator); 
                modifierData._requiresGenerator = true;
            }
            string requiredModifier = GetEntryForTableAndFieldWithType("Modifiers", "RequiresModifier", type);
            
            if (requiredModifier == "")
                modifierData._requiresModifier = false;
            else
            {
                modifierData._requiredModifier = (Modifier.Type) System.Enum.Parse(typeof(Modifier.Type),requiredModifier); 
                modifierData._requiresModifier = true;
            }
            modifierData._requiredLevel =    int.Parse( GetEntryForTableAndFieldWithType("Modifiers", "RequiresLevel", type));
            modifierData._creditPercentage = float.Parse(GetEntryForTableAndFieldWithType("Modifiers", "CreditPercentage", type));
            modifierData._happinessCost = int.Parse(GetEntryForTableAndFieldWithType("Modifiers", "HappinessCost", type));
            modifierData._levelPricePercentage = float.Parse(GetEntryForTableAndFieldWithType("Modifiers", "LevelPricePercentage", type));
        }
        private  void LoadModifierFromData(Modifier modifier)
        {
            ModifierData modifierData = GetModifierData((modifier._type));  
            modifier._costResource = modifierData._costResource;
            modifier._purchaseCost = modifierData._purchaseCost;
            modifier._levelCost = modifierData._levelCost; 
            modifier._requiresGenerator = modifierData._requiresGenerator;
            modifier._requiredGenerator = modifierData._requiredGenerator;
            modifier._requiresModifier = modifierData._requiresModifier;
            modifier._requiredModifier = modifierData._requiredModifier;
            modifier._requiredLevel = modifierData._requiredLevel;
            modifier._creditPercentage = modifierData._creditPercentage;
            modifier._happinessCost = modifierData._happinessCost;
            modifier._levelPricePercentage = modifierData._levelPricePercentage;
        }
        
        
        
        
        
        
        
        
        
        
        
     
        public ResourceData GetResourceData(Resource.Type type)
        {
            foreach (ResourceData gen in resourceDataList)
                if (gen._type == type)
                    return gen;
            Debug.LogWarning(("Did not find resourcedata for type: " + type.ToString()+"..!"));
            return null;
        }
        public ResourceData GetResourceData(string type)
        {
            foreach (ResourceData gen in resourceDataList)
                if (gen._type.ToString() == type)
                    return gen;
            Debug.LogWarning(("Did not find resourcedata for type: " + type.ToString()+"..!"));
            return null;
        }
        public GeneratorData GetGeneratorData(Generator.Type type)
        {
            foreach (GeneratorData gen in generatorDataList)
                if (gen.GetGeneratorType() == type)
                    return gen;
            Debug.LogWarning(("Did not find generatordata for type: " + type.ToString()+"..!"));
            return null;
        }

        public GeneratorData GetGeneratorData(string type)
        {
            foreach (GeneratorData gen in generatorDataList)
                if (gen.GetGeneratorType().ToString() == type)
                    return gen;
            Debug.LogWarning(("Did not find generatordata for type: " + type +"..!"));
            return null;
        }
        
        public ModifierData GetModifierData(Modifier.Type type)
        {
            foreach (ModifierData mod in modifierDataList)
                if (mod.GetModifierType() == type)
                    return mod;
            Debug.LogWarning(("Did not find modifierdata for type: " + type.ToString()+"..!"));
            return null;
        }
        public ModifierData GetModifierData(string type)
        {
            foreach (ModifierData mod in modifierDataList)
                if (mod.GetModifierType().ToString() == type)
                    return mod;
            Debug.LogWarning(("Did not find modifierdata for type: " + type.ToString()+"..!"));
            return null;
        }
        
           // Returns a string-list of all table names in database
        public List<string> GetTableNames()
        {
            List<string> returnList = new List<string>(); 
            OpenDatabase(); 
            string sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table'";
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read())        
                returnList.Add(reader.GetValue(0).ToString());        
            CloseDatabase();
            return returnList;
        }
        
        // Returns a string-list of all values for a specific field in a specific table
        public List<string> GetFieldValuesForTable(string tableName, string fieldName)
        {
            List<string> returnList = new List<string>();
            OpenDatabase();

            string sqlQuery = "SELECT "+fieldName+" FROM " + tableName;
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0)) // seems unnecessary
                    returnList.Add("NULL");
                else
                    returnList.Add(reader.GetValue(0).ToString());
            }
            CloseDatabase();
            return returnList;
        }
        
        // Returns a string value for a specific field in a specific table using Type value
        public string GetEntryForTableAndFieldWithType(string tableName, string fieldName, string type )
        { 
            string returnString = "NotFound";
            OpenDatabase(); 
            string sqlQuery = "SELECT " + fieldName + " FROM " + tableName + " WHERE Type='" + type + "'";
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read())        
                returnString = reader.GetValue(0).ToString();        
            CloseDatabase();
            return returnString;
        }
        public string GetEntryForTableAndFieldWithID(string tableName, string fieldName, string id )
        { 
            string returnString = "NotFound";
            OpenDatabase(); 
            string sqlQuery = "SELECT " + fieldName + " FROM " + tableName + " WHERE ID='" + id + "'";
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read())        
                returnString = reader.GetValue(0).ToString();        
            CloseDatabase(); 
            return returnString;
        }
        
        
        void OpenDatabase()
        {
            dbconn = new SqliteConnection(conn);
            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
        }
        void CloseDatabase()
        {
            reader.Close();
            dbcmd.Dispose();
            dbconn.Close();
        }
        
    }

}