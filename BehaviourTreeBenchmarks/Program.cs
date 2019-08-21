using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using AYLib.BehaviourTree;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BehaviourTreeBenchmarks
{
    [MemoryDiagnoser]
    // It is very easy to use BenchmarkDotNet. You should just create a class
    public class IntroBasic
    {
        // And define a method with the Benchmark attribute
        [Benchmark]
        public void DoWork1()
        {
            BehaviourTreeBuilder builder = new BehaviourTreeBuilder("Main");
            var retVal = builder
                    .Sequence("Node 1")
                        .Selector("Node 2")
                            .Action("Do 2", (x, y) => BehaviourReturnCode.Failure)
                            .Action("Do 2", (x, y) => BehaviourReturnCode.Failure)
                        .End()
                        .Action("Do 1", (x, y) => BehaviourReturnCode.Success)
                        .Action("Do 1", (x, y) => BehaviourReturnCode.Success)
                    .End()
                .End()
                .Build();

            List<IBehaviourTreeState> nodes = new List<IBehaviourTreeState>();
            for (int i = 0; i < 100000; i++)
            {
                nodes.Add(retVal.GetState());
            }
                
        }

        [Benchmark]
        public void DoWork2()
        {
            List<IBehaviourTreeNode> nodes = new List<IBehaviourTreeNode>();

            for (int i = 0; i < 100000; i++)
            {
                BehaviourTreeBuilder builder = new BehaviourTreeBuilder("Main");
                var retVal = builder
                        .Sequence("Node 1")
                            .Selector("Node 2")
                                .Action("Do 2", (x, y) => BehaviourReturnCode.Failure)
                                .Action("Do 2", (x, y) => BehaviourReturnCode.Failure)
                            .End()
                            .Action("Do 1", (x, y) => BehaviourReturnCode.Success)
                            .Action("Do 1", (x, y) => BehaviourReturnCode.Success)
                        .End()
                    .End()
                    .Build();

                nodes.Add(retVal);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<IntroBasic>();
        }
    }
}
