using System;

namespace RpcClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var rpcClient = new RpcClient();

            var seed = new Random().Next(1, 30);
            Console.WriteLine($" [x] Requesting fib({seed})");
            var response = rpcClient.Call(seed.ToString());

            Console.WriteLine(" [.] Got '{0}'", response);
            rpcClient.Close();

            Console.ReadKey();
        }
    }
}
