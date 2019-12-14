using System;
using System.Collections.Generic;
using MathSupport;
using OpenTK;
using Utilities;

namespace Scene3D
{
    public class Construction
    {
        #region Form initialization

        /// <summary>
        /// Optional form-data initialization.
        /// </summary>
        /// <param name="name">Return your full name.</param>
        /// <param name="param">Optional text to initialize the form's text-field.</param>
        /// <param name="tooltip">Optional tooltip = param help.</param>
        public static void InitParams(out string name, out string param, out string tooltip)
        {
            name = "David Napravnik";
            param = "s=50, wid=1, depth=.2";
            tooltip = "[s=<splits>,] [wid=<line width>,] [depth=<object thickness>,]";
        }

        #endregion

        #region Instance data

        // !!! If you need any instance data, put them here..

        private int splits = 50;
        private float lineWidth = 1.0f;
        private float depth = .2f;

        private void parseParams(string param)
        {
            // Defaults.
            splits = 50;
            lineWidth = 1.0f;
            depth = .2f;

            Dictionary<string, string> p = Util.ParseKeyValueList(param);
            if (p.Count > 0)
            {
                // s=<int>
                Util.TryParse(p, "s", ref splits);

                // wid=<float>
                Util.TryParse(p, "wid", ref lineWidth);
                // depth=<float>
                Util.TryParse(p, "depth", ref depth);
            }
        }

        #endregion

        public Construction()
        {
            // {{

            // }}
        }

        #region Mesh construction

        /// <summary>
        /// Construct a new Brep solid (preferebaly closed = regular one).
        /// </summary>
        /// <param name="scene">B-rep scene to be modified</param>
        /// <param name="m">Transform matrix (object-space to world-space)</param>
        /// <param name="param">Shape parameters if needed</param>
        /// <returns>Number of generated faces (0 in case of failure)</returns>
        public int AddMesh(SceneBrep scene, Matrix4 m, string param)
        {
            parseParams(param);
            scene.LineWidth = lineWidth;            

            // create shape
            // create vertices in 3D local space
            Vector3[] shapeVerticesLocal = new Vector3[]
            {
                new Vector3(0,1,depth),
                new Vector3(0.866025f, 0.5f, depth),
                new Vector3(0.866025f, -0.5f, depth),
                new Vector3(0, -1, depth),
                new Vector3(-0.866025f, -0.5f, depth),
                new Vector3(-0.866025f, 0.5f, depth),
                new Vector3(0,0,depth),

                new Vector3(0,1,-depth),
                new Vector3(0.866025f, 0.5f, -depth),
                new Vector3(0.866025f, -0.5f, -depth),
                new Vector3(0, -1, -depth),
                new Vector3(-0.866025f, -0.5f, -depth),
                new Vector3(-0.866025f, 0.5f, -depth),
                new Vector3(0,0,-depth),
            };

            // push vertices to scene and get theirs indexes
            int[] shapeVerticesIndex = new int[shapeVerticesLocal.Length];
            for (int i = 0; i < shapeVerticesLocal.Length; i++)
            {
                int vertexIndex = scene.AddVertex(Vector3.TransformPosition(shapeVerticesLocal[i], m));
                shapeVerticesIndex[i] = vertexIndex;
                scene.SetColor(vertexIndex, new Vector3(0,0,0));
            }

            Vector3[] edgeColors = new Vector3[]{ new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 0) };

            // { vertex, vertex, color }
            List<int[,]> shapeEdges = new List<int[,]>();
            shapeEdges.Add(new int[2, 3] { { 0, 1, 0 }, {  8,  9, 0 } });
            shapeEdges.Add(new int[2, 3] { { 1, 2, 1 }, {  9, 13, 1 } });
            shapeEdges.Add(new int[2, 3] { { 2, 6, 1 }, {  10, 9, 1 } });
            shapeEdges.Add(new int[2, 3] { { 2, 3, 0 }, { 10, 11, 0 } });
            shapeEdges.Add(new int[2, 3] { { 3, 4, 1 }, { 11, 13, 1 } });
            shapeEdges.Add(new int[2, 3] { { 4, 6, 1 }, { 12, 11, 1 } });
            shapeEdges.Add(new int[2, 3] { { 4, 5, 0 }, { 12,  7, 0 } });
            shapeEdges.Add(new int[2, 3] { { 5, 0, 1 }, {  7, 13, 1 } });
            shapeEdges.Add(new int[2, 3] { { 0, 6, 1 }, {  8,  7, 1 } });

            shapeEdges.Add(new int[2, 3] { { 6, 0, 2 }, { 9, 13, 2 } });
            shapeEdges.Add(new int[2, 3] { { 6, 2, 2 }, { 11, 13, 2 } });
            shapeEdges.Add(new int[2, 3] { { 6, 4, 2 }, { 7, 13, 2 } });

            shapeEdges.Add(new int[2, 3] { { 0, 1, 3 }, {  7,  8, 3 } });
            shapeEdges.Add(new int[2, 3] { { 1, 2, 3 }, {  8,  9, 3 } });
            shapeEdges.Add(new int[2, 3] { { 2, 3, 3 }, {  9, 10, 3 } });
            shapeEdges.Add(new int[2, 3] { { 3, 4, 3 }, { 10, 11, 3 } });
            shapeEdges.Add(new int[2, 3] { { 4, 5, 3 }, { 11, 12, 3 } });
            shapeEdges.Add(new int[2, 3] { { 5, 0, 3 }, { 12, 7, 3 } });



            // push edges to scene
            foreach (int[,] group in shapeEdges)
            {
                scene.AddLine(shapeVerticesIndex[group[0, 0]], shapeVerticesIndex[group[0, 1]]);
                scene.AddLine(shapeVerticesIndex[group[1, 0]], shapeVerticesIndex[group[1, 1]]);
            }


            //push faces to scene
            foreach (int[,] group in shapeEdges)
            {
                for (int i = 0; i < splits; i++)
                {
                    Vector3 beginDiff = shapeVerticesLocal[group[0, 0]] - shapeVerticesLocal[group[0, 1]];
                    beginDiff /= splits;
                    Vector3 begin = shapeVerticesLocal[group[0, 0]] - beginDiff*i;

                    Vector3 endDiff = shapeVerticesLocal[group[1, 0]] - shapeVerticesLocal[group[1, 1]];
                    endDiff /= splits;
                    Vector3 end = shapeVerticesLocal[group[1, 0]] - endDiff*i;


                    int beginIndex = scene.AddVertex(Vector3.TransformPosition(begin, m));
                    scene.SetColor(beginIndex, edgeColors[group[0, 2]]);

                    int endIndex = scene.AddVertex(Vector3.TransformPosition(end, m));
                    scene.SetColor(endIndex, edgeColors[group[1, 2]]);

                    scene.AddLine(beginIndex, endIndex);
                }

            }
            return 1; // some magic number
        }
        #endregion
    }
}
