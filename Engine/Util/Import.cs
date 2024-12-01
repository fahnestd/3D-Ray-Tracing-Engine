using Engine.Components;
using System.Numerics;

namespace Engine.Util
{
    public class Import
    {
        private static readonly char[] space_separator = [' '];

        public static Mesh? FromObjectFile(string filename)
        {
            try
            {
                Mesh mesh = new Mesh
                {
                    FaceMode = Mesh.Mode.LOAD
                };
                IEnumerable<string> file = File.ReadLines(filename);

                foreach (string item in file)
                {
                    string[] parts = item.Split(space_separator, StringSplitOptions.RemoveEmptyEntries);

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
                            mesh.Normals.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                            break;
                        case "f":
                            // Check if were dealing with normals or not.
                            if (parts[1].Contains('/'))
                            {
                                string[] v1 = parts[1].Split(['/']);
                                string[] v2 = parts[2].Split(['/']);
                                string[] v3 = parts[3].Split(['/']);

                                Face face = new Face(mesh)
                                {
                                    Vertex1 = int.Parse(v1[0]) - 1,
                                    Vertex2 = int.Parse(v2[0]) - 1,
                                    Vertex3 = int.Parse(v3[0]) - 1
                                };

                                face.SetFaceNormal(
                                    mesh.Normals[int.Parse(v1[2]) - 1],
                                    mesh.Normals[int.Parse(v2[2]) - 1],
                                    mesh.Normals[int.Parse(v3[2]) - 1]
                                );

                                mesh.Faces.Add(face);
                            }
                            else
                            {
                                Face face = new Face(mesh)
                                {
                                    Vertex1 = int.Parse(parts[1]) - 1,
                                    Vertex2 = int.Parse(parts[2]) - 1,
                                    Vertex3 = int.Parse(parts[3]) - 1
                                };

                                mesh.Faces.Add(face);

                            }
                            break;
                    }
                }
                return mesh;
            }
            catch (Exception)
            {
                Console.WriteLine("Error reading obj file at path " + filename);
                return null;
            }
        }
    }
}
