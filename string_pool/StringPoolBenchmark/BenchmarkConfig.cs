using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Diagnosers;

namespace StringPoolBenchmark;

public sealed class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        // Where to put all artifacts (results, logs, etc.)
        ArtifactsPath = "BenchmarkResults"; // relative to the working directory

        // Add the default column providers so the summary table is populated
        AddColumnProvider(DefaultColumnProviders.Instance);

        // Add common exporters so results are easy to collect and read
        AddExporter(MarkdownExporter.Default);
        AddExporter(HtmlExporter.Default);
        AddExporter(CsvExporter.Default);
        AddExporter(CsvMeasurementsExporter.Default);
        // JSON exporter is version-dependent; use Brief to be safe
        AddExporter(JsonExporter.Brief);

        // Enable memory diagnoser globally so memory columns (Allocated, Gen0/1/2) appear in results
        AddDiagnoser(MemoryDiagnoser.Default);

        // Optional: console logger
        AddLogger(ConsoleLogger.Default);

        // Keep the default summary style
        SummaryStyle = SummaryStyle.Default;

        // Order for readability
        //WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
    }
}