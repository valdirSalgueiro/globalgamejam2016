using UnityEngine;
using System.Collections;

namespace DigitalRubyShared
{
	public class EnemyScript : MonoBehaviour
	{
		public float speed = 1.0f;

		private void Start ()
		{
		
		}

		private void Update ()
		{
			Debug.Log ("[PEDRO] teste");
			transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed * Time.deltaTime);

			if (Vector3.Distance(transform.position, Vector3.zero) < 1 )
			{
				Destroy(gameObject);
			}
		}

		private void OnBecameInvisible()
		{
			GameObject.Destroy(gameObject);
		}
	}
}