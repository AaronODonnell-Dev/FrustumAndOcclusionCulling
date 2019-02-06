using Microsoft.Xna.Framework;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frustum_and_Occlusion_Culling
{
    class OctTree
    {
        public float Size { get; set; }
        public Vector3 Position { get; set; }
        public int MaxObjects { get; set; }
        public BoundingBox Bounds { get; set; }

        public List<GameObject3D> Objects { get; set; }
        public List<OctTree> Nodes { get; set; }


        public OctTree(float size, Vector3 position, int maxObjetcs)
        {
            Size = size;
            Position = position;
            MaxObjects = maxObjetcs;

            Objects = new List<GameObject3D>();
            Nodes = new List<OctTree>();

            //TODO: Create Bounds of given size at given position
            float halfSize = size / 2;
            Vector3 min = new Vector3(position.X - halfSize, position.Y - halfSize, position.Z - halfSize);
            Vector3 max = new Vector3(position.X + halfSize, position.Y + halfSize, position.Z + halfSize);
            Bounds = new BoundingBox(min, max);



            DebugEngine.AddBoundingBox(Bounds, Color.Red, 1000);
            DebugEngine.AddBoundingSphere(new BoundingSphere(new Vector3(position.X, position.Y,position.Z), 1), Color.Black, 1000);
        }

        public void SubDivide()
        {
            OctTree TFR, TBR, TFL, TBL, BFR, BBR, BFL, BBL;
            float subSize = Size / 2;

            TFR = new OctTree(subSize, new Vector3(Position.X + subSize / 2, Position.Y + subSize / 2, Position.Z + subSize / 2), MaxObjects);
            TBR = new OctTree(subSize, new Vector3(Position.X + subSize / 2, Position.Y + subSize / 2, Position.Z - subSize / 2), MaxObjects);
            TFL = new OctTree(subSize, new Vector3(Position.X - subSize / 2, Position.Y + subSize / 2, Position.Z + subSize / 2), MaxObjects);
            TBL = new OctTree(subSize, new Vector3(Position.X - subSize / 2, Position.Y + subSize / 2, Position.Z - subSize / 2), MaxObjects);
            BFR = new OctTree(subSize, new Vector3(Position.X + subSize / 2, Position.Y - subSize / 2, Position.Z + subSize / 2), MaxObjects);
            BBR = new OctTree(subSize, new Vector3(Position.X + subSize / 2, Position.Y - subSize / 2, Position.Z - subSize / 2), MaxObjects);
            BFL = new OctTree(subSize, new Vector3(Position.X - subSize / 2, Position.Y - subSize / 2, Position.Z + subSize / 2), MaxObjects);
            BBL = new OctTree(subSize, new Vector3(Position.X - subSize / 2, Position.Y - subSize / 2, Position.Z - subSize / 2), MaxObjects);

            Nodes.Add(TFR);
            Nodes.Add(TBR);
            Nodes.Add(TFL);
            Nodes.Add(TBL);
            Nodes.Add(BFR);
            Nodes.Add(BBR);
            Nodes.Add(BFL);
            Nodes.Add(BBL);
        }

        public void AddObject(GameObject3D newObject)
        {
            if (Nodes.Count == 0)
            {
                if (Objects.Count < MaxObjects)
                {
                    Objects.Add(newObject);
                }
                else
                {
                    SubDivide();

                    foreach (GameObject3D go in Objects)
                    {
                        Distribute(go);
                    }
                    Objects.Clear();
                }
            }
            else
            {
                Distribute(newObject);
            }
        }

        public void Distribute(GameObject3D newObject)
        {
            Vector3 position = newObject.World.Translation;

            foreach (OctTree node in Nodes)
            {
                if (node.Bounds.Contains(position) != ContainmentType.Disjoint)
                {
                    node.AddObject(newObject);
                }
            }
        }

        public void Process(BoundingFrustum frustum, ref List<GameObject3D> passedObjects)
        {
            if (passedObjects == null)
            {
                passedObjects = new List<GameObject3D>();
            }

            if (frustum.Contains(Bounds) != ContainmentType.Disjoint)
            {
                passedObjects.AddRange(Objects);

                foreach (OctTree node in Nodes)
                {
                    //Process(frustum, ref passedObjects);
                    node.Process(frustum, ref passedObjects);
                }
            }
        }
    }
}
