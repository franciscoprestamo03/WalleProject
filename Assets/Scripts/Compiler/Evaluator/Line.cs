namespace Compiler
{
    public class Line : Instaciable
    {
        public string Name { get; }
        public Point X { get; }
        public Point Y { get; }
        public Line(string name,Point x, Point y)
        {
            Name = name;
            Type = InstanciableType.line;
            X = x;
            Y = y;
        }
    }
}