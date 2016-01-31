using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp;
using UnityEngine.SceneManagement;

namespace DigitalRubyShared
{
	public class TutorialScript : MonoBehaviour
	{
		public FingersScript FingerScript;
		public GameObject Earth;
		public UnityEngine.UI.Text dpiLabel;
		public GameObject AsteroidPrefab;
		public Material LineMaterial;
		public GameObject[] TouchCircles;

		private Sprite[] asteroids;

		private TapGestureRecognizer tapGesture;
		private TapGestureRecognizer doubleTapGesture;
		private SwipeGestureRecognizer swipeGesture;
		private PanGestureRecognizer panGesture;
		private ScaleGestureRecognizer scaleGesture;
		private RotateGestureRecognizer rotateGesture;
		private LongPressGestureRecognizer longPressGesture;

		private float nextAsteroid = float.MinValue;
		private GameObject draggingAsteroid;
		int alive = 3;

		private bool shaking = false;
		Vector3 originalCamPos;
		float elapsed = 0.0f;

		private readonly List<Vector3> lines = new List<Vector3>();

		private GestureTouch FirstTouch(ICollection<GestureTouch> touches)
		{
			foreach (GestureTouch t in touches)
			{
				return t;
			}
			return new GestureTouch();
		}

		private void DebugText(string text, params object[] format)
		{
			Debug.Log(string.Format (text, format));
		}

		private GameObject CreateAsteroid(float screenX, float screenY)
		{
			GameObject o = GameObject.Instantiate(AsteroidPrefab);
			SpriteRenderer r = o.GetComponent<SpriteRenderer>();
			r.sprite = asteroids[UnityEngine.Random.Range (0, asteroids.Length - 1)];

			if (screenX == float.MinValue || screenY == float.MinValue)
			{
				float x = UnityEngine.Random.Range (Camera.main.rect.min.x, Camera.main.rect.max.x);
				float y = UnityEngine.Random.Range (Camera.main.rect.min.y, Camera.main.rect.max.y);
				Vector3 pos = new Vector3(x, y, 0.0f);
				pos = Camera.main.ViewportToWorldPoint(pos);
				pos.z = o.transform.position.z;
				o.transform.position = pos;
			}
			else
			{
				Vector3 pos = new Vector3(screenX, screenY, 0.0f);
				pos = Camera.main.ScreenToWorldPoint(pos);
				pos.z = o.transform.position.z;
				o.transform.position = pos;
			}

			o.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range (0.0f, 30.0f);
			Vector2 velocity = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range (0, 30.0f);
			o.GetComponent<Rigidbody2D>().velocity = velocity;
			float scale = UnityEngine.Random.Range (1.0f, 4.0f);
			o.transform.localScale = new Vector3(scale, scale, 1.0f);
			o.GetComponent<Rigidbody2D>().mass *= (scale * scale);

			return o;
		}

		private void RemoveAsteroids(float screenX, float screenY, float radius, EnemyType enemyType)
		{
			shaking = true;
			Vector3 pos = new Vector3(screenX, screenY, 0.0f);
			pos = Camera.main.ScreenToWorldPoint(pos);

			RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, radius, Vector2.zero);
			foreach (RaycastHit2D h in hits)
			{
				//Debug.Log ("[PEDRO] PEGOU " + h.transform.gameObject.GetComponent<EnemyScript>().enemyType.ToString() );

				if (h.transform.gameObject.GetComponent<EnemyForTutorial> ().enemyType == enemyType) {
					GameObject.Destroy (h.transform.gameObject);
					alive--;
				}
			}
		}

		private void BeginDrag(float screenX, float screenY)
		{
			Vector3 pos = new Vector3(screenX, screenY, 0.0f);
			pos = Camera.main.ScreenToWorldPoint(pos);
			RaycastHit2D hit = Physics2D.CircleCast(pos, 10.0f, Vector2.zero);
			if (hit.transform != null && hit.transform.gameObject.name.StartsWith("asteroid", System.StringComparison.OrdinalIgnoreCase))
			{
				draggingAsteroid = hit.transform.gameObject;
				draggingAsteroid.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				draggingAsteroid.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
			}
			else
			{
				longPressGesture.Reset();
			}
		}

