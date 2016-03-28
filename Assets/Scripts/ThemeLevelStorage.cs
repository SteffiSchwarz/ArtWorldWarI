using UnityEngine;
using System.Collections;

public class ThemeLevelStorage : MonoBehaviour {
    public static ThemeLevelStorage tls;

    public int[,] starCount = new int[5, 2];

    void Awake()
    {
        if (tls == null)
        {
            DontDestroyOnLoad(gameObject);
            tls = this;
        }
        else if (tls != this)
        {
            Destroy(gameObject);
        }
    }
}
