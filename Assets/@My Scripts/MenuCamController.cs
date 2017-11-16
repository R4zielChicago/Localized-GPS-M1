﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuCamController : MonoBehaviour
{

	public float speedFactor = 0.01f;
	public float zoomFactor = 1.0f;
	public Transform currentMount;
	public Camera cameraComp;
	Vector3 lastPosition;
	public InputField inputName, inputCity;
	UserInput userInput = new UserInput();
	public LoadUserData loadUserData;
	public List<UserInput> loadedList;


	void Start ()
	{ 
		UserListInterface.InitializeList ();
		XMLManager.LoadUsers();

		lastPosition = transform.position;
		loadedList = UserDatabase.inputs;

		UserListInterface.SetList(loadedList);
		List<UserInput> testList = UserListInterface.GetList();
		 
	}

	void Update ()
	{ 
		transform.position = Vector3.Lerp (transform.position, currentMount.position, speedFactor); 
		transform.rotation = Quaternion.Slerp (transform.rotation, currentMount.rotation, speedFactor); 
		var velocity = Vector3.Magnitude (transform.position - lastPosition);
		cameraComp.fieldOfView = 60 + velocity * zoomFactor;
		lastPosition = transform.position;
	}

//	public void setUserInfo(GameObject currentMount){
//		Debug.Log ("Current Mount = " + currentMount);
//		if (currentMount == GameObject.Find ("Name Camera Mount")){
//			userInput.userName = inputName.text;
//		}
//		if (currentMount == GameObject.Find ("City Camera Mount")){
//			userInput.userCity = inputCity.text;
//		}
//		if (currentMount == GameObject.Find ("Color Camera Mount")){
//			userInput.markerColor = ColorSlider.currentColor;
//		}	
//
//	}

	public void setMount (Transform newMount)
	{

		//Debug.Log ("Current Mount = " + currentMount);
		userInput.userName = inputName.text;
		Debug.Log ("User Name = " + userInput.userName);
			
		userInput.userCity = inputCity.text;
		Debug.Log ("User City = " + userInput.userCity);

		userInput.markerColor = ColorSlider.currentColor;
		Debug.Log ("User Color = " + userInput.markerColor);
		currentMount = newMount;
	}



	public void changeScene(){

		if(PlayerPrefs.GetInt("User Index") == null){
			PlayerPrefs.SetInt("User Index", 1);
		}
		userInput.userIndex = PlayerPrefs.GetInt ("User Index");
		UserListInterface.AddUser (userInput);
		PlayerPrefs.SetInt ("User Index", PlayerPrefs.GetInt ("User Index")+ 1);
		//XMLManager.SaveUsers ();
		SceneManager.LoadScene ("Take Picture");
		Debug.Log ("Loading Scene: Take Picture");
	}
}﻿

