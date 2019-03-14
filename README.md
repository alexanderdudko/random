# random

Random entropy files generation, .NET implementation.
Generated files info is stored in FilesInfo.txt

### Options
Application can perform one of the following tasks:
- `GenerateEntropyFiles`: Generate given number of files with varying entropy per byte. Accepts arguments:
  - `path` - path for the output files (default is current working directory)
  - `fileSize` - total size in bytes of each file (default is 1MB)
  - `numberOfFiles` - number of files to be created (default is 200)
  - `prefix` - prefix for every file (default is "exp_")
- `GenerateMarkovEntropyFiles`: Generate given files with given entropy values and with given dependency from previous N bytes
  - `path` - path for the output files (default is current working directory)
  - `initSize` - initialization sequence size in bytes which has no dependency between bytes (default is 32)
  - `totalSize` - total size in bytes of each file (default is 1MB)
  - `dependanceDepth` - number of different dependance depth options (default is 6)
  - `entropyValues` - number of different entropy values (default is 33)
  - `prefix` - prefix for every file (default is "exp_")

### Usage
```
dotnet GeneratorCLI.dll task=<task_name> [<argument>=<value>, ...]
```

Example:
```
dotnet GeneratorCLI.dll task=GenerateEntropyFiles fileSize=1024 numberOfFiles=5 prefix=exp1_
```
