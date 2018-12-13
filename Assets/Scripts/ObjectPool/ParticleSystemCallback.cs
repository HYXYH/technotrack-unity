using UnityEngine;
 
public class ParticleSystemCallback: MonoBehaviour
{
	public void OnParticleSystemStopped()
	{
		GetComponent<PoolObject>().ReturnToPool();
	}
}
