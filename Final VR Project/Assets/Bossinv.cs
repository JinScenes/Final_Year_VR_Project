using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossinv : MonoBehaviour
{
    Damageable Dmg;
    Damageable Dmg2;
    bool no;
    [SerializeField] int bossdraindamage;
    bool drain;
    float time;
    
    void Awake()
    {
        no = false;
        Dmg = gameObject.GetComponent<Damageable>();
        
    }

    
    void Update()
    {
        Bossaoe();
        
    }
    void Bossaoe()
    {
         Collider[] hitcolliders = Physics.OverlapSphere(transform.position, 10);
        if (hitcolliders.Length >= 1)
        {
            Dmg.dmgbl = false;
        }
        if (hitcolliders.Length == 0)
        {

            Dmg.dmgbl = true;
        }

        if (drain == true)
        {
            int i = 0;
            if (Dmg.Health < Dmg.maxHealth)
                foreach (Collider hit in hitcolliders)
                {

                    Dmg2 = hitcolliders[i].GetComponent<Damageable>();
                    Dmg2.DealDamage(bossdraindamage);
                    Dmg.Health += bossdraindamage;
                    i++;
                }
        }
       

    }
    IEnumerable clock()
    {
        time = 0;
        drain = false;
        time += Time.deltaTime;
        if (time >= 1)
        {
            drain = true;
        }
        return clock();
    }
  
}
