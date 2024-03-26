using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Defaults.Models;
using Neural.Onnx.Common.Dependencies;

namespace Neural.Onnx.Models;

public abstract class OnnxModel<TModelTask> : Model<TModelTask, OnnxDependencies> 
    where TModelTask : IModelTask
{
    protected InferenceSession? InferenceSession => 
        DependencyContainer?.InferenceSession;
}