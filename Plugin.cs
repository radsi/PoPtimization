using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PoPtimization
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private ConfigEntry<int> drawDistance;
        private ConfigEntry<bool> removeDeco;

        public GameObject player;

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            drawDistance = Config.Bind("General", "DrawDistance", 100, "The distance to be drawn relative to the camera view.");
            removeDeco = Config.Bind("General", "RemoveDecoration", false, "Remove all plants, trees, stones...");
        }

        private void Update()
        {
            if (player == null)
            {
                player = GameObject.Find("FPSController/FirstPersonCharacter");
            }
            else if (player != null && Input.GetKeyDown(KeyCode.U))
            {
                Camera camera = player.GetComponent<Camera>();
                camera.farClipPlane = drawDistance.Value;

                if (removeDeco.Value)
                {
                    GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("Tree");
                    tagObjects = tagObjects.Concat(GameObject.FindGameObjectsWithTag("Plant")).ToArray();
                    tagObjects = tagObjects.Concat(GameObject.FindGameObjectsWithTag("Bush")).ToArray();

                    GameObject[] nameObjects = FindObjectsOfType<GameObject>()
                        .Where(obj => obj.name.Contains("Rock")).ToArray();

                    GameObject[] objects = tagObjects.Concat(nameObjects).ToArray();

                    for(int i = 0; i < objects.Length; i++)
                    {
                        Debug.Log(objects[i].name);
                        objects[i].SetActive(false);
                    }
                }
            }
        }
    }
}
