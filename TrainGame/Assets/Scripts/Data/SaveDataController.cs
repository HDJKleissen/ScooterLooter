using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveDataController
{
    public static void SaveRun()
    {
        string savefile = "";
        savefile += JsonUtility.ToJson(InterSceneData.Map);
        File.WriteAllText(Application.persistentDataPath + "/CurrentRun.dat", savefile);
    }

    public static void LoadRun()
    {
        string savefile = File.ReadAllText(Application.persistentDataPath + "/CurrentRun.dat");
        MapData map = JsonUtility.FromJson<MapData>(savefile);
        InterSceneData.Map = map;
    }
}
