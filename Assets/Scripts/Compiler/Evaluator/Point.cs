namespace Compiler
{
    public class Point: Instaciable
    {
        public string Name { get; }
        public int X { get; }
        public int Y { get; }
        public Point(string name,int x, int y)
        {
            Name = name;
            Type = InstanciableType.point;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Point x:{X} y:{Y}";
        }
    }
}