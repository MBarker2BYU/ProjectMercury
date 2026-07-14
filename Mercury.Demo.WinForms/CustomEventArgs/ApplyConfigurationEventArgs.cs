using Mercury.Abstractions;

namespace Mercury.Demo.WinForms.CustomEventArgs;

public sealed class ApplyConfigurationEventArgs(IMercuryClientDependencies mercuryClientDependencies) : EventArgs
{
    public IMercuryClientDependencies MercuryClientDependencies { get; } = mercuryClientDependencies;
}