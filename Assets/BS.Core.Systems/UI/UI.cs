using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BS.Systems

{
    public class UI : ExtendedMonoBehaviour, ISystemComponent
    {

        //Prefabs
        public GameObject CanvasHolderPrefab;
        public GameObject LayoutHolderPrefab;
        public GameObject ContentContainerPrefab;
        public GameObject canvas;
        public GameObject MenuPanelPrefab;


        //grouList[0] = list of top menu buttons
        //groupList[1] = panel for content
        //groupList[2] = panel for advert
        public List<List<GameObject>> groupList;

        GameObject menuPanel;
        Button menu;
        Button favourites;
        Button profile;
        Vector2 contentWindowSize;

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
        }
        void Init()
        {

            List<GameObject> layout = CreateLayout();
            groupList = new List<List<GameObject>>();
            for(int i = 0; i < layout.Count; i++)
            {
                if(i == 0)
                {
                    groupList.Add(AddContent(layout[i], "Content", 3));
                }
                else
                {
                    groupList.Add(AddContent(layout[i], "Content", 1));
                }
            }
            menu=groupList[0][0].GetComponent<Button>();
            favourites = groupList[0][1].GetComponent<Button>();
            profile = groupList[0][2].GetComponent<Button>();
        }
        void AddListeners()
        {     
            menu.onClick.AddListener(delegate {
                Debug.Log("menu");
                CreateMenuPanel();
            });
            favourites.onClick.AddListener(delegate {
                Debug.Log("favourites");
            });
            profile.onClick.AddListener(delegate {
                Debug.Log("profile");
            });
        }
        List<GameObject> CreateLayout()
        {
            List<GameObject> layoutGroupList = new List<GameObject>();
            int verticalGroupCount = 3;
            float canvasWidth = canvas.GetComponent<RectTransform>().sizeDelta.x;
            float canvasHeight = canvas.GetComponent<RectTransform>().sizeDelta.y;

            List<Vector2> sizeList = new List<Vector2>();
            List<Type> typeList = new List<Type>();
            List<string> nameList = new List<string>();


            for(int i = 0; i < verticalGroupCount; i++)
            {
                float layoutHeight = 10;
                if(i == 1)
                {
                    layoutHeight = 1.25f;
                   
                }

                sizeList.Add(new Vector2(canvasWidth, canvasHeight / layoutHeight));             
                typeList.Add(typeof(HorizontalLayoutGroup));
                nameList.Add("Name" + i);
                layoutGroupList.Add(AddGroup(typeList[i], nameList[i], canvas.transform, sizeList[i]));
                GameObject AddGroup(Type type, string layoutName, Transform parent, Vector2 size)
                {
                    var newLayout = Instantiate(LayoutHolderPrefab, canvas.transform);
                    newLayout.transform.name = layoutName;
                    newLayout.transform.SetParent(parent);
                    newLayout.AddComponent(type);
                    newLayout.GetComponent<RectTransform>().sizeDelta = size;
                    return newLayout;
                }
            }



            canvas.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
            canvas.GetComponent<VerticalLayoutGroup>().childControlWidth = false;
            contentWindowSize = sizeList[1];



            return layoutGroupList;

        }
        List<GameObject> AddContent(GameObject layoutGameObject, string containerName, int containerNumber)
        {
            List<GameObject> containerList = new List<GameObject>();
            for(int i = 0; i < containerNumber; i++)
            {
                var newContainer = Instantiate(ContentContainerPrefab, layoutGameObject.transform);
                newContainer.transform.name = containerName;
                containerList.Add(newContainer);
            }
            return containerList;
        }

        void CreateMenuPanel()
        {
            if(menuPanel)
            {
                Destroy(menuPanel);
                Create();
            }
            else
            {
            Create();
            }
            void Create()
            {
                menuPanel = CreateMenu();
                GameObject CreateMenu()
                {
                    
                    GameObject menu = Instantiate(MenuPanelPrefab, canvas.transform);
                    menu.GetComponent<RectTransform>().sizeDelta = contentWindowSize;
                    return menu;
                }
            }
        }

    }
}