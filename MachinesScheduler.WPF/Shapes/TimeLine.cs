﻿namespace MachinesScheduler.WPF.Shapes
{
    /// <summary>
    /// Вспомогательный класс для отрисовки линии времени
    /// </summary>
    public class TimeLine
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public TimeLine(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }
    }
}
