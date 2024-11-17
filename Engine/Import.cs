using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Import
    {
        public static Mesh? fromObjectFile(string filename)
        {
            try
            {
                Mesh mesh = new Mesh();
                mesh.FaceMode = Mesh.Mode.LOAD;
                IEnumerable<string> file = File.ReadLines(filename);
                List<Vector3> Normals = [];

                foreach (string item in file)
                {
                    string[] parts = item.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length <= 0)
                    {
                        continue;
                    }

                    switch (parts[0])
                    {
                        case "#":
                            continue;
                        case "v":
                            mesh.Vertices.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                            break;
                        case "vt":
                            break;
                        case "vn":
                            Normals.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                            break;
                        case "f":
                            // Check if were dealing with normals or not.
                            if (parts[1].Contains('/'))
                            {
                                string[] v1 = parts[1].Split(new char[] { '/' });
                                string[] v2 = parts[2].Split(new char[] { '/' });
                                string[] v3 = parts[3].Split(new char[] { '/' });

                                Face face = new Face(mesh);

                                face.Vertex1 = int.Parse(v1[0]) - 1;
                                face.Vertex2 = int.Parse(v2[0]) - 1;
                                face.Vertex3 = int.Parse(v3[0]) - 1;

                                face.SetNormal(
                                    Normals[int.Parse(v1[2]) - 1],
                                    Normals[int.Parse(v2[2]) - 1],
                                    Normals[int.Parse(v3[2]) - 1]
                                );

                                mesh.Faces.Add(face);
                                break;

                            }
                            else
                            {
                                Face face = new Face(mesh);

                                face.Vertex1 = int.Parse(parts[1]) - 1;
                                face.Vertex2 = int.Parse(parts[2]) - 1;
                                face.Vertex3 = int.Parse(parts[3]) - 1;

                                mesh.Faces.Add(face);
                                break;
                            }
                    }
                }
                return mesh;
            } 
            catch (Exception e)
            {
                Console.WriteLine("Error reading obj file at path " + filename);
                return null;
            }
        }
    }
}
