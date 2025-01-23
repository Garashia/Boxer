using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class ItemDropList : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
    {
        [SerializeField]
        private GameObject item;
        // [SerializeField]
        private List<IsThisItem> m_item = new List<IsThisItem>();
        public List<IsThisItem> Items { get { return m_item; } set { m_item = value; } }

        // Implement your own Cache Pool here. The following is just for example.
        Stack<Transform> pool = new Stack<Transform>();
        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                return Instantiate(item);
            }
            Transform candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            // Use `DestroyImmediate` here if you don't need Pool
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans);
        }

        public void ProvideData(Transform transform, int idx)
        {
            transform.SendMessage("ScrollCellIndex", m_item[idx]);
        }


        void Start()
        {
            Debug.Log(m_item.Count);
            var ls = GetComponent<LoopScrollRect>();
            ls.prefabSource = this;
            ls.dataSource = this;
            ls.totalCount = m_item.Count;
            ls.RefillCells();
        }
    }
}