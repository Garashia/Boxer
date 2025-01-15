using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif
public static class CreateAnimatorController
{
    public static AnimatorController GetAnimatorController()
    {
#if UNITY_EDITOR

        string fullpath = EditorUtility.SaveFilePanel("AnimatorController作成", "Assets", "New AnimatorController", "controller");
        if (fullpath.Length == 0) { return null; }

        string path = "Assets" + fullpath.Substring(Application.dataPath.Length);

        //非常に単純なAnimatorControllerを生成するEditor拡張コード
        //http://qiita.com/RyotaMurohoshi/items/c1f29f9afbce910c5438
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(path);
        Debug.Log("Create: " + path, controller);

        return controller;
#else
        return null;

#endif
    }
}
