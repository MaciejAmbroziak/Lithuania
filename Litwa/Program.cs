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
        bool flag = true;
        DiscountRequest codeRequest = new DiscountRequest();
        DiscountContext discountContext = new DiscountContext();
        DiscountGenerator generator = new DiscountGenerator(3000, codeRequest);
        Server server = new Server();
        TcpClient tcpClient = server.Connect();
        while (flag)
        {
            NetworkStream stream = tcpClient.GetStream();
            Byte[] bytes = new byte[60];
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) > 0)
            {
                stream.Write(bytes, 0, i);
            }
            string data = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            data = data.Split('\n')[0];
            if (data != null & !data.Any(x => data.Contains('\0')))
            {
                codeRequest = JsonSerializer.Deserialize<DiscountRequest>(data)!;
                if (codeRequest.Count <= 2000 & codeRequest.Count > 0)
                {
                    using (discountContext)
                    {
                        while (generator.GetNumberOfDiscounts(false) < codeRequest.Count)
                        {
                            generator.CreateDiscounts(codeRequest.Length);
                        }
                        if(generator.GetNumberOfDiscounts(false) >= codeRequest.Count)
                        {                            
                            IQueryable<Discount> discountQuerry = generator.GetDiscounts(false, codeRequest.Count);
                            DiscountResponse codeResponse = new DiscountResponse(discountQuerry.ToList(), "", true);
                            string codeResponseString = JsonSerializer.Serialize(codeResponse);
                            byte[] msg = Encoding.UTF8.GetBytes(codeResponseString);
                            stream.Write(msg, 0, msg.Length);
                        }
                    }
                }
            }
            //// Buffer for reading data
            //Byte[] bytes = new Byte[256];
            //String data = null;

            //// Enter the listening loop.
            //while (true)
            //{
            //    Console.Write("Waiting for a connection... ");

            //    // Perform a blocking call to accept requests.
            //    // You could also use server.AcceptSocket() here.
            //    Console.WriteLine("Connected!");

            //    data = null;

            //    // Get a stream object for reading and writing
            //    NetworkStream stream = client.GetStream();

            //int i;



            //        byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);

            //        // Send back a response.
            //        stream.Write(msg, 0, msg.Length);
            //        Console.WriteLine("Sent: {0}", data);
            //    //    }
            //    //}
            //}
            //catch (SocketException e)
            //{
            //    Console.WriteLine("SocketException: {0}", e);
            //}
            //finally
            //{
            //    server.Stop();
            //}

            Console.WriteLine("\nHit enter to continue...");
            Console.ReadLine();
        }
    }
}