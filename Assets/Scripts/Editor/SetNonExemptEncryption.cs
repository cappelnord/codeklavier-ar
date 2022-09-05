// C# example:
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

#if UNITY_IOS
[DefaultExecutionOrder(1000)]
public class SetNonExemptEncryption {
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {

        string plistPath = pathToBuiltProject + "/Info.plist";
        string plistSource = File.ReadAllText(plistPath);

        if(!plistSource.Contains("ITSAppUsesNonExemptEncryption")) {
            plistSource = plistSource.Replace("</dict>\n", "<key>ITSAppUsesNonExemptEncryption</key>\n<false/>\n</dict>\n");
        }

        File.WriteAllText(plistPath, plistSource);

    }
}
#endif