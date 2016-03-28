using UnityEngine;
using System.Collections;

public class UIManagerScript : MonoBehaviour {

	public Animator[] contentPanels;
	public Animator contentPanel;

	// Use this for initialization
	void Start () {

		foreach (Animator contentPanel in contentPanels)
		{
			RectTransform transform = contentPanel.gameObject.transform as RectTransform;        
			Vector2 position = transform.anchoredPosition;
			position.y -= transform.rect.height;
			transform.anchoredPosition = position;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleMenu(Animator currentContentPanel)
	{
		contentPanel = currentContentPanel;
		contentPanel.enabled = true;
		
		bool isHidden = contentPanel.GetBool ("isHidden");
		contentPanel.SetBool ("isHidden", !isHidden);
	}
}
