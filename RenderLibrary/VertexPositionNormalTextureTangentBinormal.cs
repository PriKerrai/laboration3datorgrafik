using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laboration3Datorgrafik
{
    public struct VertexPositionNormalTextureTangentBinormal : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public Vector3 Tangent;
        public Vector3 Binormal;
        public static readonly VertexElement[] VertexElements = new VertexElement[] {
                new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
                new VertexElement(sizeof(float) * 11, VertexElementFormat.Vector3, VertexElementUsage.Binormal, 0),
            };

        public VertexPositionNormalTextureTangentBinormal(Vector3 position, Vector3 normal, Vector2 textureCoordinate, Vector3 tangent, Vector3 binormal)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
            Tangent = tangent;
            Binormal = binormal;
        }
        public static int SizeInBytes { get { return sizeof(float) * 14; } }

        public VertexDeclaration VertexDeclaration
        {
            get { return new VertexDeclaration(VertexElements); }
        }
    }
}
