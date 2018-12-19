using Xunit;
using Xunit.Sdk;

namespace UC.WebApi.Tests.API.Attributes
{
    [XunitTestCaseDiscoverer("DynamicSkipExample.XunitExtensions.SkippableTheoryDiscoverer", "DynamicSkipExample")]
    public class SkippableTheoryAttribute : TheoryAttribute {}
}
