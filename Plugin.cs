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
        /*public float timer, refresh, avgFramerate;

        string display = "{0} FPS";
        string FPSText;

        private GUIStyle guiStyle;*/

        private ConfigEntry<int> drawDistance;
        private ConfigEntry<bool> removeDeco;

        public GameObject player;

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            //Application.targetFrameRate = 2000;
            //QualitySettings.vSyncCount = 0;

            /*guiStyle = new GUIStyle();
            guiStyle.fontSize = 24;
            guiStyle.normal.textColor = Color.white;
            guiStyle.alignment = TextAnchor.MiddleCenter;*/

            drawDistance = Config.Bind("General", "DrawDistance", 100, "The distance to be drawn relative to the camera view.");
            removeDeco = Config.Bind("General", "RemoveDecoration", false, "Remove all plants, trees, stones...");
        }

        private void Update()
        {
            /*float timelapse = Time.smoothDeltaTime;
            timer = timer <= 0 ? refresh : timer -= timelapse;
            if (timer <= 0) avgFramerate = (int)(1f / timelapse);
            FPSText = string.Format(display + "/" + Application.targetFrameRate, avgFramerate.ToString());*/

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

        /*void OnGUI()
        {
            Vector2 textSize = guiStyle.CalcSize(new GUIContent(FPSText));
            Rect rect = new Rect((Screen.width - textSize.x) / 2, (Screen.height - textSize.y) / 2, textSize.x, textSize.y);

            GUI.Label(rect, FPSText, guiStyle);
        }*/
    }
}
