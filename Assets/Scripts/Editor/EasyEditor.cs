using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
public class EasyEditor : Editor
{
    /// <summary>
    /// 打开0.Setup场景
    /// </summary>
    [MenuItem("Custom/GotoSetup")]
    public static void GotoSetup()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/0.Setup.unity");
    }
    [MenuItem("Custom/GotoSelectRole")]
    public static void GotoSelectRole()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/2.SelectRole.unity");
    }

    [MenuItem("Custom/GotoUIEditor")]
    public static void GotoUIEditor()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/UIEditor.unity");
    }
    /// <summary>
    /// 把配置文件放入Resource目录下
    /// </summary>
    [MenuItem("Custom/ConfigToResources")]
    public static void ConfigToResources()
    {
        /*
         * 找到目标路径 和 原路径
         * 清空目标路径
         * 把原路径内的所有文件 复制到目标路径 并添加扩展名
         * 强制刷新
         */
         


        var srcPath = Application.dataPath + "/../Config";
        var dstPath = Application.dataPath + "/Resources/Config";

        //清空目标路径
        Directory.Delete(dstPath,true);
        Directory.CreateDirectory(dstPath);

        foreach (var filePath in Directory.GetFiles(srcPath + "/"))
        {
            var fileName = filePath.Substring(filePath.LastIndexOf('/') + 1);
            File.Copy(filePath, dstPath + "/" + fileName + ".bytes",true);
        }

        AssetDatabase.Refresh();
        Debug.Log("aaa");
    }
}
