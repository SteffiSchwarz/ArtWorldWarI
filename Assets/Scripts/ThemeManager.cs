using UnityEngine;
using System.Collections;

public class ThemeManager : MonoBehaviour {

    public static string lastThemeInCenterName;

	public void SetCurrentObjectInCenter (GameObject objectInCenter) 
    {
        lastThemeInCenterName = objectInCenter.name;
	}

    public string GetCurrentObjectInCenter()
    {
        return lastThemeInCenterName;
    }
}
