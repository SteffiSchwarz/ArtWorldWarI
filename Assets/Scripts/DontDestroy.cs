using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

    private bool created = false;

    void Awake() 
    {
        if (!created) 
        {
            // this is the first instance - make it persist 
            DontDestroyOnLoad(this.gameObject); 
            created = true; 
        }
        else if (created)
        {
            // this must be a duplicate from a scene reload - DESTROY! 
            Destroy(this.gameObject); 
        } 
    }
}
