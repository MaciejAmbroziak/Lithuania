using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Http;
using Litwa;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Diagnostics.Tracing;

class MyTcpListener
{
    public static void Main(string[] args)
    {
        IDiscountContext discountContext = new DiscountContext();
        IDiscountGenerator discountGenerator = new DiscountGenerator(3000);
        Console.WriteLine(discountGenerator.GetNumberOfDiscounts());
        ProgramDiscounts program = new ProgramDiscounts(discountGenerator, discountContext);
        program.Run();
    }
}