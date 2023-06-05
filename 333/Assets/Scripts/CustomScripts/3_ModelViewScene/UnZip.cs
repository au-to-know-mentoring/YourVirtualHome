using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Compression;


public class UnZip : MonoBehaviour
{
    public UnZip()
    {
        // path of the zip file
        string zipPath = @"C: \Users\fired\OneDrive\Desktop\TestObj\TEST 8.zip";
        // destination of the contents
        string extractPath = @"C:\Users\fired\OneDrive\Desktop\TestObj\FakeAssetFolder";
        // extracts zipfile
        ZipFile.ExtractToDirectory(zipPath, extractPath);
    }
}



