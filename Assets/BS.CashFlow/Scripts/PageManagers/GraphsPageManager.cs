using BS.Systems;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BS.CashFlow
{
    public class GraphsPageManager : ExtendedMonoBehaviour, IUIPageDisplay, ILayoutComponent
    {
        #region Variables
        public Assets assets = new Assets();
        public Buttons buttons = new Buttons();    
  
        [System.Serializable]
        public struct Buttons
        {
            public Button sixMonths;
            public Button threeMonths;
            public Button all;
        }
        [System.Serializable]
        public struct Assets
        {
            public GameObject graph;
            public TextMeshProUGUI titleText;
            public RectTransform widgetsHolder;
        }

        //LayoutComponent
        [field: SerializeField]
        public LayoutComponentProperties Properties { get; set; }
      
       

        #endregion

        public void InitLayoutComponent(LayoutComponentType type, bool isActive, RectTransform contentParent, string text)
        {
            gameObject.SetActive(isActive);
            Properties = new LayoutComponentProperties(type, gameObject.transform.parent.GetComponent<RectTransform>(), isActive, contentParent, text);
            //assets.titleText.text = Properties.text;         
        }   
        void OnEnable()
        {
            DisplayPage(0);
            AddListeners();
            void AddListeners()
            {
                List<GraphValue> incomeList = Values.incomeList;
                buttons.all.onClick.AddListener(delegate
                {
                    DisplayPage(0);
                });
                buttons.threeMonths.onClick.AddListener(delegate
                {
                    if(incomeList.Count - 3! >= 0)
                    {
                        DisplayPage(incomeList.Count - 3);
                    }
                });
                buttons.sixMonths.onClick.AddListener(delegate
                {
                    if(incomeList.Count - 6! >= 0)
                    {
                        DisplayPage(incomeList.Count - 6);
                    }
                });
            }
        }
        public void DisplayPage(int displayedValuesCount)
        {
           
            List<GraphValue> incomeList = Values.incomeList;
            UpdateButtons();
            DestroyWidgets();
            CreateGraph(displayedValuesCount);
            void CreateGraph(int startIndex)
            {
                List<RectTransform> graphsRectList;
                GameObject tooltip = null;


                CreateWidgets(2);
                void CreateWidgets(int graphsCount)
                {
                    graphsRectList = new List<RectTransform>();
                    int widgetCount = graphsCount + 1;

                  

                    for(int i = 0; i < widgetCount; i++)
                    {
                        if(i == 0 || i == 1)
                        {
                            var newGraph = Instantiate(assets.graph);
                            newGraph.transform.SetParent(assets.widgetsHolder);
                            newGraph.GetComponent<GraphBehaviour>().graphType = (GraphType)i;
                            graphsRectList.Add(newGraph.GetComponent<RectTransform>());
                        }
                        if(i.Equals(2))
                        {
                            tooltip = Instantiate(graphsRectList[0].gameObject.GetComponent<GraphBehaviour>().prefabs.tooltip);
                            tooltip.transform.SetParent(assets.widgetsHolder);
                            graphsRectList.Add(tooltip.GetComponent<RectTransform>());
                        }

                    }
                    
              
                   
                    foreach(RectTransform rT in graphsRectList)
                    {
                        float width = 700;
                        float height = 1000;
                        rT.sizeDelta = new Vector2(width,height/widgetCount);
                        rT.gameObject.SetActive(true);                
                    }


                }


                List<GraphValue> GetValues(List<GraphValue> incomeList, int startIndex)
                {

                    List<GraphValue> valueList = new List<GraphValue>();
                    while(startIndex < incomeList.Count)
                    {
                        valueList.Add(incomeList[startIndex]);
                        startIndex++;
                    }

                    return valueList;
                }
                List<GraphValue> valueList = GetValues(incomeList, startIndex);

                for(int i = 0; i < 2; i++)
                {
                    ShowGraph(valueList, graphsRectList[i]);
                }



                void ShowGraph(List<GraphValue> incomeList, RectTransform graphRect)
                {
                    float border = 25;
                    float offset = border;
                    var graphType = graphRect.gameObject.GetComponent<GraphBehaviour>().graphType;
                    float graphHeight = graphRect.sizeDelta.y * .87f - 25;
                    float graphWidth = graphRect.sizeDelta.x * .87f - 25;
                    float yMaximum = 0;
                    float yMinimum = 0;
                    int reducedCount = incomeList.Count - 1;

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



                    float xSize = graphWidth / reducedCount;

                    //Values
                    GameObject lastCircleGameObject = null;
                    for(int i = 0; i < incomeList.Count; i++)
                    {
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
                        float xPosition = i * xSize;
                        if(incomeList.Count == 1)
                        {
                            xPosition = graphWidth / 2;
                            yPosition = graphHeight / 2;
                        }


                        GameObject circleGameObject = CreatePoint(new Vector2(xPosition + border + offset, yPosition + border), incomeList[i], i, graphRect);
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
                        CreateLine(graphRect, new Vector2(xPosition + border + offset, border), RectTransform.Axis.Vertical, graphHeight);
                    }
                    //Rows
                    int rowsCount = 3;
                    for(int i = 0; i <= rowsCount; i++)
                    {
                        float normalizedValue = i * 1f / rowsCount;
                        Vector2 normalizedVector = new Vector2(0, normalizedValue * graphHeight + border);
                        // float graphSegment = graphWidth / valueList.Count;

                        if(i != 0)
                        {
                            CreateLabel(graphRect, normalizedVector, i - 1);
                        }
                        CreateLine(graphRect, normalizedVector + new Vector2(border + offset, 0), RectTransform.Axis.Horizontal, graphWidth);
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
                        var incomeComponent = rTincomeGO.GetComponent<GraphButtonBehaviour>();
                        incomeComponent.incomeObj = income;
                        incomeComponent.type = GraphObject.point;

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
                        rectTransform.localEulerAngles = new Vector3(0, 0, BS.Systems.Utils.GetAngleFromVectorFloat(dir));

                        var incomeComponent = gameObject.GetComponent<GraphButtonBehaviour>();
                        incomeComponent.incomeObj = income;
                        incomeComponent.graphType = graphType;
                        incomeComponent.type = GraphObject.connection;
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

                }
            }
      
            void UpdateButtons()
            {
                if(incomeList.Count >= 3)
                {
                    buttons.threeMonths.interactable = true;

                }
                else
                {
                    buttons.threeMonths.interactable = false;
                }
                if(incomeList.Count >= 6)
                {
                    buttons.sixMonths.interactable = true;
                }
                else
                {
                    buttons.sixMonths.interactable = false;
                }
                if(incomeList.Count >= 1)
                {
                    buttons.all.interactable = true;
                }
                else
                {
                    buttons.all.interactable = false;
                }


            }
        }
        public void DestroyWidgets()
        {
            foreach(Transform t in assets.widgetsHolder)
            {
                Destroy(t.gameObject);
            }

        }
    }
}
