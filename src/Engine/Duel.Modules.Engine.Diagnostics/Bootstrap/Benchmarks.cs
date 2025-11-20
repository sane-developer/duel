using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var assemblyLocation = typeof(Program).Assembly.Location;

var projectDir = Path.GetDirectoryName(assemblyLocation)!;

var solutionRoot = Path.GetFullPath(Path.Combine(projectDir, "..", "..", "..", "..", ".."));

var artifactsPath = Path.Combine(solutionRoot, "build", "BenchmarkDotNet.Artifacts");

var config = DefaultConfig.Instance.WithArtifactsPath(artifactsPath);

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);