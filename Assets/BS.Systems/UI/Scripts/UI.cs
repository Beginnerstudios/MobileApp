using System;
using UnityEngine;
using System.Collections.Generic;


namespace BS.Systems.UI
{
    public class UI : ExtendedMonoBehaviour, ISystemComponent
    {
        public Layout layout = new Layout();
        public Pages pages = new Pages();
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
            AddListeners();
            AddLayoutComponents();
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
        void AddListeners()
        {

        }
        void AddLayoutComponents()
        {
            int i = 0;
            if(pages.list.Count > 0)
            {
                foreach(GameObject pagePrefab in pages.list)
                {
                    bool isActive = false;
                    if(i.Equals(0))
                    {
                        isActive = true;
                    }

                    var page = Instantiate(pagePrefab);
                    page.transform.SetParent(layout.content.transform);
                    page.AddComponent<LayoutComponentBehaviour>().Init(LayoutComponentType.page, isActive, layout.content);
                    page.transform.name = pagePrefab.name;


                    var button = Instantiate(layout.LayoutComponentPrefab, layout.topMenu);
                    button.transform.name = pagePrefab.name;
                    var layoutComponentButton = button.GetComponent<LayoutComponentBehaviour>();
                    layoutComponentButton.Init(LayoutComponentType.button, true, layout.content);


                    i += 1;

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
        public LayoutComponentProperties(LayoutComponentType type, RectTransform parent, bool isActive,RectTransform contentParent)
        {
            this.type = type;
            this.parent = parent;
            this.isActive = isActive;
        }
    }
    public interface ILayoutComponent
    {
        public LayoutComponentProperties properties { get; set; }
    }

}