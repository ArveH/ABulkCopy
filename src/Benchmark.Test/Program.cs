﻿using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace Benchmark.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = DefaultConfig.Instance
                .AddJob(Job
                    .MediumRun
                    .WithLaunchCount(1)
                    .WithToolchain(InProcessNoEmitToolchain.Instance));

            var summary = BenchmarkRunner.Run<ParseConvertFunction>(config);
        }
    }
}