using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DismissMenuOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ActionToPerformWhenClicked);
    }

    void ActionToPerformWhenClicked()
    {
        transform.parent.gameObject.SetActive(false);
        
    }
    //lecture code
}
