using Benchmarks;

var results = new List<BenchmarkDotNet.Reports.Summary>
{
    //BenchmarkDotNet.Running.BenchmarkRunner.Run<WriteBenchmarks>(),
    //BenchmarkDotNet.Running.BenchmarkRunner.Run<ReadBenchmarks>(),
    BenchmarkDotNet.Running.BenchmarkRunner.Run<EnumerateStructBenchmarks>()
};

// foreach (var summary in results)
// {
//     foreach (var report in summary.Reports)
//     {
//         var r = report.GetResultRuns();
//         
//     }
//     Console.WriteLine(summary.Reports);
// }

return;

//PrintStructInfo<ConstructorStructDefaultLayout>();
//PrintStructInfo<ConstructorStructP0>();
//PrintStructInfo<ConstructorStructP1>();
// I don't know how to get the size of a struct with StructLayout(LayoutKind.Auto), so we will skip it for now
// PrintStructInfo<ConstructorStructAuto>();

return;