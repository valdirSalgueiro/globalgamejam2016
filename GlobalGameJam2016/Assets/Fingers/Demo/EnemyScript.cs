using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using DG.Tweening;

namespace DigitalRubyShared
{
	public class EnemyScript : MonoBehaviour
	{
		public float speed = 2.0f;
		public EnemyType enemyType;

//		float m_degrees;
//		float m_speed = 1.0f;
//		float m_amplitude = 0.05f;
//		float m_period = 1.0f;

//		public int movement;
		public Transform target;
		public Sprite nav_tap;
		public Sprite nav_doubletap;
		public Sprite nav_swipe;

		private void Start ()
		{
			enemyType = typeGenerator ();
//			movement = Random.Range (0, 2);
			//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		}

		private void Update ()
		{
//			if (movement == 0) {
				
				transform.position = Vector3.MoveTowards (transform.position, Vector3.zero, speed * Time.deltaTime);
				Vector3 dir = -transform.position;
				dir.Normalize ();
				float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward) * Quaternion.Euler (0, 0, -90);

//			} else {
				
//				transform.position = Vector3.MoveTowards (transform.position, Vector3.zero, speed * Time.deltaTime);
//				Vector3 dir = -transform.position;
//				dir.Normalize ();
//				float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
//				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward) * Quaternion.Euler (0, 0, -90);
//				transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
				// Update degrees
//				float degreesPerSecond = 360.0f / m_period;
//				m_degrees = Mathf.Repeat (m_degrees + (Time.deltaTime * degreesPerSecond), 360.0f);
//				float radians = m_degrees * Mathf.Deg2Rad;
				//
//				// Offset by sin wave
//				Vector3 offset = new Vector3 (m_amplitude * Mathf.Sin (radians), m_amplitude * Mathf.Sin (radians), 0.0f);
//				transform.position += offset;
//			}

			//if (Vector3.Distance(transform.position, Vector3.zero) < 1 )
			//{
			//	Destroy(gameObject);
			//}
		}

		private EnemyType typeGenerator()
		{
			int r = Random.Range(0, 3);

			switch (r) {
			case 0:
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = nav_tap;
				return EnemyType.SINGLE;
				break;
			case 1:
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = nav_doubletap;
				return EnemyType.DOUBLE;
				break;
			case 2:
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = nav_swipe;
				return EnemyType.SWIPE;
				break;
			default:
				this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				return EnemyType.SWIPE;
				break;
			}
		}

		public void bounce()
		{
			transform.localScale = Vector3.one * 0.1f;
			//if (Input.GetKeyDown (KeyCode.A))
			//transform.DOScale(transform.localScale* 1.2f, 0.1f).SetEase(Ease.InOutQuad).SetLoops(4,LoopType.Yoyo);
			//while(transform.DOScale(transform.localScale* 0.7f, 0.1f).SetEase(Ease.InOutQuad).SetLoops(3,LoopType.Yoyo).IsPlaying());
			//WaitForSeconds (StartCoroutine(GameObject.Destroy(this.gameObject)));
			//GameObject.Destroy(this.gameObject);
		}

		private void OnBecameInvisible()
		{
			GameObject.Destroy(gameObject);
		}
	}
}