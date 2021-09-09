using UnityEngine;
using static GameManager;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public static class LoadSave
{
    static string path = Application.persistentDataPath;
    public static Progress progress;

    [System.Serializable]
    public class Progress
    {
        public string[] packName;
        List<int>[] levelpoints;

        public Progress(int packageCount)
        {
            packName = new string[packageCount];
            levelpoints = new List<int>[packageCount];
        }

        public int FindPackage(string name)
        {
            int index = -1;
            for(int i = 0; i < packName.Length; i++) if(name == packName[i]) { index = i; break; }
            return index;
        }
        public int GetPoints(string package, int level)
        {
            int points = 0;
            int index = FindPackage(package);
            if (index != -1 && level < levelpoints[index].Count) points = levelpoints[index][level];
            return points;
        }
        public void SetPoints(string package, int level, int points)
        {
            int index = FindPackage(package);
            if (index == -1) return;//Error: Package existiert nicht
            if (levelpoints[index].Count <= level) levelpoints[index].AddRange(new int[level + 1 - levelpoints[index].Count]);
            levelpoints[index][level] = points;
        }
    }


    static int packageCount = 0;
    public static void UpdatePackageCount()
    {
        //Laufe alle gebuildeten Level nach Namen ab (Startszene und Credits ausgeschlossen):
        string pName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(2)).Split('_')[0];
        string pName_active = pName;
        packageCount = 1;
        for (int i = 3; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            //identifiziere Package anhand des Namen:
            pName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (!pName.Contains("_")) continue;
            pName = pName.Split('_')[0];

            if (pName_active != pName)
            {
                pName_active = pName;
                packageCount++;
            }
        }
    }

    public static void DeleteSaveFile()
    {
        if(File.Exists(path + "/notbreakout/savefile.game")) File.Delete(path + "/notbreakout/savefile.game");
    }

    public static bool SaveFileExists() => File.Exists(path + "/notbreakout/savefile.game");

    public static void LoadProgress()
    {
        //update der packageInfo, falls noch nicht geschehen:
        if (packageCount == 0) UpdatePackageCount();

        if (File.Exists(path + "/notbreakout/savefile.game"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path + "/notbreakout/savefile.game", FileMode.Open);
            progress = (Progress)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            if (!Directory.Exists(path + "/notbreakout"))
                Directory.CreateDirectory(path + "/notbreakout");
            progress = new Progress(packageCount);
        }
    }

    public static void SaveProgress()
    {
        if (progress == null) { Debug.Log("Error: progress is nullptr"); return; }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path + "/notbreakout/savefile.game", FileMode.Create);
        formatter.Serialize(stream, progress);
        stream.Close();
    }
}
