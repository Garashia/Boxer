using UnityEngine;

namespace DesignPatterns.Singleton
{

    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //  �A�N�Z�X���ꂽ��܂��́A�C���X�^���X�����邩���ׂ�
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance == null)
                    {
                        //  �Ȃ���������
                        SetupInstance();
                    }
                    else
                    {
                        //  ���ɉ�������̃f�o�b�O���O�@���ɈӖ��͂Ȃ�
                        string typeName = typeof(T).Name;

                        Debug.Log("[Singleton] " + typeName + " instance already created: " +
                            _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }

        public virtual void Awake()
        {
            //  �d������̂��߂̃`�F�b�N
            RemoveDuplicates();
        }

        //  �V���O���g��������
        private static void SetupInstance()
        {
            _instance = (T)FindObjectOfType(typeof(T));

            if (_instance == null)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = typeof(T).Name;

                _instance = gameObj.AddComponent<T>();
                DontDestroyOnLoad(gameObj);
            }
        }

        private void RemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}