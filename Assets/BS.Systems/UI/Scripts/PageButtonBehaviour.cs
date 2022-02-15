using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BS.Systems.UI
{

    public class PageButtonBehaviour : ExtendedMonoBehaviour, ILayoutComponent
    {
        [field: SerializeField]
        public LayoutComponentProperties Properties { get; set; }
        [field: SerializeField]
        public TextMeshProUGUI titleText { get; set; }


        void Awake()
        {

        }

        public void InitLayoutComponent(LayoutComponentType type, bool isActive, RectTransform contentParent, string text, UI sender)
        {
            gameObject.SetActive(isActive);
            Properties = new LayoutComponentProperties(type, gameObject.transform.parent.GetComponent<RectTransform>(), isActive, contentParent, text);
            titleText.text = Properties.text;


            if(Properties.type == LayoutComponentType.button)
            {
                CreatePageButton(gameObject, contentParent);
                void CreatePageButton(GameObject gameObject, RectTransform contentParent)
                {
                    var but = gameObject.GetComponent<Button>();
                    var img = gameObject.GetComponent<Image>();

                    but.targetGraphic = img;
                    but.onClick.AddListener(delegate
                    {
                        var butIndex = gameObject.transform.GetSiblingIndex();
                        foreach(RectTransform rT in contentParent)
                        {
                            if(rT.gameObject.transform.GetSiblingIndex() == butIndex)
                            {
                                rT.gameObject.SetActive(true);                       
                            }
                            else
                            {
                                rT.gameObject.SetActive(false);
                            }
                        }
                    });
                }
            }

        }


    }
}

