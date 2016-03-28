//thanks to http://answers.unity3d.com/questions/776667/can-a-scroll-rect-snap-to-elements.html

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScrollRectSnap : MonoBehaviour
{
    ScrollPanelHandler scrollPanelHandler;
    GameObject[] themeObjects;
    float[] points;
    [Tooltip("how many screens or pages are there within the content (steps)")]
    public int screens = 5;
    float stepSize;

    ScrollRect scroll;
    bool LerpH;
    float targetH;
    [Tooltip("Snap horizontally")]
    public bool snapInH = true;

    bool LerpV;
    float targetV;
    [Tooltip("Snap vertically")]
    public bool snapInV = false;

    public GameObject currentObject;
    float oldMiddleObjectPosition;
    float currentMiddleObjectPosition;
    public bool leftSwipe = false;
    public bool rightSwipe = false;
    GameObject nextObject;

    // Use this for initialization
    void Awake()
    {
        GameObject panelHandler = GameObject.Find("ScrollPanelHandler");
        scrollPanelHandler = panelHandler.GetComponent<ScrollPanelHandler>();
        //currentThemeObject = scrollPanelHandler.currentThemeObject;

        scroll = gameObject.GetComponent<ScrollRect>();
        themeObjects = FindObjectsWithTag("ThemeButton");

        if (screens > 0)
        {
            points = new float[screens];
            stepSize = 1 / (float)(screens - 1);

            for (int i = 0; i < screens; i++)
            {
                points[i] = i * stepSize;
            }
        }
        else
        {
            points[0] = 0;
        }
    }

    void Update()
    {
        DetectSwipeDirection();

        if (LerpH)
        {
            scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetH, 80 * scroll.elasticity * Time.deltaTime);
            if (Mathf.Approximately(scroll.horizontalNormalizedPosition, targetH)) LerpH = false;
        }
        if (LerpV)
        {
            scroll.verticalNormalizedPosition = Mathf.Lerp(scroll.verticalNormalizedPosition, targetV, 10 * scroll.elasticity * Time.deltaTime);
            if (Mathf.Approximately(scroll.verticalNormalizedPosition, targetV)) LerpV = false;
        }
    }

    GameObject[] FindObjectsWithTag(string tag)
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);
        Array.Sort(foundObjects, CompareObjectNames); 
        return foundObjects;
    }

    int CompareObjectNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

    public void DragEnd()
    {
        if (scroll.horizontal && snapInH)
        {
            targetH = points[GetNextObject()];
            LerpH = true;
        }
        if (scroll.vertical && snapInV)
        {
            targetV = points[GetNextObject()];
            LerpH = true;
        }
    }

    public void OnDrag()
    {
        LerpH = false;
        LerpV = false;
    }

    public void DetectSwipeDirection()
    {
        if (oldMiddleObjectPosition > currentMiddleObjectPosition)
        {
            leftSwipe = false;
            rightSwipe = true;
            
        }
        else if (oldMiddleObjectPosition < currentMiddleObjectPosition)
        {
            leftSwipe = true;
            rightSwipe = false;
        }
    }

    //depending of the swipe direction get the next object in the list
    public int GetNextObject()
    {
        int i = Array.IndexOf<GameObject>(themeObjects, currentObject);

        if (leftSwipe && !rightSwipe)
        {
            if (i - 1 >= 0)
            {
                nextObject = themeObjects[i - 1];
                return Array.IndexOf<GameObject>(themeObjects, nextObject);
            }
            else
            {
                nextObject = themeObjects[i];
                return i;
            }
        }
        else
        {
            if (i + 1 < themeObjects.Length)
            {
                nextObject = themeObjects[i + 1];
                return Array.IndexOf<GameObject>(themeObjects, nextObject);
            }
            else
            {
                nextObject = themeObjects[i];
                return i;
            }
        }
    }

    //save the POSITION of the object in the middle
    public void SaveOldObjectPosition()
    {
        currentObject = scrollPanelHandler.GetCurrentObject();
        oldMiddleObjectPosition = scrollPanelHandler.GetCurrentObjectPosition();
    }

    //save the POSITION of the object which WAS in the middle before!
    public void SaveCurrentObjectPosition()
    {
        if (!currentObject)
            currentObject = GameObject.Find("03_NewYork");
        else
            currentMiddleObjectPosition = currentObject.transform.position.x;
    }

    public void SaveCurrentObjectInCenter()
    {
        ThemeManager themeManager = GameObject.Find("ThemeManager").GetComponent<ThemeManager>();
        themeManager.SetCurrentObjectInCenter(nextObject);
    }

    //set this object in the center, which was in the center before
    void OnLevelWasLoaded()
    {
        ThemeManager themeManager = GameObject.Find("ThemeManager").GetComponent<ThemeManager>();

        if (themeManager.GetCurrentObjectInCenter() != null)
        {
            string objectName = themeManager.GetCurrentObjectInCenter();
            currentObject = GameObject.Find(objectName);

            //currentObject.name = themeManager.GetCurrentObjectInCenter();
            int index = Array.IndexOf<GameObject>(themeObjects, currentObject);

            if (scroll.horizontal && snapInH)
            {
                targetH = points[index];
                LerpH = true;
            }
            if (scroll.vertical && snapInV)
            {
                targetV = points[index];
                LerpH = true;
            }
        }
    }

    //int FindNearest(float f, float[] array)
    //{
    //    float distance = Mathf.Infinity;
    //    int output = 0;
    //    for (int index = 0; index < array.Length; index++)
    //    {
    //        if (Mathf.Abs(array[index] - f) < distance)
    //        {
    //            distance = Mathf.Abs(array[index] - f);
    //            output = index;
    //        }
    //    }
    //    return output;
    //}

    ////look for the index of the nearest object and return it
    //public int FindNextThemeObject()
    //{
    //    foreach (GameObject themeObject in themeObjects)
    //    {
    //        int i = Array.IndexOf<GameObject>(themeObjects, scrollPanelHandler.currentThemeObjectInCenter);
    //        Debug.Log(scrollPanelHandler.currentThemeObjectInCenter);
    //        Debug.Log(themeObject.GetComponent<ThemeObject>().isNextObject);
    //        //if there is a direct next ThemeObject take this one
    //        if (themeObject.GetComponent<ThemeObject>().isNextObject == true)
    //        {
    //            return Array.FindIndex<GameObject>(themeObjects, nextObject => nextObject.GetComponent<ThemeObject>().isNextObject == true);
    //        }
    //        else
    //        {
    //            return FindNearest(scroll.horizontalNormalizedPosition, points);
    //        }
    //    }
    //    return FindNearest(scroll.horizontalNormalizedPosition, points);
    //}
}