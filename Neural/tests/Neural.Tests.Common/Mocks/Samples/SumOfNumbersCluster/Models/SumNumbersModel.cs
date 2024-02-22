using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Common.Dependencies;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Tasks.IntToInt;
using NullReferenceException = System.NullReferenceException;

namespace Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Models;

public class SumNumbersModel : IModel<IntsToIntTask, SumNumbersDependencies>
{
    public string Name { get; set; } = Guid.NewGuid().ToString();
    public SumNumbersDependencies? DependencyContainer { get; set; }

    private ModelStatus _status = ModelStatus.Inactive;

    public ModelStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            StatusChanged?.Invoke(this, new ModelStatusChangedEventArgs(value));
        }
    }

    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;

    public async Task<IntsToIntTask> RunAsync(IntsToIntTask task)
    {
        if (DependencyContainer is null)
        {
            throw new NullReferenceException(nameof(DependencyContainer));
        }
        
        Status = ModelStatus.Active;
        
        var sum = await DependencyContainer.SumNumbersService.SumAsync(task.IntsInput().Value);
        
        task.SetOutput(this, sum);
        
        Status = ModelStatus.Inactive;
        
        return task;
    }

    public IntsToIntTask TryRunInBackground(IntsToIntTask input)
    {
        Status = ModelStatus.Active;

        Task.Run(async () => await RunAsync(input));

        return input;
    }
}