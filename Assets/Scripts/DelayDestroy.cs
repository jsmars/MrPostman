using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class DelayDestroy : MonoBehaviour
	{
        float LifeTimeSecs = 30;
        float lifeLeft;

		public void Start()
		{
            Reset();
		}

        public void Update()
        {
            lifeLeft -= Time.deltaTime;
            if (lifeLeft < 0)
                Destroy(gameObject);
        }

        public void Reset()
        {
            lifeLeft = LifeTimeSecs;
        }
	}
}
