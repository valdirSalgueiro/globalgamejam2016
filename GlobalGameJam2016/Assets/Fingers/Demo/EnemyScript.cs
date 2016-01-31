using UnityEngine;
using System.Collections;
using AssemblyCSharp;

namespace DigitalRubyShared
{
	public class EnemyScript : MonoBehaviour
	{
		public float speed = 1.0f;
		public EnemyType enemyType;
		public Transform target;
		public Sprite nav_tap;
		public Sprite nav_doubletap;
		public Sprite nav_swipe;

		private void Start ()
		{
			enemyType = typeGenerator ();
			//this.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		}

		private void Update ()
		{
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed * Time.deltaTime);
			Vector3 dir = -transform.position;
			dir.Normalize();
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward)*Quaternion.Euler(0,0,-90);

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

		private void OnBecameInvisible()
		{
			GameObject.Destroy(gameObject);
		}
	}
}