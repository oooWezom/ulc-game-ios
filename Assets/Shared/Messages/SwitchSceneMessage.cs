using System;

namespace Assets.Shared.Messages
{
	[Serializable]
	public class SwitchSceneMessage : Message
	{
		public String scene;
	}
}

