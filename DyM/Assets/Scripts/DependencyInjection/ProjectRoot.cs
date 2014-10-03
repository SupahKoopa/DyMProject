﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.CameraControl;
using Assets.Scripts.CameraControl.Interfaces;
using Assets.Scripts.Character;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.DependencyInjection
{
	public class ProjectRoot : MonoInstaller
	{
		public static GameObjectInstantiator Instantiator;
		public override void InstallBindings()
		{
			dependencyFrameworkBindings();
			 
			factoryBindings();

			movementBindings();
			
			cameraBindings();

			messengerBindings();

			characterBindings();
			
			rangeWeaponBindings();
			meleeWeaponBindings();

			abilityBindings();

			Instantiator = new GameObjectInstantiator(_container);
		}

		private void abilityBindings()
		{
			_container.Bind<IAbility>().ToTransient<Ability>();
		}

		private void rangeWeaponBindings()
		{
			_container.Bind<IRangeWeapon>().ToTransient<TestRangeWeapon>();
			_container.Bind<IProjectile>().ToTransient<Projectile>();
			_container.Bind<IPooledProjectile>().ToTransient<PooledProjectile>();
			_container.Bind<IBulletPool>().ToSingle<BulletPool>();
			_container.Bind<IPooledGameObjects>().ToSingle<PooledGameobjects>();
		}

		private void meleeWeaponBindings()
		{
			_container.Bind<IMeleeWeapon>().ToTransient<TestMeleeWeapon>();
		}

		private void characterBindings()
		{
			_container.Bind<ICharacter>().ToTransient<TestCharacter>();
		}

		private void messengerBindings()
		{
			_container.Bind<IMessageDispatcher>().ToSingle<MessageDispatcher>();
			_container.Bind<IReceiver>().ToTransient<Receiver>();
		}

		private void cameraBindings()
		{
			_container.Bind<ICameraLogic>().ToTransient<CameraLogic>();
		}

		private void movementBindings()
		{
			_container.Bind<ICardinalMovement>().ToTransient<CardinalMovement>();
			_container.Bind<IMovement>().ToTransient<Movement>();
		}

		private void factoryBindings()
		{
			_container.Bind<PlaneShiftFactory>().ToSingle();
		}

		private void dependencyFrameworkBindings()
		{
			_container.Bind<IInstaller>().ToSingle<StandardUnityInstaller>();
			_container.Bind<IDependencyRoot>().ToSingle<DependencyRootStandard>();
		}
	}
}
