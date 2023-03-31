using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PoPtimization
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private ConfigEntry<int> drawDistance;
        private ConfigEntry<string[]> removeDeco;

        public GameObject player;

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            drawDistance = Config.Bind("General", "DrawDistance", 100, "The distance to be drawn relative to the camera view.");
            removeDeco = Config.Bind("General", "RemoveDecoration", Array.Empty<string>(), "Valid entries: \"Tree\", \"Bush\", \"Plant\" and \"Rock\"");
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

                if (removeDeco.Value.Length > 0)
                {
                    GameObject[] objects = null;

                    for(int i = 0; i < removeDeco.Value.Length; i++)
                    {
                        if(removeDeco.Value[i] == "Rock")
                        {
                            GameObject[] nameObjects = FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("Rock")).ToArray();
                            objects = objects.Concat(nameObjects).ToArray();
                        }
                        else
                        {
                            objects = objects.Concat(GameObject.FindGameObjectsWithTag(removeDeco.Value[i])).ToArray();
                        }
                    } 

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
