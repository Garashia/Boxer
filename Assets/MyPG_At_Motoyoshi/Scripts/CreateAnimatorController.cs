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

        string fullpath = EditorUtility.SaveFilePanel("AnimatorController�쐬", "Assets", "New AnimatorController", "controller");
        if (fullpath.Length == 0) { return null; }

        string path = "Assets" + fullpath.Substring(Application.dataPath.Length);

        //���ɒP����AnimatorController�𐶐�����Editor�g���R�[�h
        //http://qiita.com/RyotaMurohoshi/items/c1f29f9afbce910c5438
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(path);
        Debug.Log("Create: " + path, controller);

        return controller;
#else
        return null;

#endif
    }
}
