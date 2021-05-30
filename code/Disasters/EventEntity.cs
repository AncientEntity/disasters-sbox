using Sandbox;
using System;

namespace Disasters
{
	public abstract class EventEntity : Entity
	{
		public bool enabled { get; set; }

		public abstract void OnDisasterStart();
		public abstract void WhileEvent();

	}
}
