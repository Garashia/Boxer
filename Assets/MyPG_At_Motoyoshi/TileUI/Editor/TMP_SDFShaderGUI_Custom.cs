using UnityEditor;

public class TMP_SDFShaderGUI_Custom : TMPro.EditorUtilities.TMP_SDFShaderGUI
{
    static bool s_Custom = false;

    protected override void DoGUI()
    {
        base.DoGUI();
        s_Custom = BeginPanel("Custom", s_Custom);
        if (s_Custom)
        {
            OnCustomPanel();
        }
        EndPanel();
    }

    private void OnCustomPanel()
    {
        EditorGUI.indentLevel += 1;
        if (m_Material.HasProperty("_Range"))
            DoFloat("_Range", "Moving Range");
        if (m_Material.HasProperty("_Speed"))
            DoFloat("_Speed", "Moving Speed");
        EditorGUI.indentLevel -= 1;
        EditorGUILayout.Space();
    }

}
