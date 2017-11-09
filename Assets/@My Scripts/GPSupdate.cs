using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPSupdate : MonoBehaviour
{

 
	// this is the text where the gps mesages will show up
	public Text debugText;
	// this is the time between GPS updates
	public float updateGPStime;
	float elapsed;
	// this is the radius of the sphere in which the marker will be placed
	public float R;
	// variables that the GPS will modify or the developer will test on the editor. These are
	// the values of latitude and longitude;
	public float lat, lon;
	//marker Object that will be placed accordingly to the lat and lon:
	public Transform marker;


	void Start()
	{
		StartCoroutine (checkGPS());
	}


	void FixedUpdate()
	{
		elapsed += Time.fixedDeltaTime;

		if (elapsed > updateGPStime) {

			StartCoroutine (checkGPS ());


			float x = Mathf.Cos (lat*Mathf.PI/180) * Mathf.Cos ((lon-90)*Mathf.PI/180) * R;
			float z = Mathf.Cos (lat*Mathf.PI/180) * Mathf.Sin ((lon-90)*Mathf.PI/180) * R;
			float y = Mathf.Sin (lat*Mathf.PI/180) * R;

			marker.localPosition = new Vector3 (x, y, z);
			marker.forward = marker.position;


			elapsed = 0;
		}
	}



	// this function checks the GPS status and updates the user's position
	IEnumerator checkGPS()
	{
		debugText.text="startingGPS";


		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break;



		// Start service before querying location
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;

			debugText.text="startingGPS:"+maxWait;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			debugText.text="Timed out";
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			debugText.text="Unable to determine device location";
			yield break;
		}
		else
		{
			// Access granted and location value could be retrieved
			debugText.text= ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

			//********************************
			// set values of latitude
			// *******************************

			lat = Input.location.lastData.latitude;
			lon = Input.location.lastData.longitude;

		}

		debugText.text="GPSupdated";

	}



	void OnDestroy()
	{
		// Stop service if there is no need to query location updates continuously
		Input.location.Stop();
	}


	void OnDisable()
	{
		// Stop service if there is no need to query location updates continuously
		Input.location.Stop();
	}
}