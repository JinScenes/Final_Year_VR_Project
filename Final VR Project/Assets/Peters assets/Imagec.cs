using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Imagec : MonoBehaviour
{
    
    Image Timage;
    
    [SerializeField] Gamemanager Gm;

    // Start is called before the first frame update
    void Start()
    {
        Timage = gameObject.GetComponent<Image>();
        UpdateHealthBar();


    }

    // Update is called once per frame
    void Update()
    {
       




    }
    public void UpdateHealthBar()
    {

        Timage.fillAmount = Mathf.Clamp(Gm.health / Gm.maxHealth, 0, 1f);

    }

    
}