		private void DragTo(float screenX, float screenY)
		{
			if (draggingAsteroid == null)
			{
				return;
			}

			Vector3 pos = new Vector3(screenX, screenY, 0.0f);
			pos = Camera.main.ScreenToWorldPoint(pos);
			draggingAsteroid.GetComponent<Rigidbody2D>().position = pos;
		}

		private void EndDrag(float velocityXScreen, float velocityYScreen)
		{
			if (draggingAsteroid == null)
			{
				return;
			}

			Vector3 origin = Camera.main.ScreenToWorldPoint(Vector3.zero);
			Vector3 end = Camera.main.ScreenToWorldPoint(new Vector3(velocityXScreen, velocityYScreen, 0.0f));
			Vector3 velocity = (end - origin);
			draggingAsteroid.GetComponent<Rigidbody2D>().velocity = velocity;
			draggingAsteroid.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(5.0f, 45.0f);
			draggingAsteroid = null;

			DebugText("Long tap flick velocity: {0}", velocity);
		}
		
		private void HandleSwipe(float endX, float endY)
		{
			Vector2 start = new Vector2(swipeGesture.StartX, swipeGesture.StartY);
			Vector3 startWorld = Camera.main.ScreenToWorldPoint(start);
			Vector3 endWorld = Camera.main.ScreenToWorldPoint(new Vector2(endX, endY));
			float distance = Vector3.Distance(startWorld, endWorld);
			startWorld.z = endWorld.z = 0.0f;

			lines.Add(startWorld);
			lines.Add(endWorld);

			if (lines.Count > 4)
			{
				lines.RemoveRange(0, lines.Count - 4);
			}

			RaycastHit2D[] collisions = Physics2D.CircleCastAll(startWorld, 0.25f, (endWorld - startWorld).normalized, distance);

			if (collisions.Length != 0)
			{
				Debug.Log ("Raycast hits: " + collisions.Length + ", start: " + startWorld + ", end: " + endWorld + ", distance: " + distance);

				Vector3 origin = Camera.main.ScreenToWorldPoint(Vector3.zero);
				Vector3 end = Camera.main.ScreenToWorldPoint(new Vector3(swipeGesture.VelocityX, swipeGesture.VelocityY, 0.0f));
				Vector3 velocity = (end - origin);
				Vector2 force = velocity * 500.0f;

				foreach (RaycastHit2D h in collisions)
				{
					if (h.transform.gameObject.GetComponent<EnemyForTutorial> ().enemyType == EnemyType.SWIPE) {
						shaking = true;
						GameObject.Destroy (h.transform.gameObject);
						alive--;
					}
					//h.rigidbody.gameObject.SetActive (false);
					//h.rigidbody.AddForceAtPosition(force, h.point);
				}
			}
		}

		private void TapGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
		{
			if (gesture.State == GestureRecognizerState.Ended)
			{
				GestureTouch t = FirstTouch (touches);
				if (t.IsValid())
				{
					DebugText("Tapped at {0}, {1}", t.X, t.Y);
					RemoveAsteroids(t.X, t.Y, 0.5f, EnemyType.SINGLE);
					//CreateAsteroid(t.X, t.Y);
				}
			}
		}
		
		private void CreateTapGesture()
		{
			tapGesture = new TapGestureRecognizer();
			tapGesture.Updated += TapGestureCallback;
			FingerScript.AddGesture(tapGesture);
		}

		private void DoubleTapGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
		{
			if (gesture.State == GestureRecognizerState.Ended)
			{
				GestureTouch t = FirstTouch (touches);
				if (t.IsValid())
				{
					DebugText("Double tapped at {0}, {1}", t.X, t.Y);
					RemoveAsteroids (t.X, t.Y, 0.5f, EnemyType.DOUBLE);
				}
			}
		}
		
