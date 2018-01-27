using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class DelayDestroy : MonoBehaviour
	{
		public void Start()
		{
			StartCoroutine(Destroy());
		}

		public IEnumerator Destroy()
		{
			yield return new WaitForSeconds(15);
			Destroy(gameObject);
		}
	}
}
