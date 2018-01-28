using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class DelayDestroy : MonoBehaviour
	{
        public float LifeTimeSecs = 15;
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
