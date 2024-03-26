using Neural.Samples.SumOfNumbersCluster.Tasks.IntToInt;

namespace Neural.Samples.SumOfNumbersCluster.Common;

public static class Utils
{
    public static IntsToIntTask[] GenerateIntsToIntTasks(int i)
    {
        var numbersInput = Enumerable.Repeat(Enumerable.Range(0, i), i);
        var intsToIntTasks = numbersInput.Select(x => new IntsToIntTask(x.ToArray())).ToArray();
        
        return intsToIntTasks;
    }
}