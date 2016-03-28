using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollPanelHandler : MonoBehaviour {

    float[] oldThemeObjectPositions;
	static GameObject[] themeObjects;
    GameObject header;
	GameObject footer;
    public GameObject currentThemeObjectInCenter;
    public float currentThemeObjectPosition;

    static int middleOfScreen = Screen.width / 2;
    float currentPosition;
    float scalePercent;
    float scaleValue;
    float transformValue;

    public float maxThemeObjectScale;
    public float minThemeObjectScale;
    public float maxBaddyHeaderScale;
    public float minBaddyHeaderScale;
	public float maxScoreStarScale;
	public float minScoreStarScale;

	public int maxScoreStarPosY;
    public int maxBaddyHeaderPosY;
    public int leftSideRange;
    public int rightSideRange;

    public ScrollRect scrollRect;

	// Use this for initialization
	void Start () 
	{
        //maxThemeObjectScale = 0.3f;
        //minThemeObjectScale = 0.1f;

        //maxBaddyHeaderScale = 0.45f;
        //minBaddyHeaderScale = 0.01f;

        //maxScoreStarScale = 1f;
        //minScoreStarScale = 0.001f;

        //maxScoreStarPosY = -95;
        //maxBaddyHeaderPosY = 240;

        leftSideRange = (int)((float)middleOfScreen * 0.33f);
        rightSideRange = (int)((float)middleOfScreen * 1.66f);

		//get all ThemeObjects (Basel, NewYork, London..)
		themeObjects = GameObject.FindGameObjectsWithTag ("ThemeButton");

		//initialize the standardScale of ThemeObjects
		GetStandardPositionsFromThemeObjects();
        ScaleMiddleThemeObjectBig();
	}
	
	// Update is called once per frame
	void Update () 
	{
        HandleThemeObjects ();
	}

	void GetStandardPositionsFromThemeObjects()
	{
        oldThemeObjectPositions = new float[themeObjects.Length];

		for (int i = 0; i < themeObjects.Length; i++) 
        {
            oldThemeObjectPositions[i] = themeObjects[i].transform.position.x;
		}
	}

    //scale themeobject in the middle of the screen big initially
    void ScaleMiddleThemeObjectBig()
    {
        for (int i = 0; i < themeObjects.Length; i++)
        {
            currentPosition = themeObjects[i].transform.position.x;

            if ((int)currentPosition <= middleOfScreen + 1 && (int)currentPosition >= middleOfScreen - 1)
            {
                ScaleObject(themeObjects[i], new Vector3(maxThemeObjectScale, maxThemeObjectScale, 1f));

                header = themeObjects[i].transform.parent.GetChild(0).gameObject;
                header.SetActive(true);
                ScaleObject(header, new Vector3(maxBaddyHeaderScale, maxBaddyHeaderScale, 1));
                header.transform.localPosition = new Vector3(0, maxBaddyHeaderPosY, 0);

				footer = themeObjects[i].transform.parent.GetChild(2).gameObject;
				footer.SetActive(true);
				ScaleObject(footer, new Vector3(maxScoreStarScale, maxScoreStarScale, 1));
				footer.transform.localPosition = new Vector3(0, maxScoreStarPosY, 0);

                currentThemeObjectInCenter = themeObjects[i];
                currentThemeObjectPosition = currentThemeObjectInCenter.transform.position.x;
            }
        }
    }

	//detects which of the ThemeObject is in the middle of the Screen
	void HandleThemeObjects()
	{
		for (int i = 0; i < themeObjects.Length; i++) 
		{
            currentPosition = themeObjects[i].transform.position.x;
            header = themeObjects[i].transform.parent.GetChild(0).gameObject;
			footer = themeObjects[i].transform.parent.GetChild(2).gameObject;

            bool isLeftSide = ThemeObjectIsOnLeftSide(oldThemeObjectPositions[i]);
            bool isRightSide = ThemeObjectIsOnRightSide(oldThemeObjectPositions[i]);

            if (isLeftSide)
            {
                TransformObject(themeObjects[i], ref oldThemeObjectPositions[i], isLeftSide);
                currentThemeObjectInCenter = themeObjects[i];
                currentThemeObjectPosition = currentThemeObjectInCenter.transform.position.x;
            }
            else if (isRightSide)
            {
                TransformObject(themeObjects[i], ref oldThemeObjectPositions[i], isLeftSide);
                currentThemeObjectInCenter = themeObjects[i];
                currentThemeObjectPosition = currentThemeObjectInCenter.transform.position.x;
            }
            //themeobject is somewhere else and moving
            else if (currentPosition != oldThemeObjectPositions[i]) // && scaleValue != minThemeObjectScale)
            {
                //Debug.Log("object: " + themeObjects[i]);
                //Debug.Log("scalevalue: " + scaleValue);
                scaleValue = minThemeObjectScale;
                ScaleObject(themeObjects[i], new Vector3(scaleValue, scaleValue, 1f));

                header.SetActive(false);
				footer.SetActive(false);
                oldThemeObjectPositions[i] = currentPosition;
            }
		}
	}

    bool ThemeObjectIsOnLeftSide(float oldPosition)
    {
        if (currentPosition > leftSideRange && currentPosition < middleOfScreen && currentPosition != oldPosition)
            return true;
        else
            return false;
    }

    bool ThemeObjectIsOnRightSide(float oldPosition)
    {
        if (currentPosition > middleOfScreen && currentPosition < rightSideRange && currentPosition != oldPosition)
            return true;
        else
            return false;
    }

    void TransformObject(GameObject currentObject, ref float oldPosition, bool isLeftSide)
    {
        //get percentage of themobject in left area (between middleOfScreen & leftSideRange)
        if (isLeftSide)
        {
            scalePercent = (currentPosition - leftSideRange) / (middleOfScreen - leftSideRange);
        }
        else
        {
            scalePercent = (rightSideRange - currentPosition) / (rightSideRange - middleOfScreen);
        }

        //scale themeobject
        if (scalePercent * maxThemeObjectScale >= minThemeObjectScale)
        {
            scaleValue = scalePercent * maxThemeObjectScale;
            ScaleObject(currentObject, new Vector3(scaleValue, scaleValue, 1f));
        }

        //translate header
        TranslateHeader(scalePercent);
		TranslateScoreStar(scalePercent);

        //scale header
        if (scalePercent * maxBaddyHeaderScale <= maxBaddyHeaderScale)
        {
            scaleValue = scalePercent * maxBaddyHeaderScale;
            ScaleObject(header, new Vector3(scaleValue, scaleValue, 1));
        }

		//scale star
		if (scalePercent * maxScoreStarScale <= maxScoreStarScale)
		{
			scaleValue = scalePercent * maxScoreStarScale;
			ScaleObject(footer, new Vector3(scaleValue, scaleValue, 1));
		}
		
		//check if it's next ThemeObject and set value
        if ((isLeftSide && currentPosition > oldPosition) || (!isLeftSide && currentPosition < oldPosition))
        {
            currentObject.GetComponent<ThemeObject>().isNextObject = true;
        }
        else
        {
            currentObject.GetComponent<ThemeObject>().isNextObject = false;
        }

        oldPosition = currentObject.transform.position.x;
    }

	//sets new Scale to ThemeObject
	void ScaleObject(GameObject themeObject, Vector3 scaleVector)
	{
		themeObject.transform.localScale = scaleVector;
	}

    void TranslateHeader(float scalePercent)
    {
        header.SetActive(true);
        float baddyHeaderPosX = header.transform.localPosition.x;

        if (maxBaddyHeaderPosY * scalePercent <= maxBaddyHeaderPosY)
        {
            transformValue = (maxBaddyHeaderPosY * scalePercent);
            header.transform.localPosition = new Vector3(baddyHeaderPosX, transformValue, 0);
        }
    }

	void TranslateScoreStar(float scalePercent)
	{
		footer.SetActive(true);
		float scoreStarPosX = footer.transform.localPosition.x;
		
		if (maxScoreStarPosY * scalePercent <= maxScoreStarScale)
		{
			transformValue = (maxScoreStarPosY * scalePercent);
			footer.transform.localPosition = new Vector3(scoreStarPosX, transformValue, 0);
		}
	}

    public GameObject GetCurrentObject()
    {
        return currentThemeObjectInCenter;
    }

    public float GetCurrentObjectPosition()
    {
        return currentThemeObjectPosition;
    }
}
