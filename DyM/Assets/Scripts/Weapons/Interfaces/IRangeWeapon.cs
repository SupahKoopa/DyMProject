//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Weapons.Interfaces
{
	public interface IRangeWeapon : IWeapon
	{
		IProjectile Fire();
		IProjectile Projectile { get; }
		bool FireRate(float time);
        ICharacter Character { get; set; }
	}
}

