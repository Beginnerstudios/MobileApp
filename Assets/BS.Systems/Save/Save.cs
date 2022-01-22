using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



namespace BS.Systems
{
    //This asset requires obect SaveData with any content.
    public class Save : ExtendedMonoBehaviour, ISystemComponent
    {

        public void Awake()
        {
            AddISystemComponent(this);
        }
        public void OnDestroy()
        {
            RemoveISystemComponent(this);
        }
        void GetReferences()
        {

        }
        public void SaveGame(SaveData saveData)
        {
            string planetDirectory = Application.streamingAssetsPath + "/Save/";
            if(!Directory.Exists(planetDirectory))
            {

                Directory.CreateDirectory(planetDirectory);
            }
            string saveGameFileName = "save";
            string saveGamePath = Application.streamingAssetsPath + "/Save/" + saveGameFileName + ".xxx";


            if(File.Exists(saveGamePath))
            {
                File.Delete(saveGamePath);
            }
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveGamePath, FileMode.Create);
            formatter.Serialize(stream, saveData);
            stream.Close();
        }
        public SaveData Load()
        {
            string directoryPath = Application.streamingAssetsPath + "/Save/";


            if(Directory.Exists(directoryPath))
            {
                string fileName;
                SaveData savedData = null;

                DirectoryInfo di = new DirectoryInfo(directoryPath);
                FileInfo[] fi = di.GetFiles("*.xxx");
                foreach(FileInfo item in fi)
                {
                    fileName = item.FullName;

                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(fileName, FileMode.Open);

                    savedData = formatter.Deserialize(stream) as SaveData;
                    stream.Close();

                }
                return savedData;


            }
            else
            {

                return null;
            }


        }

        //Objects for save
        [Serializable]
        public class SaveData
        {
            List<SaveObject> saveObjects;
            public SaveData(List<SaveObject> saveObjects)
            {
                this.saveObjects = saveObjects;
            }
            public List<SaveObject> GetSaveObjectList()
            {
                return saveObjects;
            }
        }
        [Serializable]
        public class SaveObject
        {
            public enum Type { tile, config }
            public Type type;
            public float[] position;
            public int shapeType;
            //Config
            public string text;

            //Saveobject what store position and type
            public SaveObject(Type type, float[] position, int shapeType)
            {
                this.position = position;
                this.type = type;
                this.shapeType = shapeType;
            }
            //Saveobject what some string
            public SaveObject(Type type, string text)
            {
                this.text = text;
                this.type = type;
            }
        }
        public interface ISaveable
        {
            //All components with this interface must have this property.
            public SaveObject saveObject { get; set; }
        }
    }
}
