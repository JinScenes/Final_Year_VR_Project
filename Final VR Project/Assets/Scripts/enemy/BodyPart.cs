using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BodyPart[] parts;
    [SerializeField] GameObject Limb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hitBodyPart()
    { 

        if(parts.Length > 0) {
        
                foreach(BodyPart part in parts) 
                    { 
            
                    if(part != null)
                        {
                    part.hitBodyPart();
                        }
                        }
        }

        if(Limb != null)
        {
            Instantiate(Limb, transform.position, transform.rotation);
        }
                transform.localScale = Vector3.zero;
                Destroy(this);
    }
}
