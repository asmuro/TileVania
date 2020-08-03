using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    #region SerializeFields

    [SerializeField] float scrollRate = 2;

    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log (this.transform.position.y * scrollRate * Time.deltaTime);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (scrollRate * Time.deltaTime), this.transform.position.z);        
    }

    #endregion
}
