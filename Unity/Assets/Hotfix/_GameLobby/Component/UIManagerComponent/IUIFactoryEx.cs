using UnityEngine;

namespace ETHotfix
{
	public interface IUIFactoryEx
	{
		UIEx Create(Scene scene, string type, GameObject parent);
		void Remove(string type);
	}
}