using UnityEngine;

/// <summary>
/// ��ɃJ�����̕��������I�u�W�F�N�g��]���J�����ɌŒ�
/// </summary>
public class BillboardTest : MonoBehaviour
{
    void LateUpdate()
    {
        // ��]���J�����Ɠ���������
        transform.rotation = Camera.main.transform.rotation;
    }
}