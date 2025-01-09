using Litwa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Litwa
{
    public class ProgramDiscounts
    {
        bool flag = true;
        private DiscountRequest _codeRequest;
        private IDiscountContext _discountContext;
        private IDiscountGenerator _generator;

        public ProgramDiscounts(IDiscountGenerator generator, IDiscountContext context)
        {
            _codeRequest = new DiscountRequest();
            _discountContext = context;
            _generator = generator;
        }

        public void Run()
        {
            Server server = new Server();
            TcpClient tcpClient = server.Connect();
            while (flag)
            {
                NetworkStream stream = tcpClient.GetStream();
                Console.WriteLine("Listening");
                Byte[] bytes = new byte[256];
                int i;
                string data ="";
                i = stream.Read(bytes, 0, bytes.Length);
                data = Encoding.UTF8.GetString(bytes, 0, i);
                if (data != null & !data.Any(x => data.Contains('\0')))
                {
                    _codeRequest = JsonSerializer.Deserialize<DiscountRequest>(data)!;
                    if (_codeRequest.Count <= 2000 & _codeRequest.Count > 0)
                    {
                        while (_discountContext.Discounts
                            .Where(x => x.Used == false)
                            .Where(y => y.Length == _codeRequest.Length)
                            .Count() < _codeRequest.Count)
                        {
                            _generator.CreateDiscounts(_codeRequest.Length);
                        }
                        if (_discountContext.Discounts
                            .Where(x => x.Used == false)
                            .Where(y => y.Length == _codeRequest.Length)
                            .Count() >= _codeRequest.Count)
                        {
                            IQueryable<Discount> discountQuerry = _generator.GetDiscounts(false, _codeRequest.Count);
                            DiscountResponse codeResponse = new DiscountResponse(discountQuerry.ToList(), "", true);
                            string codeResponseString = JsonSerializer.Serialize(codeResponse);
                            byte[] msg = Encoding.UTF8.GetBytes(codeResponseString);
                            stream.Write(msg, 0, msg.Length);
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
}