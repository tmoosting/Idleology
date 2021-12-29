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
        public List<GeneratorData> generatorDataList = new List<GeneratorData>();
        // ADD: ModifierDataList
        // ADD: UnlockerDataList
 
        
        
        [Header("Settings")] 
        public bool loadDatabase;

        public void LoadDataObjects()
        {
            if (loadDatabase == true)
                ImportDatabase();
 
            foreach (Generator generator in GetComponent<GeneratorManager>().generatorList) 
                LoadGeneratorFromData(generator);  
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
            if (GetTableNames().Contains("Generators"))
            {
                foreach (string str in GetFieldValuesForTable("Generators", "Type")) 
                    LoadGeneratorDataObject(str); 

            }
            else
                Debug.LogWarning("NO Generator Table Found..!");
            //ADD: Modifiers
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
        
        
        public GeneratorData GetGeneratorData(Generator.Type type)
        {
            foreach (GeneratorData gen in generatorDataList)
                if (gen.GetGeneratorType() == type)
                    return gen;
            Debug.LogWarning(("Did not find generator for type: " + type.ToString()+"..!"));
            return null;
        }

        public GeneratorData GetGeneratorData(string type)
        {
            foreach (GeneratorData gen in generatorDataList)
                if (gen.GetGeneratorType().ToString() == type)
                    return gen;
            Debug.LogWarning(("Did not find generator for type: " + type +"..!"));
            return null;
        }
        
        
        
        
        
    }

}