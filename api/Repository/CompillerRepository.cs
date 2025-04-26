using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using System.Diagnostics;

namespace api.Repository
{
    public class CompillerRepository : ICompillerRepository
    {
        public async Task<CompileResult> CompileAsync(CompileRequest request)
        {
            var tempFolder = Path.Combine("Temp", Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);

            var filePath = Path.Combine(tempFolder, request.FileName);
            await System.IO.File.WriteAllTextAsync(filePath, request.Code);

            var dockerfilePath = Path.Combine(tempFolder, "Dockerfile");

            var dockerfileContent = @$"
                FROM openjdk:17-slim
                WORKDIR /app
                COPY . /app
                RUN javac {request.FileName}
                CMD [""java"", ""{Path.GetFileNameWithoutExtension(request.FileName)}""]";

            await System.IO.File.WriteAllTextAsync(dockerfilePath, dockerfileContent);

            var imageTag = $"temp-{Path.GetFileNameWithoutExtension(tempFolder).ToLower()}";

            var buildResult = await RunProcessAsync("docker", $"build -t {imageTag} \"{tempFolder}\"");

            if (buildResult.ExitCode != 0)
            {
                Cleanup(tempFolder);
                return new CompileResult
                {
                    Success = false,
                    Error = buildResult.Error
                };
            }

            var runResult = await RunProcessAsync("docker", $"run --rm {imageTag}");

            Cleanup(tempFolder);

            return new CompileResult
            {
                Success = runResult.ExitCode == 0,
                Output = runResult.Output,
                Error = runResult.ExitCode != 0 ? runResult.Error : null
            };
        }

        private async Task<(int ExitCode, string Output, string Error)> RunProcessAsync(string fileName, string arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            return (process.ExitCode, output, error);
        }

        private void Cleanup(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }
    }
}