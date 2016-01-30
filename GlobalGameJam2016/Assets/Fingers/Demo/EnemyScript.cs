using UnityEngine;
using System.Collections;

namespace DigitalRubyShared
{
	public class EnemyScript : MonoBehaviour
	{
		private void Start ()
		{
		
		}

		private void Update ()
		{
			
		}

		private void OnBecameInvisible()
		{
			GameObject.Destroy(gameObject);
		}
	}
}