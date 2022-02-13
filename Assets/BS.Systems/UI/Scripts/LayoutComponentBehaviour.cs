using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

namespace BS.Systems.UI
{
   
    public class LayoutComponentBehaviour : MonoBehaviour, ILayoutComponent
    {
       [field: SerializeField]
        public LayoutComponentProperties properties { get;set; }
        public TextMeshProUGUI textField;
        

        void Awake()
        {
          
        }
 
        public void Init(LayoutComponentType type,bool isActive,RectTransform contentParent,string text)
        {
            gameObject.SetActive(isActive);
            properties = new LayoutComponentProperties(type, gameObject.transform.parent.GetComponent<RectTransform>(),isActive,contentParent,text);                             
            textField.text = properties.text;

            if(properties.type == LayoutComponentType.page)
            {
             
            }

            if(properties.type == LayoutComponentType.button)
            {
            Utils.CreatePageButton(gameObject,contentParent);
          
            }
        
        }
       

    }
}

