using HalfDog.EasyInteractive;
using System;
using System.Collections.Generic;

public static class IInteractableExtension
{
	/// <summary>
	/// 判断两个交互对象是否属于同一个交互情景
	/// </summary>
	public static bool IsBelongToSameCase(this IInteractable self, IInteractable other)
	{
		return EasyInteractive.Instance.InteractRelationMapper.TryGetValue(self.interactTag, out List<Type> types) && types.Contains(other.interactTag);
	}
}
