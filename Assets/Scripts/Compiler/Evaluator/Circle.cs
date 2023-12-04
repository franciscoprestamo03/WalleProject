namespace Compiler
{
    public class Circle : Instaciable
    {
        public string Name { get; }
        public Point Center { get; }
        public double Radius { get; }
        public Circle(string name, Point center, double radius)
        {
            Name = name;
            Type = InstanciableType.circle;
            Center = center;
            Radius = radius;
        }
    }
}