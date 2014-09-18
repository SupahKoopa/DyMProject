﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Projectiles.Interfaces
{
	public interface IPooledProjectile
	{
		IProjectile Projectile { get; }
		bool Active { get; set; }
	}
}
