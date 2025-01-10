using Azure.Core;
using Litwa;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        private UseCodeRequest _useCodeRequest;
        private IDiscountContext _discountContext;
        private IDiscountGenerator _generator;
        private IUseCode _useCode;

        public ProgramDiscounts(IDiscountGenerator generator, IDiscountContext context, IUseCode useCode)
        {

            _useCodeRequest = new UseCodeRequest();
            _useCode = useCode;
            _discountContext = context;
            _codeRequest = new DiscountRequest();
            _generator = generator;
        }

        public void Run()
        {
            Server server = new Server();
            TcpClient tcpClient = server.Connect();
            while (true)
            {
                NetworkStream stream = tcpClient.GetStream();
                Console.WriteLine("Listening");
                int switchCase = 0;
                Byte[] bytes = new byte[256];
                int i =0;
                int j = bytes.Length;
                string data = "";
                i = stream.Read(bytes, i, j);
                data = Encoding.UTF8.GetString(bytes, 0, i);
                j = j + i;
                if (data != null)
                {
                    if (data.Contains("Count"))
                    {
                        try
                        {
                            _codeRequest = JsonSerializer.Deserialize<DiscountRequest>(data)!;
                            if (_codeRequest.Count <= 2000 & _codeRequest.Count > 0 & _codeRequest.Length >= 7 & _codeRequest.Length <= 8)
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
                                    List<Discount> discountList = _generator.GetDiscounts(_codeRequest.Count);
                                    DiscountResponse codeResponse = new DiscountResponse(discountList, "", true);
                                    string codeResponseString = JsonSerializer.Serialize(codeResponse);
                                    byte[] discountMsg = Encoding.UTF8.GetBytes(codeResponseString);
                                    stream.Write(discountMsg, 0, discountMsg.Length);
                                }
                            }
                        }
                        catch (SocketException e)
                        {
                            Console.WriteLine("SocketException: {0}", e);
                        }
                    }
                    else if (data.Contains("Code"))
                    {
                        {
                            try
                            {
                                bool useCode;
                                _useCodeRequest = JsonSerializer.Deserialize<UseCodeRequest>(data)!;
                                if (_discountContext.Discounts
                                        .Where(x => !x.Used)
                                        .Where(y => y.Length == _codeRequest.Length)
                                        .Count() >= _codeRequest.Count)
                                {
                                    useCode = _useCode.Use(_useCodeRequest.Code);

                                }
                                else
                                {
                                    useCode = false;
                                }
                                UseCodeResponse useCodeResponse = new UseCodeResponse(useCode);
                                string useCodeResponseString = JsonSerializer.Serialize(useCodeResponse);
                                byte[] useMsg = Encoding.UTF8.GetBytes(useCodeResponseString);
                                stream.Write(useMsg, 0, useMsg.Length);
                            }
                            catch (SocketException e)
                            {
                                Console.WriteLine("SocketException: {0}", e);
                            }
                        }
                    }
                    else
                    {
                        stream.Close();

                    }
                }
            }
        }
    }
}