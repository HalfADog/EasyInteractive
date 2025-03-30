using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using System.Linq;

namespace HalfDog.EasyInteractive
{
    public static class EditorToolBarExtension
    {
        private static NamedBuildTarget buildTarget => NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
		[MenuItem("Settings/Interactive/Use 2D Mode")]
        private static void Change2DSymbol()
        {
			var defines = PlayerSettings.GetScriptingDefineSymbols(buildTarget).Split(';').ToList();
			bool is2DMode = defines.Contains("INTERACTIVE_2D_MODE");
			Menu.SetChecked("Settings/Interactive/Use 2D Mode",!is2DMode);
			if (is2DMode)
			{
				defines.Remove("INTERACTIVE_2D_MODE");
			}
			else 
			{
				defines.Add("INTERACTIVE_2D_MODE");
			}
			string strDef = string.Join(";", defines);
			PlayerSettings.SetScriptingDefineSymbols(buildTarget, strDef);
		}
    }
}
