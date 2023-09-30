
using System.IO.Compression;
using System.Text.Json.Nodes;

if (File.Exists(args[0] + "/info.json"))
{
    var directory = new DirectoryInfo(args[0]);
    var info = JsonObject.Parse(File.ReadAllText(directory + "/info.json"));
    var outputName = directory.Name + "_" + info["version"] + ".zip";
    var outputPath = directory + "/" + outputName;
    
    File.Delete(outputPath);

    using (var archive = ZipFile.Open(outputPath, ZipArchiveMode.Create))
    {
        foreach (var path in directory.GetFiles(".", SearchOption.AllDirectories))
        {
            if (!path.Name.EndsWith(".zip") && !path.FullName.Contains(".git") && !path.Name.EndsWith(".bat") && !path.FullName.Contains(".vscode"))
            {
                archive.CreateEntryFromFile(path.FullName.Replace("\\", "/"), (directory.Name + "\\" + path.FullName.Replace(directory.FullName + "\\", "")).Replace("\\", "/"));
            }
        }
    }

    File.Copy(outputPath, args[1] + "/" + outputName, true);
}