﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Weapons.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Interfaces
{
	public interface IBulletPool
	{
		void Initialize(IRangeWeapon rangeWeapon, Vector3 startPosition, int count);
		IPooledProjectile Projectile { set; }
		List<IPooledProjectile> Projectiles { get; }
		IPooledProjectile GetPooledProjectile();
		void DeactivatePooledProjectile(IProjectile projectile);
		void ChangeBullet(IRangeWeapon rangeWeapon);
	}
}
