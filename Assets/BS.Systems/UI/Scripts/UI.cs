using System;
using System.Collections.Generic;
using UnityEngine;


namespace BS.Systems
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
            if(pages.list[pageIndex].GetComponent<IUIPageDisplay>() != null)
            {
            pages.list[pageIndex].GetComponent<IUIPageDisplay>().DisplayPage(pageIndex);
            }
            else
            {
                Debug.Log("Your page GameObject not Implement IUIPageDisplay interface !!");
            }
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
                        LayoutComponentType type = LayoutComponentType.page;
                        string name = page.transform.name;
                        page.GetComponent<ILayoutComponent>().InitLayoutComponent(type, isActive, layout.content, name);
                        page.transform.name = pagePrefab.name;
                        pages.list.Add(page);

                        var button = Instantiate(layout.LayoutComponentPrefab, layout.topMenu);
                        type = LayoutComponentType.button;
                        name = page.transform.name;
                        button.transform.name = pagePrefab.name;
                        button.GetComponent<ILayoutComponent>().InitLayoutComponent(type, true, layout.content, name);

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
        public string text;
        public LayoutComponentType type;
        public RectTransform parent;
        public bool isActive;

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
        public void InitLayoutComponent(LayoutComponentType type, bool isActive, RectTransform contentParent, string text);
    }
    public interface IUIPageDisplay
    {
        public void DisplayPage(int displayedValuesCount);
        public void DestroyWidgets();
    }

}