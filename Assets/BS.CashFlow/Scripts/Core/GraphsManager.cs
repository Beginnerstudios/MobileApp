using BS.Systems;
using CodeMonkey.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BS.CashFlow
{
    public class GraphsManager : ExtendedMonoBehaviour
    {
        #region Variables
        public RectTransform graphsParent;
        public Prefabs prefabs = new Prefabs();
        public Buttons buttons = new Buttons();
        [System.Serializable]
        public struct Buttons
        {
            public Button sixMonths;
            public Button threeMonths;
            public Button all;
            public Button Generate;
        }
        [System.Serializable]
        public struct Prefabs
        {
            public GameObject graph;
        }

        List<RectTransform> graphsRectList;
        public List<GraphValue> incomeList { get; private set; }

        public List<int> exapnsesList { get; private set; }
        GameObject tooltip;
        #endregion

        private void Awake()
        {
            Init();
        }
        void Init()
        {
            int graphCount = 2;
            graphsRectList = new List<RectTransform>();
            for(int i = 0; i < graphCount; i++)
            {
                var newGraph = Instantiate(prefabs.graph, graphsParent);
                newGraph.GetComponent<GraphBehaviour>().graphType = (GraphType)i;
                graphsRectList.Add(newGraph.GetComponent<RectTransform>());
            }

            CreateDetail();

        }

        private void Start()
        {

            buttons.all.onClick.AddListener(delegate
            {
                DestroyGraph();
                CreateGrapth(incomeList.Count - incomeList.Count);
            });
            buttons.threeMonths.onClick.AddListener(delegate
            {
                DestroyGraph();
                CreateGrapth(incomeList.Count - 3);
            });
            buttons.sixMonths.onClick.AddListener(delegate
            {
                DestroyGraph();
                CreateGrapth(incomeList.Count - 6);
            });
            buttons.Generate.onClick.AddListener(delegate
            {
                Generate();
            });


        }

        public void Generate()
        {
            DestroyGraph();
            CreateValues();
            CreateGrapth(0);
            void CreateValues()
            {

                GraphValues gV = new GraphValues();
                incomeList = gV.GenerateDummyData(10);
            }
        }

        void CreateGrapth(int startIndex)
        {
            List<GraphValue> valuList = new List<GraphValue>();
            List<GraphValue> valuList2 = new List<GraphValue>();
            int startIndex2 = startIndex;


            while(startIndex < incomeList.Count)
            {
                valuList.Add(incomeList[startIndex]);
                valuList2.Add(incomeList[startIndex2]);
                startIndex++;
                startIndex2++;
            }


            ShowGraph(new GraphValues().CountDifferences(valuList, GraphType.balance), graphsRectList[0]);
            ShowGraph(new GraphValues().CountDifferences(valuList2, GraphType.income), graphsRectList[1]);

            void ShowGraph(List<GraphValue> incomeList, RectTransform graphRect)
            {
                float border = 25;
                var graphType = graphRect.gameObject.GetComponent<GraphBehaviour>().graphType;
                float graphHeight = graphRect.sizeDelta.y * .87f;
                float graphWidth = graphRect.sizeDelta.x * .93f;
                float yMaximum = 0;
                float yMinimum = 0;

                foreach(GraphValue value in incomeList)
                {
                    int yValue = 0;
                    if(graphType == GraphType.income)
                    {
                        yValue = Utils.GetIntValueFromDictionary(value.incomeDict);
                    }
                    if(graphType == GraphType.balance)
                    {
                        yValue = Utils.GetIntValueFromDictionary(value.balanceDict);
                    }


                    if(yValue > yMaximum)
                    {
                        yMaximum = yValue;
                    }
                    if(yValue < yMinimum)
                    {
                        yMinimum = yValue;
                    }

                }


                yMaximum = yMaximum + ((yMaximum - yMinimum) * 0.2f);
                yMinimum = yMinimum - ((yMaximum - yMinimum) * 0.2f);
                float xSize = graphWidth / (incomeList.Count - 1);

                //Values
                GameObject lastCircleGameObject = null;
                for(int i = 0; i < incomeList.Count; i++)
                {
                    float xPosition = i * xSize;
                    int value = 0;

                    if(graphType == GraphType.income)
                    {
                        value = Utils.GetIntValueFromDictionary(incomeList[i].incomeDict);
                      
                    }
                    if(graphType == GraphType.balance)
                    {
                        value = Utils.GetIntValueFromDictionary(incomeList[i].balanceDict);
                       
                    }



                    float yPosition = ((value - yMinimum) / (yMaximum - yMinimum)) * graphHeight;


                    GameObject circleGameObject = CreatePoint(new Vector2(xPosition + border, yPosition + border), incomeList[i], i, graphRect);
                    if(lastCircleGameObject != null)
                    {
                        CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, graphRect, incomeList[i], graphType);
                    }
                    lastCircleGameObject = circleGameObject;

                }
                //Columns
                int columnCount = incomeList.Count;
                for(int i = 0; i < columnCount; i++)
                {
                    float xPosition = i * xSize;
                    CreateLabel(graphRect, new Vector2(xPosition + border, 0), i);
                    CreateLine(graphRect, new Vector2(xPosition + border, border), RectTransform.Axis.Vertical, graphHeight);
                }
                //Rows
                int rowsCount = 10;
                for(int i = 0; i <= rowsCount; i++)
                {
                    float normalizedValue = i * 1f / rowsCount;
                    Vector2 normalizedVector = new Vector2(0, normalizedValue * graphHeight + border);
                    // float graphSegment = graphWidth / valueList.Count;

                    if(i != 0)
                    {
                        CreateLabel(graphRect, normalizedVector, i - 1);
                    }
                    CreateLine(graphRect, normalizedVector + new Vector2(border, 0), RectTransform.Axis.Horizontal, graphWidth);
                }
            }
        }
        void DestroyGraph()
        {
            foreach(Transform t in graphsParent)
            {
                if(t.childCount > 1 && t.name != "Tooltip(Clone)")
                {
                    foreach(Transform child in t.transform)
                    {
                        if(child.name != "Assets")
                        {
                            Destroy(child.gameObject);

                        }
                    }
                }
            }

        }


        GameObject CreatePoint(Vector2 anchoredPosition, GraphValue income, int index, RectTransform graphRect)
        {
            var graphComponent = graphRect.gameObject.GetComponent<GraphBehaviour>();
            var graphType = graphComponent.graphType;


            GameObject incomeGO = Instantiate(graphComponent.prefabs.point);
            incomeGO.SetActive(true);
            incomeGO.transform.SetParent(graphRect, false);
            RectTransform rTincomeGO = incomeGO.GetComponent<RectTransform>();
            rTincomeGO.anchoredPosition = anchoredPosition + new Vector2(0, 0);
            rTincomeGO.anchorMin = new Vector2(0, 0);
            rTincomeGO.anchorMax = new Vector2(0, 0);
            var incomeComponent = rTincomeGO.GetComponent<PointBehaviour>();
            incomeComponent.incomeObj = income;
            incomeComponent.index = index;
            incomeComponent.incomeList = incomeList;
            incomeComponent.tooltip = tooltip.GetComponent<TooltipBehaviour>();
            incomeComponent.graphType = graphType;
            return incomeGO;
        }
        void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, RectTransform graphRect, GraphValue income, GraphType graphType)
        {
            var graphComponent = graphRect.GetComponent<GraphBehaviour>();

            GameObject gameObject = Instantiate(graphComponent.prefabs.connection);
            gameObject.SetActive(true);
            gameObject.transform.SetParent(graphRect, false);
            gameObject.transform.SetSiblingIndex(2);
            gameObject.GetComponent<Image>().color = graphComponent.color;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

            var incomeComponent = gameObject.GetComponent<ConnectionBehaviour>();
            incomeComponent.incomeObj = income;
            incomeComponent.graphType = graphType;
            incomeComponent.tooltip = tooltip.GetComponent<TooltipBehaviour>();

        }
        void CreateLabel(RectTransform graphRect, Vector2 position, int i)
        {
            var graphComponent = graphRect.gameObject.GetComponent<GraphBehaviour>();
            GameObject labelX = Instantiate(graphComponent.prefabs.label);
            labelX.SetActive(true);
            RectTransform labelXTransform = labelX.GetComponent<RectTransform>();
            labelXTransform.SetParent(graphRect);
            labelXTransform.anchoredPosition = new Vector2(position.x, position.y);
            labelX.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();

        }
        void CreateLine(RectTransform graphRect, Vector2 position, RectTransform.Axis axis, float size)
        {
            var graphComponent = graphRect.gameObject.GetComponent<GraphBehaviour>();
            Vector2 anchor = new Vector2(0, 0);
            GameObject dashX = Instantiate(graphComponent.prefabs.line);
            dashX.SetActive(true);
            RectTransform dashXTransform = dashX.GetComponent<RectTransform>();
            dashXTransform.SetParent(graphRect);
            dashXTransform.anchorMin = anchor;
            dashXTransform.anchorMax = anchor;
            dashXTransform.pivot = anchor;
            dashXTransform.anchoredPosition = new Vector2(position.x, position.y);
            dashXTransform.SetSizeWithCurrentAnchors(axis, size);

        }
        void CreateDetail()
        {
            tooltip = Instantiate(graphsRectList[0].gameObject.GetComponent<GraphBehaviour>().prefabs.tooltip);
            tooltip.transform.SetParent(graphsParent);
      
            tooltip.SetActive(true);
        }
    }
}
