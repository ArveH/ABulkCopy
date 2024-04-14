namespace CrossRDBMS.Tests;

public class End2EndBase
{
    protected string GetName(string prefix = "")
    {
        var st = new StackTrace();
        // Frames:
        //   0: GetName
        //   1: MoveNext
        //   2: Start
        //   3: <Should be the name of the test method>
        var sf = st.GetFrame(3);
        if (sf == null)
        {
            throw new InvalidOperationException("Stack Frame is null");
        }

        var methodName = sf.GetMethod()?.Name ?? throw new InvalidOperationException("Method is null");
        var methodName20 = methodName.Length > 24 ? methodName[4..24] : methodName;

        return prefix + methodName20;
    }
}