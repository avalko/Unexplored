using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Types;

namespace Unexplored.Core.Physics
{
    public interface IQuadItem
    {
        FRect Bounds { get; }
    }

    public class QuadNode<T> where T : IQuadItem
    {
        const int MAX_OBJECTS = 10;
        const int REAL_MAX_OBJECTS = MAX_OBJECTS + 10;
        const int MAX_LEVEL = 5;

        const int NODE_TOP_LEFT = 0;
        const int NODE_TOP_RIGHT = 1;
        const int NODE_BOTTOM_LEFT = 2;
        const int NODE_BOTTOM_RIGHT = 3;

        public QuadNode<T> Parent;
        //public T[] Objects;
        public List<T> Objects;
        public FRect Bounds;
        public int Level;
        public QuadNode<T>[] Nodes;

        private bool splitted;
        private int objectsCount;
        //private T[] tempObjects;
        //private static T[] retrieveObjects;


        static QuadNode()
        {
            //retrieveObjects = new T[REAL_MAX_OBJECTS];
        }

        public QuadNode(QuadNode<T> parent, FRect bounds, int level = 0)
        {
            Level = level;
            Bounds = bounds;
            Parent = parent;
            Objects = new List<T>(REAL_MAX_OBJECTS);
            //Objects = new T[REAL_MAX_OBJECTS];
            //tempObjects = new T[REAL_MAX_OBJECTS];
            Nodes = new QuadNode<T>[4];
            splitted = false;
        }

        public void Clear()
        {
            Objects.Clear();
            objectsCount = 0;
            Nodes[NODE_TOP_LEFT]?.Clear();
            Nodes[NODE_TOP_RIGHT]?.Clear();
            Nodes[NODE_BOTTOM_LEFT]?.Clear();
            Nodes[NODE_BOTTOM_RIGHT]?.Clear();
        }

        public void Split()
        {
            float halfWidth = Bounds.Width / 2;
            float halfHeight = Bounds.Height / 2;

            Nodes[NODE_TOP_LEFT] = new QuadNode<T>(this, new FRect(Bounds.X, Bounds.Y, halfWidth, halfHeight), Level + 1);
            Nodes[NODE_TOP_RIGHT] = new QuadNode<T>(this, new FRect(Bounds.X + halfWidth, Bounds.Y, halfWidth, halfHeight), Level + 1);
            Nodes[NODE_BOTTOM_LEFT] = new QuadNode<T>(this, new FRect(Bounds.X, Bounds.Y + halfHeight, halfWidth, halfHeight), Level + 1);
            Nodes[NODE_BOTTOM_RIGHT] = new QuadNode<T>(this, new FRect(Bounds.X + halfWidth, Bounds.Y + halfHeight, halfWidth, halfHeight), Level + 1);

            splitted = true;
        }

        public int GetIndex(FRect rect)
        {
            int index = -1;
            float verticalMidpoint = Bounds.X + Bounds.Width / 2;
            float horizontalMidpoint = Bounds.Y + Bounds.Height / 2;

            // Object can completely fit within the top quadrants
            bool topQuadrant = (rect.Y < horizontalMidpoint && rect.Y + rect.Height < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            bool bottomQuadrant = (rect.Y > horizontalMidpoint);

            // Object can completely fit within the left quadrants
            if (rect.X < verticalMidpoint && rect.X + rect.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = NODE_TOP_LEFT;
                }
                else if (bottomQuadrant)
                {
                    index = NODE_BOTTOM_LEFT;
                }
            }
            // Object can completely fit within the right quadrants
            else if (rect.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = NODE_TOP_RIGHT;
                }
                else if (bottomQuadrant)
                {
                    index = NODE_BOTTOM_RIGHT;
                }
            }

            return index;
        }

        public void Insert(T item)
        {
            if (splitted)
            {
                int index = GetIndex(item.Bounds);
                if (index != -1)
                {
                    Nodes[index].Insert(item);
                    return;
                }
            }

            //Objects[objectsCount++] = item;
            Objects.Add(item);
            objectsCount = Objects.Count;

            if (objectsCount > MAX_OBJECTS && Level < MAX_LEVEL)
            {
                if (!splitted)
                    Split();

                int i = 0;
                int index = 0;
                while (i < objectsCount)
                {
                    //T obj = Objects[objectsCount - 1 - i];
                    T obj = Objects[i];
                    index = GetIndex(obj.Bounds);
                    if (index != -1)
                    {
                        Nodes[index].Insert(obj);
                        Objects.Remove(obj);
                        --objectsCount;
                    }
                    else
                        //tempObjects[i++] = obj;
                        ++i;
                }

                //while (--i >= 0)
                //    Objects[i] = tempObjects[i];
            }
        }

        public void Retrieve(List<T> returnObjects, FRect bounds)
        {
            int index = GetIndex(bounds);
            if (index != -1 && splitted)
            {
                Nodes[index].Retrieve(returnObjects, bounds);
            }

            returnObjects.AddRange(Objects);
        }
    }

    public class QuadTree<T> where T : IQuadItem
    {
        
    }
}
