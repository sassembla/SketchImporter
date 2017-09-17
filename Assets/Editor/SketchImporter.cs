using UnityEditor.Experimental.AssetImporters;
using System.Text;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using UnityEditor;

[ScriptedImporter(1, "sketch")] public class SketchImporter : ScriptedImporter {
    public override void OnImportAsset(AssetImportContext ctx) {
		// set text assert for treat .sketch file as Unity asset.
		var textAsset = new TextAsset();
        ctx.SetMainAsset("MainAsset", textAsset);

		var path = ctx.assetPath;
		var fileName = Path.GetFileNameWithoutExtension(path);

		/*
			create Sketch file named folder for export files.
		 */
		var outputFolderPathParts = path.Split('/');

		// set filename as folder name.
		outputFolderPathParts[outputFolderPathParts.Length - 1] = fileName;

		var folderPath = string.Join("/", outputFolderPathParts);
		if (!Directory.Exists(folderPath)) {
			Directory.CreateDirectory(folderPath);
			AssetDatabase.Refresh();
		}

		// execute slice export command.
		var proc = new ProcessStartInfo("/usr/local/bin/sketchtool", "export slices " + path + " --output=" + folderPath);
		proc.UseShellExecute = false;
		
		var p = Process.Start(proc);
		p.WaitForExit();
		
		// refresh for add exporterd images.
		AssetDatabase.Refresh();
    }

	// public void Log (string message) {
	// 	using (var sw = new StreamWriter("log.txt", true)) {
	// 		sw.WriteLine(message);
	// 	}
	// }
}
