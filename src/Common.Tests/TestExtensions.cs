namespace Common.Tests;

public class TestExtensions
{
    [Theory]
    [InlineData(0, "rows")]
    [InlineData(1, "row")]
    [InlineData(3, "rows")]
    public void TestPluralWithInt(int cnt, string expected)
    {
        var result = "row".Plural(cnt);
        result.Should().Be(expected, "because \"expected\" was expected");
    }

    [Theory]
    [InlineData(0L, "rows")]
    [InlineData(1L, "row")]
    [InlineData(3L, "rows")]
    public void TestPluralWithLong(long cnt, string expected)
    {
        var result = "row".Plural(cnt);
        result.Should().Be(expected, "because \"expected\" was expected");
    }

}