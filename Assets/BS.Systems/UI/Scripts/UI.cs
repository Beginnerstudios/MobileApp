using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BS.CashFlow;


namespace BS.Systems.UI
{
    public class UI : ExtendedMonoBehaviour, ISystemComponent
    {
        public Layout layout = new Layout();
        public Pages pages = new Pages();
        public Prefabs prefabs = new Prefabs();
        [System.Serializable]
        public struct Layout
        {
            public RectTransform canvas;
            public RectTransform topMenu;
            public RectTransform content;
            public RectTransform ads;
            public GameObject LayoutComponentPrefab;
        }
        [System.Serializable]
        public struct Pages
        {
            public List<GameObject> list;
        }
        [System.Serializable]
        public struct Prefabs
        {
            public List<GameObject> listPages;
        }

        public void Awake()
        {
            AddISystemComponent(this);
        }
        public void OnDestroy()
        {
            RemoveISystemComponent(this);
        }

        private void Start()
        {
            Init();
            AddLayoutComponents();
        }
        public void DisplayPageContent(int pageIndex)
        {
          pages.list[pageIndex].GetComponent<IUIPageDisplay>().DisplayPage(pageIndex);
        }
        void Init()
        {
            SetLayoutSize(new float[3] { .1f, .8f, .1f }); //Each value define height of game object, total must equal 1           
        }
        void SetLayoutSize(float[] layoutSizes)
        {
            float[] validatedSizes = Utils.ValidateLayoutSize(layoutSizes);

            float width = layout.canvas.rect.width;
            float height = layout.canvas.rect.height;


            RectTransform[] layouts = new RectTransform[3];
            layouts[0] = layout.topMenu;
            layouts[1] = layout.content;
            layouts[2] = layout.ads;

            for(int i = 0; i < layouts.Length; i++)
            {
                layouts[i].sizeDelta = new Vector2(width, height * validatedSizes[i]);
            }
        }

        void AddLayoutComponents()
        {
            List<GameObject> pagePrefabList = prefabs.listPages;
            pages.list = new List<GameObject>();
            int i = 0;
            if(pagePrefabList.Count > 0)
            {
                foreach(GameObject pagePrefab in pagePrefabList)
                {
                    if(pagePrefab != null)
                    {
                        bool isActive = false;
                        if(i.Equals(0))
                        {
                            isActive = true;
                        }
                        var page = Instantiate(pagePrefab, layout.content.transform);
                        page.GetComponent<ILayoutComponent>().InitLayoutComponent(LayoutComponentType.page, isActive, layout.content, "",this);
                        page.transform.name = pagePrefab.name;
                        pages.list.Add(page);

                        var button = Instantiate(layout.LayoutComponentPrefab, layout.topMenu);
                        button.transform.name = pagePrefab.name;
                        button.GetComponent<ILayoutComponent>().InitLayoutComponent(LayoutComponentType.button, true, layout.content, button.transform.name,this);

                        i += 1;

                    }

                }
            }
            else
            {
                Debug.Log("No pages ale selected.");
            }
        }
    }
    [Serializable]
    public enum LayoutComponentType
    {
        button, page
    }
    [Serializable]
    public class LayoutComponentProperties
    {
        public LayoutComponentType type;
        public RectTransform parent;
        public bool isActive;
        public string text;

        public LayoutComponentProperties(LayoutComponentType type, RectTransform parent, bool isActive, RectTransform contentParent, string text)
        {
            this.type = type;
            this.parent = parent;
            this.isActive = isActive;
            this.text = text;

        }
    }
    public interface ILayoutComponent
    {
        public LayoutComponentProperties Properties { get; set; }       
        public void InitLayoutComponent(LayoutComponentType type, bool isActive, RectTransform contentParent, string text,UI sender);
    }

}