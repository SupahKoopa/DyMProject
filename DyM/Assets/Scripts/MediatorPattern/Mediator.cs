﻿using System;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class Mediator : MonoBehaviour
	{
		[Inject]
		protected IMessageDispatcher messageDispatcher;
		protected static PhysicsDirector physicsDirector;

		[Inject]
		protected OurColliderFactory ourColliderFactory;
		protected OurCollider ourCollider;

		public OurCollider GetOurCollider
		{
			get { return ourCollider; }
		}
	}
}
