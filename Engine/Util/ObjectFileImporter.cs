﻿using Engine.Components;
using Engine.Geometry;
using System.Numerics;

namespace Engine.Util
{
    public class ObjectFileImporter
    {
        private static readonly char[] space_separator = [' '];

        public static Mesh? FromFile(string filename)
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

                                face.CalculateNormalFromVertices();
                                face.HasVertexNormals = true;

                                face.Vertex1Normal = int.Parse(v1[2]) - 1;
                                face.Vertex2Normal = int.Parse(v2[2]) - 1;
                                face.Vertex3Normal = int.Parse(v3[2]) - 1;

                                mesh.Faces.Add(face);

                                if (parts.Length == 5)
                                {
                                    string[] v4 = parts[4].Split(['/']);
                                    Face face2 = new Face(mesh)
                                    {
                                        Vertex1 = int.Parse(v1[0]) - 1,
                                        Vertex2 = int.Parse(v3[0]) - 1,
                                        Vertex3 = int.Parse(v4[0]) - 1
                                    };

                                    face2.CalculateNormalFromVertices();
                                    face2.HasVertexNormals = true;

                                    face2.Vertex1Normal = int.Parse(v1[2]) - 1;
                                    face2.Vertex2Normal = int.Parse(v3[2]) - 1;
                                    face2.Vertex3Normal = int.Parse(v4[2]) - 1;

                                    mesh.Faces.Add(face2);
                                }

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

                                if (parts.Length == 5)
                                {
                                    string[] v4 = parts[4].Split(['/']);
                                    Face face2 = new Face(mesh)
                                    {
                                        Vertex1 = int.Parse(parts[1]) - 1,
                                        Vertex2 = int.Parse(parts[3]) - 1,
                                        Vertex3 = int.Parse(parts[4]) - 1
                                    };

                                    mesh.Faces.Add(face2);
                                }

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
