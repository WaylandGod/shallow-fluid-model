﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Polyhedra;
using Engine.Utilities;
using Xunit;

namespace EngineTests.PolyhedraTests
{
    public class FaceTests
    {
        [Fact]
        public void Faces_GivenAClockwiseSetOfVertices_SortsVerticesInEachFaceInAnticlockwiseOrder()
        {
            // Fixture setup
            var clockwiseVertices = new List<Vertex>
            {
                VertexUtilities.NewVertex(0, 0), 
                VertexUtilities.NewVertex(Math.PI/2, Math.PI/2),
                VertexUtilities.NewVertex(Math.PI/2, 0)
            };

            // Exercise system
            var face = new Face(clockwiseVertices);
            var vertices = face.Vertices;
            var center = face.SphericalCenter();

            // Verify outcome
            var vectors = vertices.Select(vertex => vertex.Position).ToList();
            var viewDirection = -center;
            Assert.True(TestUtilities.AreInAntiClockwiseOrder(vectors, center, viewDirection));

            // Teardown
        }
    }
}
