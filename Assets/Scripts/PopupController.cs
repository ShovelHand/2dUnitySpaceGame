using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour {

	public void Open(){
		gameObject.SetActive (true);
		//if this is the settings popup, then pause the scene
		if( this.GetComponent<SettingsController>() != null){
			Time.timeScale = 0;
		}
	}

	public void Close(){
		if( this.GetComponent<SettingsController>() != null){
			Time.timeScale = 1;
		}
		gameObject.SetActive (false);
	}
}
