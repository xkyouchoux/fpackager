
using System.IO.Compression;
using System.Text.Json.Nodes;

if (File.Exists(args[0] + "/info.json"))
{
    var directory = new DirectoryInfo(args[0]);
    var info = JsonObject.Parse(File.ReadAllText(directory + "/info.json"));
    var outputNameLessExtension = directory.Name + "_" + info["version"];
    var outputName = outputNameLessExtension + ".zip";
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

    foreach (var file in Directory.GetFiles(args[1]))
    {
        if (file.Contains(directory.Name + "_"))
        {
            File.Delete(file);
        }
    }
    
    File.Copy(outputPath, args[1] + "/" + outputName, true);
}