using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Mm24f.Core;
using Mm24f.UI;
using MetalMaxXenoRebornDebugMeunEnable;
using UnityEngine.UI;

namespace MetalMaxXenoRebornDebugMenuEnable;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private bool _debugMenuLoaded = false;

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DebugMenu")
        {
            _debugMenuLoaded = true;
            Logger.LogInfo("DebugMenu scene loaded");

            OpenDebugMenu();

            var content = Singleton<Mm24f.UI.MenuController>.Instance._content;
            var problematic = new string[] { "TestRun", "Camera"};
            foreach (UnityEngine.Transform child in content)
            {
                foreach (var name in problematic)
                {
                    if (child.name == name)
                    {
                        child.gameObject.SetActive(!child.gameObject.activeSelf);
                        Logger.LogInfo("Disabled: " + child.name);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!_debugMenuLoaded)
            {
                Logger.LogInfo("Loading DebugMenu scene...");
                SceneManager.LoadScene("DebugMenu", LoadSceneMode.Additive);
            }
            else
            {
                OpenDebugMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Singleton<Mm24f.UI.MenuController>.Instance._content.Find("Camera").gameObject.SetActive(!Singleton<Mm24f.UI.MenuController>.Instance._content.Find("Camera").gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Singleton<Mm24f.UI.MenuController>.Instance._content.Find("TestRun").gameObject.SetActive(!Singleton<Mm24f.UI.MenuController>.Instance._content.Find("TestRun").gameObject.activeSelf);
        }
    }

    private void OpenDebugMenu()
    {
        if (Singleton<MenuController>.IsInitialized && GameManager.IsSaveDataInitialized)
        {
            Singleton<MenuController>.Instance.OpenClose();
            Logger.LogInfo("DebugMenu toggled");
        }
        else
        {
            Logger.LogInfo("MenuController or SaveData not ready");
        }
    }
}