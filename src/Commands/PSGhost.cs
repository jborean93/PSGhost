using System.Management.Automation;
using System.Management.Automation.Host;
using System.Reflection;

namespace PSGhost.Commands;

internal static class HostState
{
    private static MethodInfo? _setHostRefMethod;
    private static PropertyInfo? _externalHostProperty;

    public static PSHost? OriginalHost { get; set; }

    public static PSHost? GetExternalHost(PSHost host)
    {
        if (_externalHostProperty == null)
        {
            _externalHostProperty = host.GetType().GetProperty(
                "ExternalHost",
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        return (PSHost?)_externalHostProperty?.GetValue(host);
    }

    public static void SetExternalHost(PSHost originalHost, PSHost newHost)
    {
        if (_setHostRefMethod == null)
        {
            _setHostRefMethod = originalHost.GetType().GetMethod(
                "SetHostRef",
                BindingFlags.Instance | BindingFlags.NonPublic)!;
        }

        _setHostRefMethod.Invoke(originalHost, new[] { newHost });
    }
}

[Cmdlet(VerbsLifecycle.Enable, "PSGhost")]
public sealed class EnablePSGhost : PSCmdlet
{
    protected override void EndProcessing()
    {
        PSHost? externalHost = HostState.GetExternalHost(Host);
        if (externalHost == null)
        {
            ErrorRecord err = new(
                new PSInvalidOperationException("Failed to find current PSHost to override"),
                "NoExternalPSHost",
                ErrorCategory.InvalidOperation,
                null);
            WriteError(err);
            return;
        }

        HostState.OriginalHost = externalHost;
        SpectreHost newHost = new(externalHost);

        HostState.SetExternalHost(Host, newHost);
    }
}

[Cmdlet(VerbsLifecycle.Disable, "PSGhost")]
public sealed class DisablePSGhost : PSCmdlet
{
    protected override void EndProcessing()
    {
        if (HostState.OriginalHost != null)
        {
            HostState.SetExternalHost(Host, HostState.OriginalHost);
            HostState.OriginalHost = null;
        }
    }
}
