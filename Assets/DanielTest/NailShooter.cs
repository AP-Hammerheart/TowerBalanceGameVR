using UnityEngine;

public class NailShooter : Usable
{
	//SoundStuff
	public AK.Wwise.Event NailGunEv;
	public Nail nailPrefab;
	public BetterNail betterNailPrefab;
	public float speed = 5f;

	public void Fire()
	{
		if (betterNailPrefab != null)
		{
			NailGunEv.Post(gameObject);
			var nail = Instantiate(betterNailPrefab, transform.position, Quaternion.LookRotation(transform.forward) * Quaternion.Euler(-90, 0, 0));
			nail.Fly(transform.forward * speed);
		}
		else
		{
			NailGunEv.Post(gameObject);
			var nail = Instantiate(nailPrefab, transform.position, Quaternion.LookRotation(transform.forward) * Quaternion.Euler(-90, 0, 0));
			nail.Fly(transform.forward * speed);
		}
		
	}

	public override void Use()
	{
		Fire();
	}
}
