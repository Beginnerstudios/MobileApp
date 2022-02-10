using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace BS.Systems
{
    public static class Utils
    {      
        public static float[] ValidateLayoutSize(float[] layoutSizes)
        {
            float total = 0;
            foreach(float value in layoutSizes)
            {
                total += value;
            }
            if(total.Equals(1))
            {
                return layoutSizes;
            }
            else
            {
                Debug.Log("Invalid layout size, default was used.");
                return new float[3] { .1f, .8f, .1f };
            }
        }
        public static void CreatePageButton(GameObject gameObject,RectTransform contentParent)
        {
            var but = gameObject.AddComponent<Button>();
            var img = gameObject.AddComponent<Image>();
            var butTextGO = Object.Instantiate(new GameObject(gameObject.transform.name), but.transform);
            butTextGO.AddComponent<TextMeshProUGUI>().text = gameObject.transform.name;
            butTextGO.GetComponent<TextMeshProUGUI>().color = Color.black;
            but.targetGraphic = img;

            but.onClick.AddListener(delegate
            {
                var butIndex = gameObject.transform.GetSiblingIndex();
                foreach (RectTransform rT in contentParent)
                {
                    if (rT.gameObject.transform.GetSiblingIndex() == butIndex)
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