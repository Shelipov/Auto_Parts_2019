using System;

namespace SetData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Начинаем копирование!");
            Setdata set = new Setdata();
            set.Update();
        }
    }
}