		private void CreateDoubleTapGesture()
		{
			doubleTapGesture = new TapGestureRecognizer();
			doubleTapGesture.NumberOfTapsRequired = 2;
			doubleTapGesture.Updated += DoubleTapGestureCallback;
			FingerScript.AddGesture(doubleTapGesture);
		}

		private void SwipeGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
		{
			if (gesture.State == GestureRecognizerState.Ended)
			{
				GestureTouch t = FirstTouch (touches);
				if (t.IsValid())
				{
					HandleSwipe(t.X, t.Y);
					DebugText("Swiped at {0}, {1}; velocity: {2}, {3}", t.X, t.Y, swipeGesture.VelocityX, swipeGesture.VelocityY);
				}
			}
		}

		private void CreateSwipeGesture()
		{
			swipeGesture = new SwipeGestureRecognizer();
			swipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
			swipeGesture.Updated += SwipeGestureCallback;
			swipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
			FingerScript.AddGesture(swipeGesture);
		}

		private void PanGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
		{
			if (gesture.State == GestureRecognizerState.Executing)
			{
				GestureTouch t = FirstTouch (touches);
				if (t.IsValid())
				{
					DebugText("Panned, {0}, {1}", t.X, t.Y);
					
					float deltaX = panGesture.DeltaX / 25.0f;
					float deltaY = panGesture.DeltaY / 25.0f;
					Vector3 pos = Earth.transform.position;
					pos.x += deltaX;
					pos.y += deltaY;
					Earth.transform.position = pos;
				}
			}
		}
		
		private void CreatePanGesture()
		{
			panGesture = new PanGestureRecognizer();
			panGesture.MinimumNumberOfTouchesRequired = panGesture.MaximumNumberOfTouchesAllowed = 2;
			panGesture.Updated += PanGestureCallback;
			FingerScript.AddGesture(panGesture);
		}

		private void ScaleGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
		{
			if (gesture.State == GestureRecognizerState.Executing)
			{
				DebugText("Scaled: {0}, Focus: {1}, {2}", scaleGesture.ScaleMultiplier, scaleGesture.FocusX, scaleGesture.FocusY);
				Earth.transform.localScale *= scaleGesture.ScaleMultiplier;
			}
		}
		
		private void CreateScaleGesture()
		{
			scaleGesture = new ScaleGestureRecognizer();
			scaleGesture.Updated += ScaleGestureCallback;
			FingerScript.AddGesture(scaleGesture);
		}

		private void RotateGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
		{
			if (gesture.State == GestureRecognizerState.Executing)
			{
				Earth.transform.Rotate(0.0f, 0.0f, rotateGesture.RotationRadiansDelta * Mathf.Rad2Deg);
			}
		}
		
		private void CreateRotateGesture()
		{
			rotateGesture = new RotateGestureRecognizer();
			rotateGesture.Updated += RotateGestureCallback;
			FingerScript.AddGesture(rotateGesture);
		}

		private void LongPressGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
		{
			GestureTouch t = FirstTouch (touches);
			if (gesture.State == GestureRecognizerState.Began)
			{
				DebugText("Long press began: {0}, {1}", t.X, t.Y);
				BeginDrag(t.X, t.Y);
			}
			else if (gesture.State == GestureRecognizerState.Executing)
			{
				DebugText("Long press moved: {0}, {1}", t.X, t.Y);
				DragTo(t.X, t.Y);
			}
			else if (gesture.State == GestureRecognizerState.Ended)
			{
				DebugText("Long press end: {0}, {1}, delta: {2}, {3}", t.X, t.Y, t.DeltaX, t.DeltaY);
				EndDrag(longPressGesture.VelocityX, longPressGesture.VelocityY);
			}
		}
		
		private void CreateLongPressGesture()
		{
			longPressGesture = new LongPressGestureRecognizer();
			longPressGesture.Updated += LongPressGestureCallback;
			FingerScript.AddGesture(longPressGesture);
		}

