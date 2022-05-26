using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class SavedGameUtility
{
    private static string Filename { get => Application.persistentDataPath + "/world.json"; }

    public static void SaveWorld(World world)
    {
        var json = JsonConvert.SerializeObject(world);
        File.WriteAllText(Filename, json);
        Debug.Log($"Saved world to {Filename}: {json}");
    }

    public static World? LoadWorld()
    {
        if (File.Exists(Filename))
        {
            try
            {
                var json = File.ReadAllText(Filename);
                return JsonConvert.DeserializeObject<World>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return null;
            }

        }
        return null;
    }
}

