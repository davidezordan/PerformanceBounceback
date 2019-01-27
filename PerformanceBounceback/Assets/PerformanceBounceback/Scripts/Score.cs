using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	public GameManager gameManager;
	private Text textComponent;
	void Start () {
	   gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	   textComponent = GetComponentInChildren<Text>();
	}
	
	void Update () {
           textComponent.text = "Score: " + gameManager.score.ToString();		
	}
}
