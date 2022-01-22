
using UnityEngine;


namespace BS.Systems
{
    public class Camera2D : ExtendedMonoBehaviour, ISystemComponent
    {
        public void Awake()
        {
            AddISystemComponent(this);
        }
        public void OnDestroy()
        {
            RemoveISystemComponent(this);
        }    
       
    }
}