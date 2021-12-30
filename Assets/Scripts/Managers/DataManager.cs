using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System; 
using System.IO;
using ScriptableObjects;
using UnityEditor.Experimental.GraphView;


namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;

        
        string loadedDatabaseName;
        string conn;
        IDbConnection dbconn;
        IDbCommand dbcmd;
        IDataReader reader;  
        
        [Header("Assigns")]  
        public List<ResourceData> resourceDataList = new List<ResourceData>();
        public List<GeneratorData> generatorDataList = new List<GeneratorData>();
        // ADD: ModifierDataList
        // ADD: UnlockerDataList
 
        
        
        [Header("Settings")] 
        public bool forceCloseDatabase;
        public bool loadDatabase;
        public bool loadDataObjects;

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
                    //ADD: Modifier
                }
            }
        
        }

        private void Awake()
        {
            Instance = this;
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
            //ADD: Modifiers
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
            generatorData._purchaseCost = int.Parse( GetEntryForTableAndFieldWithType("Generators", "PurchaseCost", type));
            generatorData._workerBaseCost = int.Parse( GetEntryForTableAndFieldWithType("Generators", "WorkerBaseCost", type));
            generatorData._production= int.Parse( GetEntryForTableAndFieldWithType("Generators", "Production", type));
 
        }
        private  void LoadGeneratorFromData(Generator generator)
        {
            GeneratorData generatorData = GetGeneratorData((generator._type));
            generator._resource = generatorData._resource;
            generator._purchaseCost = generatorData._purchaseCost;
            generator._workerBaseCost = generatorData._workerBaseCost;
            generator._production = generatorData._production; 
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