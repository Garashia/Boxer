﻿using System.Collections.Generic;
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

        private List<ShoppingGUI> m_guiList = new List<ShoppingGUI>();
        private OptionsWindow m_optionsWindow;
        public OptionsWindow OptionsWind
        {
            get
            { return m_optionsWindow; }
            set
            { m_optionsWindow = value; }
        }



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
            transform.SendMessage("SetItemDropList", this);
        }

        public void SpawnItemBuyGUI(ShoppingGUI shoppingGUI)
        {

            // foreach (var item in pool)
            foreach (var gui in m_guiList)
                gui.SetInteractable(false);

            var obj = Instantiate(m_optionsWindow.gameObject, transform);
            var y = obj.GetComponent<OptionsWindow>();

            if ((StartupInitializer.StartUp.Parameter.Money - shoppingGUI.Item.BuyPrice)
                < 0)
            {
                y.YesButton.interactable = false;
            }

            y.YesButton.onClick.AddListener(() =>
            {
                StartupInitializer.StartUp.Parameter.Money -= shoppingGUI.Item.BuyPrice;
                StartupInitializer.StartUp.ItemList.AddItem(shoppingGUI.Item.Index, shoppingGUI.Item);
                Destroy(obj);
                foreach (var gui in m_guiList)
                    gui.SetInteractable(true);

            });

            y.NoButton.onClick.AddListener(() =>
            {
                Destroy(obj);
                foreach (var gui in m_guiList)
                    gui.SetInteractable(true);

            });


        }

        void Start()
        {
            // Debug.Log(m_item.Count);
            var ls = GetComponent<LoopScrollRect>();
            ls.prefabSource = this;
            ls.dataSource = this;
            ls.totalCount = m_item.Count;
            ls.RefillCells();
        }

        public void AddGUI(ShoppingGUI shoppingGUI)
        {
            if (m_guiList.Find(n => ReferenceEquals(n, shoppingGUI)) == null)
                m_guiList.Add(shoppingGUI);
        }

        public void DeleteGUI(ShoppingGUI shoppingGUI)
        {
            if (m_guiList.Find(n => ReferenceEquals(n, shoppingGUI)) != null)
                m_guiList.Remove(shoppingGUI);
        }

    }
}