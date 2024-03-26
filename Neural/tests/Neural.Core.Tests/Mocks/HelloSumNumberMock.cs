using Neural.Defaults.Models;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Models;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Tasks.IntToString;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;

namespace Neural.Core.Tests.Mocks;

public class HelloSumNumberMock : IPipeline
{
    public Cluster<SumNumbersModel, IntsToIntTask>? SumNumbersCluster;
    public Cluster<HelloNumberModel, IntToStringTask>? HelloNumberCluster;

    public bool Init(NeuralHub neuralHub)
    {
        SumNumbersCluster = neuralHub.ShapeCluster<Cluster<SumNumbersModel, IntsToIntTask>>();
        HelloNumberCluster = neuralHub.ShapeCluster<Cluster<HelloNumberModel, IntToStringTask>>();

        return ClustersInitialized();
    }

    public async Task<IntToStringTask?> RunAsync(IntsToIntTask task)
    {
        if (!ClustersInitialized())
        {
            return null;
        }

        var intOutput = await SumNumbersCluster!.RunAsync(task);
        var stringOutput = await HelloNumberCluster!.RunAsync(IntToStringTask.FromTask(intOutput));
        
        return stringOutput;
    }

    private bool ClustersInitialized()
    {
        return SumNumbersCluster is not null && HelloNumberCluster is not null;
    }
}