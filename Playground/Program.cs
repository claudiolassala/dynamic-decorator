using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Playground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Sample1();
            //Sample2();
            //Sample3();
            //Sample4();
            //Sample5();
            //Sample6();
            //Sample7();
        }

        private static void Sample1()
        {
            Console.WriteLine("Sample 1");
            var names = new[] {"Claudio", "Paul"};
            var result = names.Select(n => new {n.Length}).ToList();
            Console.WriteLine(result.GetType().Name);
        }

        private static void Sample2()
        {
            Console.WriteLine("Sample 2");

            var names = new[] {"Claudio", "Paul"};
            dynamic result = names.Select(n => new {n.Length}).ToList();

            Console.WriteLine(result.GetType().Name);

            result = "Woohoo";

            Console.WriteLine(result.GetType().Name);
        }

        private static void Sample3()
        {
            var user = new User();
            var hello = user.SayHello();

            Console.WriteLine(hello);
        }

        private static void Sample4()
        {
            var instanceType = Type.GetType("Playground.User");
            var instance = Activator.CreateInstance(instanceType);

            var sayHelloMethod = instanceType.GetMethods().SingleOrDefault(m => m.Name == "SayHello");

            var hello = sayHelloMethod.Invoke(instance, null);

            Console.WriteLine(hello.ToString());
        }

        private static void Sample5()
        {
            dynamic user = new ExpandoObject();
            user.FirstName = "Claudio";
            user.LastName = "Lassala";

            user.FullName = new Func<string>(() => $"{user.FirstName} {user.LastName}");

            Console.WriteLine(user.FullName());
        }

        private static void Sample6()
        {
            dynamic user = new ExpandoObject();
            user.FullName = new Func<string>(() => $"{user.FirstName} {user.LastName}");

            user.FirstName = "Claudio";
            user.LastName = "Lassala";

            Console.WriteLine(user.FullName());
        }


        private static void Sample7()
        {
            var instanceType = Type.GetType("Playground.User");
            dynamic instance = Activator.CreateInstance(instanceType);

            object hello = instance.SayHello();

            Console.WriteLine(hello.ToString());
        }

        private static void Sample8()
        {
            var instance = MakeObject("Playground.User");
            var result = instance.Call("SayHello");

            Console.WriteLine(result);
        }

        private static dynamic MakeObject(string className)
        {
            var instanceType = Type.GetType(className);
            dynamic instance = Activator.CreateInstance(instanceType);

            dynamic expando = new ExpandoObject();
            expando.Call = new Func<string, dynamic>(m =>
            {
                IEnumerable<dynamic> methods = instance.GetType().GetMethods();
                var method = methods.SingleOrDefault(x => x.Name == m);
                return method.Invoke(instance, null);
            });

            return expando;
        }
    }

    public class User
    {
        public string SayHello()
        {
            return "Hello!!!";
        }
    }
}