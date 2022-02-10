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

        void Awake()
        {
          
        }
 
        public void Init(LayoutComponentType type,bool isActive,RectTransform contentParent)
        {
            properties = new LayoutComponentProperties(type, gameObject.transform.parent.GetComponent<RectTransform>(),isActive,contentParent);
            gameObject.SetActive(isActive);

            if(properties.type == LayoutComponentType.button)
            {
            Utils.CreatePageButton(gameObject,contentParent);
            }
        
        }
       

    }
}

