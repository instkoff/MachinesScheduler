namespace MachinesScheduler.WPF.Shapes
{
    /// <summary>
    /// Вспомогательный класс для отрисовки текстовых данных
    /// </summary>
    public class TextDetails
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Text { get; set; }

        public TextDetails(double x, double y, double width, double height, string text)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Text = text;
        }
    }
}