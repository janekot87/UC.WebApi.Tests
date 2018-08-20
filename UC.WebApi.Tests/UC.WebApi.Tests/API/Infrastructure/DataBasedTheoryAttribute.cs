using System;
using System.IO;
using UC.WebApi.Tests.API.Logic;
using Xunit;

namespace UC.WebApi.Tests.API.Infrastructure
{
    public class DataBasedTheoryAttribute : TheoryAttribute
    {
        private readonly string _dataFolder;
        private readonly Lazy<string> _dataFile;

        public DataBasedTheoryAttribute(string dataFolder)
        {
            if (string.IsNullOrWhiteSpace(dataFolder))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dataFolder));
            }

            _dataFolder = dataFolder;
            _dataFile = new Lazy<string>(() => GetEnvironmentDataFile(dataFolder));
        }

        public string Data => _dataFile.Value;

        public override string Skip
        {
            get => Data == null
                ? $@"Data file is missing ({_dataFolder}\{TestConfiguration.Environment}.data or {_dataFolder}\Common.data)"
                : string.IsNullOrWhiteSpace(Data)
                    ? "Data file is empty"
                    : null;

            set { }
        }

        private string GetEnvironmentDataFile(string dataFolder)
        {
            if (string.IsNullOrWhiteSpace(dataFolder))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dataFolder));
            }

            string GetResourceContent(string resourceName)
            {
                using (var stream = GetType().Assembly.GetManifestResourceStream(resourceName.Replace('\\', '.')))
                {
                    if (stream != null)
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }

                    return null;
                }
            }

            var dataPathPrefix = $"CompatibilityGuide.WebApi.AutomatedTests.API.Tests.{dataFolder}";

            var environmentDataFile = $"{dataPathPrefix}.{TestConfiguration.Environment}.data";
            var commonDataFile = $"{dataPathPrefix}.Common.data";

            return GetResourceContent(environmentDataFile) ?? GetResourceContent(commonDataFile);
        }
    }
}
