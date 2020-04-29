namespace MachinesScheduler.WPF.Shapes
{
    /// <summary>
    /// Вспомогательный класс для отрисовки партии
    /// </summary>
    public class RectItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Color { get; set; }

        public RectItem(double x, double y, double width, double height, string color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
        }
    }

}
