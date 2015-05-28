using UnityEngine;
using System.Collections;
using iGUI;
using System;

public class DigitalMovements : MonoBehaviour {


	private GameObject corneaPivot;
	private GameObject BFSCenterAnt;
	private GameObject BFSCenterPost;

	// Values init
	private static Quaternion cameraQuaternion;

	// Cornea touch Rotation
	private bool isRotating;
	private bool isPivotRotating;
	private float rotatingDegSpeed;
	private float speedFactor = 5f; // variable to control the speed rotation

	//Zoom
	private bool isZoom;
	private float zoomSpeed;

	//improving frame
	private int countFrame;


	//Graphics Interface Object
	private GraphicsUserInterface gui;

	// drag
	private bool isTranslated;
	private float translatedSpeed;
	private GameObject zoneTouchable;

	// TouchScreen
	private Touch touch;
	private Touch touch1;
	private Touch touch2;

	private static float screenLandscapeXMin;

	// Long press time
	private float touchTime;
	private const float longPressTime = 0.9f;
	private Vector3 corneaCenter;

	private float BFSRadius;
	private int corneaTag;

	private GameObject crossTarget1;
	private GameObject crossTarget2;

	// Use this for initialization
	void Start () {

		#if UNITY_IPHONE
		{
			this.speedFactor = 5f;
			this.rotatingDegSpeed = 0.6f;
			this.zoomSpeed = 1f;
			this.translatedSpeed = 0.025f;
			screenLandscapeXMin = Screen.width - Screen.height;
		}
		#elif UNITY_ANDROID
		{
			this.speedFactor = 30f;
			this.rotatingDegSpeed = 3.6f;
			this.zoomSpeed = 6f;
			this.translatedSpeed = 0.15f;
			screenLandscapeXMin = Screen.width - (1.30f * Screen.height);
		}
		#endif

		if(this.gameObject.tag.CompareTo("corneaCollider") == 0)
		{
			this.corneaTag = 0;
			this.BFSRadius = iGUICode_CorneaScene.BFSAnt.R;
		}
		else if(this.gameObject.tag.CompareTo("corneaPost") == 0)
		{
			this.corneaTag = 1;
			this.BFSRadius = iGUICode_CorneaScene.BFSPost.R;
		}

		this.gui = new GraphicsUserInterface ();

		this.corneaPivot = GameObject.Find ("corneaPivot");
		this.corneaPivot.transform.rotation = MobileUnityInterface.corneaQuaternion;
		this.corneaPivot.transform.position = MobileUnityInterface.corneaPosition;
		// Calculate the center point of the mesh
		this.corneaCenter = this.corneaPivot.transform.position;

		// Create the touchable zone 
		this.zoneTouchable = GameObject.Find ("ZoneCollider");
		this.zoneTouchable.transform.position = this.corneaPivot.transform.position;


		Camera.main.transform.position = new Vector3(this.corneaCenter.x, 3.1f, 0f);
		cameraQuaternion = Camera.main.transform.rotation;
		Camera.main.orthographicSize = 5f;

		//Rotation
		this.isRotating = false;
		this.isPivotRotating = false;
		
		// Zoom
		this.isZoom = false;
		
		//Translation
		this.isTranslated = false;
		
		this.gui.setTactileZoneOrientation ();
		iGUICode_CorneaScene.getInstance().container1.setPositionAndSize(new Rect(0.25f, 0.497f, 0f, 1530f));
		this.countFrame = 0;

		// Initialization of the cross target vizualisation
		this.crossTarget1 = GameObject.FindGameObjectWithTag ("crossTarget1");
		this.crossTarget2 = GameObject.FindGameObjectWithTag ("crossTarget2");
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.touchCount == 1) 
		{
			this.touch = Input.GetTouch(0);
			if(touch.position.x > screenLandscapeXMin)
				this.oneFingerGestures();
		}
		else if (Input.touchCount == 2) 
		{
			this.touch1 = Input.touches [0];
			this.touch2 = Input.touches [1];

			if(touch1.position.x > screenLandscapeXMin && touch2.position.x > screenLandscapeXMin)
				this.twoFingersGestures();
		}
		this.zoneTouchable.transform.rotation = MobileUnityInterface.corneaQuaternion;