		private void Start()
		{
			originalCamPos = Camera.main.transform.position;

			CreateTapGesture();
			CreateDoubleTapGesture();
			CreateSwipeGesture();
			CreatePanGesture();
			CreateScaleGesture();
			CreateRotateGesture();
			CreateLongPressGesture();

			// single tap gesture requires that the double tap gesture fail
			tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;

			// pan, scale and rotate can all happen simultaneously
			panGesture.AllowSimultaneousExecution(scaleGesture);
			panGesture.AllowSimultaneousExecution(rotateGesture);
			scaleGesture.AllowSimultaneousExecution(rotateGesture);
		}
		
		private void LateUpdate()
		{

			if (alive == 0)
				SceneManager.LoadSceneAsync("DemoScene");// .LoadScene("DemoScene");
			
			if (Time.timeSinceLevelLoad > nextAsteroid)
			{
				nextAsteroid = Time.timeSinceLevelLoad + UnityEngine.Random.Range (1.0f, 4.0f);
				//SpawnEnemy ();
				//CreateAsteroid(float.MinValue, float.MinValue);
			}

			if (TouchCircles != null && TouchCircles.Length != 0)
			{
				int index = 0;
				foreach (GestureTouch t in FingerScript.Touches)
				{
					GameObject obj = TouchCircles[index++];
					obj.SetActive(true);
					obj.transform.position = new Vector3(t.X, t.Y);
				}
				while (index < TouchCircles.Length)
				{
					TouchCircles[index++].gameObject.SetActive(false);
				}
			}

			if (shaking) {
				Handheld.Vibrate ();
				Shake ();
			}
		}

		private void OnRenderObject()
		{
			GL.PushMatrix();
			LineMaterial.SetPass(0);
			GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
			GL.Begin (GL.LINES);
			for (int i = 0; i < lines.Count; i++)
			{
				GL.Color(Color.white);
				GL.Vertex(lines[i]);
				GL.Vertex(lines[++i]);
			}
			GL.End();
			GL.PopMatrix();
		}


		private void SpawnEnemy()
		{
			Vector3 position = Vector3.zero;
			int side = Random.Range(0, 4);

			switch (side) {
			case 0:
				//left
				position = Camera.main.ViewportToWorldPoint (new Vector3 (-0.1f, Random.Range (0.0f, 1.0f), 0.0f));
				break;
			case 1:
				//right
				position = Camera.main.ViewportToWorldPoint (new Vector3 (1.1f, Random.Range (0.0f, 1.0f), 0.0f));
				break;
			case 2:
				//up
				position = Camera.main.ViewportToWorldPoint (new Vector3 (Random.Range (0.0f, 1.0f), -0.1f, 0.0f));
				break;
			case 3:
				//down
				position = Camera.main.ViewportToWorldPoint (new Vector3 (Random.Range (0.0f, 1.0f), 1.1f, 0.0f));
				break;
			}

			Instantiate(AsteroidPrefab, position, Quaternion.identity);
			//yield return new WaitForSeconds(Random.Range(0, 3));
		}

		public void Shake() {

			float duration = 0.5f;

			//Vector3 originalCamPos = Camera.main.transform.position;

			//while (elapsed < duration) {

				elapsed += Time.deltaTime;          

				float percentComplete = elapsed / duration;         
				float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

				// map value to [-1, 1]
				float x = Random.value * 2f - 1.0f;
				float y = Random.value * 2f - 1.0f;
				x *= 0.06f * damper;
				y *= 0.06f * damper;

				//Debug.Log ("[PEDRO] Original: ");
				Camera.main.transform.position = new Vector3(x, originalCamPos.y, originalCamPos.z);

			//}

			Debug.Log ("[PEDRO] SHAKE " + elapsed.ToString() + " - " + duration.ToString());
			if (elapsed > duration) {
				elapsed = 0.0f;
				shaking = false;
				Camera.main.transform.position = originalCamPos;
			}
		}

		public void StartGame()
		{
			SceneManager.LoadSceneAsync("DemoScene");
		}
	}
}
