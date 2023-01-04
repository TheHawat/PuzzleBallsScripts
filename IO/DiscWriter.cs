using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;

internal class DiscWriter
{
    private String _rootPath;
    private String _sceneSubpath;
    public DiscWriter(String RootPath, String SceneSubpath) {
        _rootPath = RootPath;
        _sceneSubpath = SceneSubpath;
    }
    public void SaveScene(SceneConfig SC, String filename) {
        String path = _rootPath + _sceneSubpath;
        Directory.CreateDirectory(path);

        String filepath = $"{path}/{filename}";

        String sceneJSON = JsonConvert.SerializeObject(SC, Formatting.Indented, new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        String[] content = { sceneJSON };

        File.WriteAllLines(filepath, content);
    }
    // How to deserialze Vector2?
    // 
    // public async Task<SceneConfig> ReadScene(String filename) {
    //     String path = _rootPath + _sceneSubpath;
    //     String filepath = $"{path}/{filename}";

    //     String json = await File.ReadAllTextAsync(filepath);
    //     SceneConfig SC = JsonConvert.DeserializeObject<SceneConfig>(json);
    //     return SC;
    // }
}
