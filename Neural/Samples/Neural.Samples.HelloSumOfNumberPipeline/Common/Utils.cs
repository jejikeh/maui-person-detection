using Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToInt;

namespace Neural.Samples.HelloSumOfNumberPipeline.Common;

public static class Utils
{
    public static IntsToIntTask[] GenerateIntsToIntTasks(int i)
    {
        var numbersInput = Enumerable.Repeat(Enumerable.Range(0, i), i);
        var intsToIntTasks = numbersInput.Select(x => new IntsToIntTask(x.ToArray())).ToArray();
        
        return intsToIntTasks;
    }
}