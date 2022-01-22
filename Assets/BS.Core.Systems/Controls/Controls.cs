
using UnityEngine;
using System.Collections.Generic;

namespace BS.Systems
{
    public class Controls : ExtendedMonoBehaviour, ISystemComponent
    {
        public enum Control { UP, DOWN, LEFT, RIGHT }
        public GameObject controlledObject;

        IDictionary<Control, KeyCode> controls;

        public void Awake()
        {
            AddISystemComponent(this);

            controls = new Dictionary<Control, KeyCode>();
            controls.Add(Control.UP, KeyCode.W);
        }
        public void OnDestroy()
        {
            RemoveISystemComponent(this);
        }

    
        private void Start()
        {
            GetReferences();
        

        }
        void GetReferences()
        {
        
        }
        public void Move()
        {
           
                if(controls.TryGetValue(Control.UP,out KeyCode W))
                {
                    controlledObject.transform.Translate(new Vector3(0, 0, 1));
                }
          
            
        }
        private void Update()
        {
            


        }
          
      
        
    }

}