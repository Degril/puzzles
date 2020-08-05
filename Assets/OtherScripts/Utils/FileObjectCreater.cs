using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Globalization;

public static class FileObjectCreater
{
    public static void CreateFileObj(Vector2[] uvs, Vector3[] verts, int[] tris)
    {
        using (var file = File.Open(System.Environment.CurrentDirectory + "/1.obj", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        {
            using (var sw = new StreamWriter(file))
            {
                foreach (var vert in verts)
                {
                    sw.WriteLine($"v {vert.x.ToString("F6", CultureInfo.InvariantCulture)} {vert.y.ToString("F6", CultureInfo.InvariantCulture)} {vert.z.ToString("F6", CultureInfo.InvariantCulture)}");
                }
                foreach (var uv in uvs)
                {
                    sw.WriteLine($"vt {uv.x.ToString("F6", CultureInfo.InvariantCulture)} {uv.y.ToString("F6", CultureInfo.InvariantCulture)}");
                }
                //foreach (var uv in mesh.uv2)
                //{
                //    sw.WriteLine($"vt {uv.x.ToString("F6", CultureInfo.InvariantCulture)} {uv.y.ToString("F6", CultureInfo.InvariantCulture)}");
                //}
                //sw.WriteLine("o t1");
                for (int i = 0; i < tris.Length; i += 3)
                {
                    sw.WriteLine($"f {tris[i] + 1}/{tris[i] + 1}" +
                        $" {tris[i + 1] + 1}/{tris[i + 1] + 1}" +
                        $" {tris[i + 2] + 1}/{tris[i + 2] + 1}");
                }
                //sw.WriteLine("o t2");
                //for (int i = 0; i < mesh.triangles.Length; i += 3)
                //{
                //    sw.WriteLine($"f {mesh.triangles[i] + 1}/{mesh.triangles[i] + 1 + mesh.uv.Length}" +
                //        $" {mesh.triangles[i + 1] + 1}/{mesh.triangles[i + 1] + 1 + mesh.uv.Length}" +
                //        $" {mesh.triangles[i + 2] + 1}/{mesh.triangles[i + 2] + 1 + mesh.uv.Length}");
                //}

            }
        }

    }
}
