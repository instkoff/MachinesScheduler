using System;
using MachinesScheduler.BL;

namespace MachinesScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var data = new Data();
            var list = data.LoadMachines();
            Console.WriteLine(string.Join("\n", list));
            Console.ReadLine();
        }
    }
}
