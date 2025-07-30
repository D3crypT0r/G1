namespace SnakeGame
{
    public static class Settings
    {
        public static int Width { get; } = 20;
        public static int Height { get; } = 20;
        public static int Speed { get; } = 12;
        public static int Points { get; } = 10;
        public static Color HeadColor { get; } = Color.FromArgb(0, 120, 215);
        public static Color BodyColor { get; } = Color.FromArgb(0, 174, 219);
        public static Color FoodColor { get; } = Color.FromArgb(220, 20, 60);
    }
}
