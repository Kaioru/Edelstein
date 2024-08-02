using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

internal record PipeStep<TMessage>(
    int Priority,
    IPipe<TMessage> Pipe
);
