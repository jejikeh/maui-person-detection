using Neural.Core.Models;
using Neural.Samples.SumOfNumbersCluster.Common.Dependencies;
using Neural.Samples.SumOfNumbersCluster.Tasks.IntToInt;
using NullReferenceException = System.NullReferenceException;

namespace Neural.Samples.SumOfNumbersCluster.Models;

public class SumNumbersModel : IModel<IntsToIntTask, SumNumbersDependencies>
{
    public string Name { get; set; }
    public SumNumbersDependencies? DependencyContainer { get; set; }
    public event EventHandler<ModelStatusChangedEventArgs>? StatusChanged;
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
    
    void IModel<IntsToIntTask, SumNumbersDependencies>.Initialize(SumNumbersDependencies dependencyContainer)
    {
        DependencyContainer = dependencyContainer;
        Name = dependencyContainer.ModelNameProvider.GetModelName();
    }
}