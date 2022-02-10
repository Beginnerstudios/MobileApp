using System;
using UnityEngine;


namespace BS.Systems.UI
{
    public class UI : ExtendedMonoBehaviour, ISystemComponent
    {
        public Layout layout = new Layout();
        [System.Serializable]
        public struct Layout
        {
            public RectTransform canvas;
            public RectTransform topMenu;
            public RectTransform content;
            public RectTransform ads;
            public GameObject LayoutComponentPrefab;
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
            string[] pages = new string[3]{"List","Graphs","Profile"};

            for(int i = 0; i < pages.Length; i++)
            {

                var page = Instantiate(layout.LayoutComponentPrefab, layout.content);
                page.transform.name = pages[i]+"Page";
                var layoutComponentPage = page.GetComponent<LayoutComponentBehaviour>();
                bool isActive;
                if(i.Equals(0))
                {
                    isActive = true;                                   
                }
                else
                {
                    isActive = false;                
                }
                layoutComponentPage.Init(LayoutComponentType.page, isActive,layout.content);

                var button = Instantiate(layout.LayoutComponentPrefab, layout.topMenu);
                button.transform.name = pages[i];
                var layoutComponentButton = button.GetComponent<LayoutComponentBehaviour>();
                layoutComponentButton.Init(LayoutComponentType.button, true,layout.content);
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