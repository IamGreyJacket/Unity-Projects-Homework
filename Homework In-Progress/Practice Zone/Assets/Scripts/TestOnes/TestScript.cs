using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Method()
    {

    }
    
    private void CreateFile(string fileName = "NewFile.txt", string path = null)
    {
        if(path == null)
        {
            path = Environment.CurrentDirectory;
        }
        Debug.Log($"Current direcrory is: \"{path}\"");
        using (var file = File.Create($"{path}\\{fileName}"))
        {
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine($"Some information, like...\nDirectory: \"{path}\"\nOr File: \"{file.Name}\"");
            }
        }
        Debug.Log($"File \"{fileName}\" created with \"{path}\" path.");
    }
}
