
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

public class StackPlistEditor {

	[PostProcessBuild]
	public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject) {

		if (buildTarget == BuildTarget.iOS) {

			// Get plist
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));

			// Get root
			PlistElementDict rootDict = plist.root;

			// ADMOB
			var admobKey = "GADApplicationIdentifier";
			rootDict.SetString(admobKey, "ca-app-pub-9539815930599175~9858094409");

			// Write to file
			File.WriteAllText(plistPath, plist.WriteToString());
		}
	}
}