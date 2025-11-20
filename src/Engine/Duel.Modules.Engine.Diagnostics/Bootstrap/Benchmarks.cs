using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var location = typeof(Program).Assembly.Location;

var directory = Path.GetDirectoryName(location)!;

var root = Path.GetFullPath(Path.Combine(directory, "..", "..", "..", "..", ".."));

var artifactsPath = Path.Combine(root, "build", "BenchmarkDotNet.Artifacts");

var config = DefaultConfig.Instance.WithArtifactsPath(artifactsPath);

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);