		/*// Memory improvement
		if (Time.frameCount % 30 == 0)
		{
			System.GC.Collect();
		}*/
	}

	private void twoFingersGestures() 
	{
		// If at least one of them moved ...
		if(touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
		{
			// Zoom in/out
			this.zoom(touch1, touch2);

			// Rotate Pivot
			this.rotatePivot(touch1, touch2);

			// Translation
			this.translate(touch1, touch2);
		}

		if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
		{
			this.isZoom = false;
			this.isTranslated = false;
			this.isPivotRotating = false;
		}
	}

	private void oneFingerGestures() 
	{
		if(touch.phase == TouchPhase.Began)
		{
			touchTime = Time.time;
			this.isRotating = false;
		}

		// One finger rotation
		if(this.gameObject.tag.CompareTo("Pachymetry") != 0)
			this.rotate (this.touch);

		// Long Press
		this.longPress(this.touch);

		/*if (iGUICode_CorneaScene.getInstance ().floatHorizontalSlider1.value == 0f && this.corneaTag == 0) // If the cornea is not subject of deformation
			// double tap
			this.doubleTap (this.touch, "corneaCollider", "BFSAnt", iGUICode_CorneaScene.getInstance().label148, this.crossTarget1);
		else if(iGUICode_CorneaScene.getInstance ().floatHorizontalSlider1.value != 0f && this.corneaTag == 0)
		    iGUICode_CorneaScene.getInstance ().label148.label.text = "";
		else if (iGUICode_CorneaScene.getInstance ().floatHorizontalSlider2.value == 0f && this.corneaTag == 1)
			this.doubleTap(this.touch, "corneaPost", "BFSPost", iGUICode_CorneaScene.getInstance().label150, this.crossTarget2);		
		else
			iGUICode_CorneaScene.getInstance ().label150.label.text = "";
*/
	}

	private void doubleTap(Touch touch, String corneaTag, String BFSTag, iGUILabel label, GameObject crossTarget) 
	{
		if(touch.tapCount == 2 && Camera.main.orthographicSize > 0)
		{
			/*GameObject helpPosition = new GameObject();
			Quaternion quater = this.corneaPivot.transform.rotation;
			this.corneaPivot.transform.rotation = MobileUnityInterface.corneaQuaternion;
			helpPosition.transform.position = this.corneaPivot.transform.position;
			print(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.z)) - (helpPosition.transform.position - MobileUnityInterface.corneaPosition));
			this.corneaPivot.transform.rotation = quater;*/
			// Bit shift the index of the layer (8) to get a bit mask
			int layerMask1 = 1 << 8;
			int layerMask2 = 0;
			if(this.corneaTag == 0)
				layerMask2 = 1 << 10;
			else if(this.corneaTag == 1)
				layerMask2 = 1 << 9;

			int layerMask = layerMask1 | layerMask2;
			// This would cast rays only against colliders in layer 8.
			// But instead we want to collide against everything except layer 8. The ~ operator does this, it invers a bitmask.
			layerMask = ~layerMask;

			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				if(hit.collider.gameObject.CompareTag(corneaTag))
				{	
					Vector3 BFSCenter = GameObject.FindGameObjectWithTag(BFSTag).transform.position;
					float distance = Convert.ToSingle(Math.Sqrt(Math.Pow(hit.point.x - BFSCenter.x, 2) + 
				    	                                        Math.Pow(hit.point.y - BFSCenter.y, 2) +
				        	                                    Math.Pow(hit.point.z - BFSCenter.z, 2))) - this.BFSRadius;
					label.label.text = Convert.ToString(distance * 1000);
					crossTarget.renderer.enabled = true;
					crossTarget.transform.position = hit.point + new Vector3(-0.225f, -0.225f, 0.1f);
				}
		}
	}
	
	private void longPress(Touch touch) 
	{
		if(touch.phase == TouchPhase.Stationary && (Time.time - touchTime) > longPressTime && this.isRotating == false)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			if(Physics.Raycast(ray, out hit))
			{
				this.corneaPivot.transform.position = this.corneaCenter;
				this.corneaPivot.transform.rotation = MobileUnityInterface.corneaQuaternion;
				Camera.main.transform.position = new Vector3(this.corneaCenter.x, 3.1f, 0f);
				Camera.main.orthographicSize = 5f;
				iGUICode_CorneaScene.getInstance ().container1.setHeight (1530f);
			}
		}
	}

	private void rotate(Touch touch) 
	{
		// Only deal with input if the finger has moved
		if (touch.phase == TouchPhase.Moved) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			if(Physics.Raycast(ray, out hit))
			{
				this.isRotating = true;
				// On ne cherche pas à savoir quel collider est frappé parce qu'on fait tourner la cornée sur elle-même
				// Get the movement of the finger since last frame
				Vector2 touchDeltaPosition = touch.deltaPosition;
				if(Camera.main.transform.rotation == cameraQuaternion)
				{
					this.corneaPivot.transform.RotateAround(this.corneaPivot.transform.position, Vector3.left, Time.deltaTime * speedFactor * touchDeltaPosition.x);
					this.corneaPivot.transform.RotateAround(this.corneaPivot.transform.position, Vector3.up, Time.deltaTime * speedFactor * touchDeltaPosition.y);
				}
				else
				{
					this.corneaPivot.transform.RotateAround(this.corneaPivot.transform.position, Vector3.down, Time.deltaTime * speedFactor * touchDeltaPosition.x);
					this.corneaPivot.transform.RotateAround(this.corneaPivot.transform.position, Vector3.left, Time.deltaTime * speedFactor * touchDeltaPosition.y);
				}
			}
		}
	}

	private void rotatePivot(Touch touch1, Touch touch2)
	{
		// check the delta angle between them ...
		float turnAngle = Angle(touch1.position, touch2.position);
		float prevTurn = Angle(touch1.position - touch1.deltaPosition,
		                       touch2.position - touch2.deltaPosition);
		float turnAngleDelta = Mathf.DeltaAngle(prevTurn, turnAngle);

		if(Mathf.Abs(turnAngleDelta) > 1f)
			this.isPivotRotating = true;

		// If it's greater than a minimum threshold, it's a turn!
		if(this.isPivotRotating == true && Mathf.Abs(turnAngleDelta) > 0.25f) // Rotate
		{
			Vector3 rotationDeg = Vector3.zero;
			rotationDeg.z = -turnAngleDelta;

			Ray ray;
			RaycastHit hit;

			// Bit shift the index of the layer (8) to get a bit mask
			int layerMask1 = 1 << 8;
			int layerMask2 = 1 << 10;

			int layerMask = layerMask1 | layerMask2;

			// This would cast rays only against colliders in layer 8.
			// But instead we want to collide against everything except layer 8. The ~ operator does this, it invers a bitmask.
			layerMask = ~layerMask;

			if(touch1.phase == TouchPhase.Stationary)
				ray = Camera.main.ScreenPointToRay(touch2.position);
			else if(touch2.phase == TouchPhase.Stationary)
				ray = Camera.main.ScreenPointToRay(touch1.position);
			else
				ray = Camera.main.ScreenPointToRay(GetMidPoint(touch1, touch2));

			if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				if(hit.collider.gameObject.CompareTag("corneaCollider"))
					this.corneaPivot.transform.Rotate(rotationDeg * this.rotatingDegSpeed, Space.World);
		}
		turnAngle = 0f;
		turnAngleDelta = 0f;
	}

	private Vector2 GetMidPoint(Touch touch1, Touch touch2) 
	{
		return Vector2.Lerp (touch1.position, touch2.position, 0.5f);
	}

	private static float Angle(Vector2 pos1, Vector2 pos2) 
	{
		Vector2 from = pos2 - pos1;
		Vector2 to = new Vector2 (1f, 0f);

		float result = Vector2.Angle (from, to);
		Vector3 cross = Vector3.Cross (from, to);

		if (cross.z > 0)
			result = 360f - result;
		return result;
	}

	private void zoom(Touch touch1, Touch touch2) 
	{
		Vector2 centerTouches = GetMidPoint(touch1, touch2);

		// Check the delta distance between them...
		float pinchDistance = Vector2.Distance(touch1.position, touch2.position);
		float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
		                                      touch2.position - touch2.deltaPosition);
		float pinchDistanceDelta = pinchDistance - prevDistance;

		if (Mathf.Abs (pinchDistanceDelta) > 1f)
			this.isZoom = true;

		// If it's greater than a minimum threshold, it's a pinch!
		if(this.isZoom == true && Mathf.Abs(pinchDistanceDelta) > 0.25f) // zoom
		{
			// Create ray from the camera and passing through the touch position:
			Ray ray = Camera.main.ScreenPointToRay(centerTouches);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
				Vector3 coordHG = Vector3.zero;
				Vector3 coordBD = Vector3.zero;
				Vector3 newPos = Vector3.zero;
				Vector3 worldCenterTouch = Camera.main.ScreenToWorldPoint(new Vector3(centerTouches.x, centerTouches.y, Camera.main.transform.position.z));

				if(pinchDistanceDelta > 0)
					newPos = Camera.main.transform.position + ((worldCenterTouch - Camera.main.transform.position) / 5);
				else
					newPos = Camera.main.transform.position + ((Camera.main.transform.position - worldCenterTouch) / 5);

				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, Mathf.Abs(pinchDistanceDelta * 3f/pinchDistance) * this.zoomSpeed);
				Camera.main.orthographicSize  -= (Camera.main.orthographicSize / 5) * pinchDistanceDelta * 3f/pinchDistance * this.zoomSpeed;

				//Make sure the orthographic size never drops under 2 cm2
				Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, 5f * 2f);
				coordHG = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width - Screen.height, Screen.height, Camera.main.transform.position.z));
				coordBD = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width, 0f, Camera.main.transform.position.z));

				if(this.countFrame == 6)
				{
					this.setRulerDimension(Vector3.Distance (coordBD, coordHG));
					this.countFrame = -1;
				}
				this.countFrame++;
			}
		}
	}

	private void translate(Touch touch1, Touch touch2) 
	{
		Vector2 screenPoint = (touch1.deltaPosition + touch2.deltaPosition) / 2;
		Vector2 deltaPos = GetMidPoint(touch1, touch2);

		// Bit shift the index of the layer (9) to get a bit mask
		int layerMask1 = 1 << 9;
		int layerMask2 = 1 << 10;

		int layerMask = layerMask1 | layerMask2;

		// This would cast rays only against colliders in layer 9.
		// But instead we want to collide against everything except layer 9. The ~ operator does this, it invers a bitmask.
		layerMask = ~layerMask;

		if (Mathf.Abs (Vector2.Distance (screenPoint, deltaPos)) > 1f)
			this.isTranslated = true;

		if(this.isTranslated == true && Mathf.Abs(Vector2.Distance(screenPoint, deltaPos)) > 0.25f)
		{
			Ray ray = Camera.main.ScreenPointToRay(deltaPos);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				if(hit.collider.gameObject.CompareTag("zoneCollider"))
					if(Camera.main.transform.rotation == cameraQuaternion)
						this.corneaPivot.transform.position += new Vector3(screenPoint.y, screenPoint.x, 0f) * this.translatedSpeed * Camera.main.orthographicSize/22;
			else
				this.corneaPivot.transform.position += new Vector3(-screenPoint.x, screenPoint.y, 0f) * this.translatedSpeed * Camera.main.orthographicSize/22;
		}
	}

	// Adapt the ruler size to the zoom scale of the camera
	public void setRulerDimension(float diagonaleScaleCamera)
	{
		iGUICode_CorneaScene.getInstance ().container1.setHeight (Mathf.Sqrt (200)/diagonaleScaleCamera * 1530f);
	}
}
