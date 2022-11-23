using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Lambdas are just short handed anonymous method or functions usally

        //for use in passing to other methods or functions to call such as a sort.
        ////Example:
        /// //The Lambda used here after the =s
        Action ExampleContainerForDelagate = ()=>
        {/*Do something such as this example*/
            Console.WriteLine("Hello world");
        };

        //has been shorted from this anonymous delegate or method.
         Action ExampleContainerForDelagate1 = delegate()
         {
             Console.WriteLine("Hello world");
         };
         // And this full (named) method signature method passing
         Action ExampleContainerForDelagate2 = ExampleMethod;
         void ExampleMethod()
         { Console.WriteLine("Hello world"); }
         //usage example
        List<int> ExampleList = new List<int>() {1, 3, 0};
        ExampleList.Sort((x, y) => { return y - x; });
        //x代表前面的值，y代表後一位的值(x為第二個值的時候，y就是第三個值)
        //return的值大於0的時候就要互換位置(1-3<0不換，1-0>0，所以1跟0換位置)
        //所以x-y代表小排到大，因為若前一項大於後一項就會換位置
        //同理y-x代表大排到小
        foreach (var i in ExampleList)
        {
            print(i); //0,1,3
        }

        // Make a new lambda function
        Func<string, string> greet = (name) => $"Hello, {name}!";
        Func<string, string> greet1 = delegate(string name)
        {
            return (string.Format("Hello, {0}!", name));
        };

        // Call it
        Console.WriteLine(greet("John"));

        //------------------------------------------------------------------------
        string[] words = { "bot", "apple", "apricot" };
        int minimalLength = words.Where(w => w.StartsWith("a"))
            .Min(w => w.Length);
        Console.WriteLine(minimalLength);   // output: 5

        int[] numbers = { 4, 7, 10 };
        int product = numbers.Aggregate((interim, next) => interim * next);
        print(product);   // output: 280
        int product1 = numbers.Aggregate(2, (interim, next) => interim * next);
        print(product1);   // output:

    }
}