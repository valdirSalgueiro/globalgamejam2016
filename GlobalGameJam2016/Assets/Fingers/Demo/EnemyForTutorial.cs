using UnityEngine;
using System.Collections;
using AssemblyCSharp;

namespace DigitalRubyShared
{
	public class EnemyForTutorial : MonoBehaviour
	{
		public EnemyType enemyType;
		public Transform target;

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