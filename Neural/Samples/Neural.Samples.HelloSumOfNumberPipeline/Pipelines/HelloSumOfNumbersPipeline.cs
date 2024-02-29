using Neural.Core;
using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Samples.HelloSumOfNumberPipeline.Models;
using Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToInt;
using Neural.Samples.HelloSumOfNumberPipeline.Tasks.IntToString;

namespace Neural.Samples.HelloSumOfNumberPipeline.Pipelines;

public class HelloSumOfNumbersPipeline : IPipeline
{
    private Cluster<SumNumbersModel, IntsToIntTask>? _sumNumbersCluster;
    private Cluster<HelloNumberModel, IntToStringTask>? _helloNumberCluster;

    public bool Init(NeuralHub neuralHub)
    {
        _sumNumbersCluster = neuralHub.ShapeCluster<Cluster<SumNumbersModel, IntsToIntTask>>();
        _helloNumberCluster = neuralHub.ShapeCluster<Cluster<HelloNumberModel, IntToStringTask>>();

        return ClustersInitialized();
    }

    public async Task<IntToStringTask?> RunAsync(IntsToIntTask[] tasks)
    {
        if (!ClustersInitialized())
        {
            return null;
        }
        
        await _sumNumbersCluster!.RunHandleAsync(tasks, async output =>
        {
            await _helloNumberCluster!.RunAsync(IntToStringTask.FromTask(output));
        });
        
        // Since we do not collect any output, we can just return the first task
        return IntToStringTask.FromTask(tasks.FirstOrDefault());
    }

    private bool ClustersInitialized()
    {
        return _sumNumbersCluster is not null && _helloNumberCluster is not null;
    }
}