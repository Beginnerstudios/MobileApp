using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;


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
    
        void AddLayoutComponents()
        {            
            List<GameObject> pageList = pages.list;
            int i = 0;
            if(pageList.Count > 0)
            {
                foreach(GameObject pagePrefab in pageList)
                {
                    if(pagePrefab != null)
                    {
                        bool isActive = false;
                        if(i.Equals(0))
                        {
                            isActive = true;
                        }
                        var page = Instantiate(pagePrefab, layout.content.transform);
                        page.GetComponent<LayoutComponentBehaviour>().Init(LayoutComponentType.page, isActive, layout.content,"");
                        page.transform.name = pagePrefab.name;


                        var button = Instantiate(layout.LayoutComponentPrefab, layout.topMenu);
                        button.transform.name = pagePrefab.name;
                        button.GetComponent<LayoutComponentBehaviour>().Init(LayoutComponentType.button, true, layout.content,button.transform.name);

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
     
        public LayoutComponentProperties(LayoutComponentType type, RectTransform parent, bool isActive,RectTransform contentParent,string text)
        {
            this.type = type;
            this.parent = parent;
            this.isActive = isActive;
            this.text = text;
           
        }
    }
    public interface ILayoutComponent
    {
        public LayoutComponentProperties properties { get; set; }
    }

}