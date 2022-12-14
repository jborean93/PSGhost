using System.Management.Automation;
using System.Management.Automation.Host;
using System.Collections.ObjectModel;
using System.Reflection;

namespace PSGhost.Commands;

public static class HostState
{
    internal const string PromptFuncName = "function:prompt";

    private static MethodInfo? _setHostRefMethod;
    private static PropertyInfo? _externalHostProperty;
    internal static ScriptBlock? _originalPromptFunc = null;

    internal static ScriptBlock PromptFunc = ScriptBlock.Create("[PSGhost.Commands.HostState]::Prompt()");

    internal static PSHost? OriginalHost { get; set; }
    internal static SpectreHost? NewHost { get; set; }

    internal static PSHost? GetExternalHost(PSHost host)
    {
        if (_externalHostProperty == null)
        {
            _externalHostProperty = host.GetType().GetProperty(
                "ExternalHost",
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        return (PSHost?)_externalHostProperty?.GetValue(host);
    }

    internal static void SetExternalHost(PSHost originalHost, PSHost newHost)
    {
        if (_setHostRefMethod == null)
        {
            _setHostRefMethod = originalHost.GetType().GetMethod(
                "SetHostRef",
                BindingFlags.Instance | BindingFlags.NonPublic)!;
        }

        _setHostRefMethod.Invoke(originalHost, new[] { newHost });
    }

    public static Collection<PSObject> Prompt()
    {
        // Hack to cancel the progress display when pwsh clears the repl
        NewHost?._userInterface?.CancelProgressOutput();

        PowerShell ps = PowerShell.Create(RunspaceMode.CurrentRunspace);
        ps.AddScript(_originalPromptFunc?.ToString() ?? "");
        return ps.Invoke();
    }
}

[Cmdlet(VerbsLifecycle.Enable, "PSGhost")]
public sealed class EnablePSGhost : PSCmdlet
{
    protected override void EndProcessing()
    {
        if (HostState.NewHost != null)
        {
            return;
        }

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

        SpectreHost newHost = new(externalHost);

        HostState.NewHost = newHost;
        HostState.OriginalHost = externalHost;

        PSObject promptFunc = (PSObject)InvokeProvider.Item.Get(HostState.PromptFuncName)[0];
        HostState._originalPromptFunc = ((FunctionInfo)promptFunc.BaseObject).ScriptBlock;
        InvokeProvider.Item.Set(HostState.PromptFuncName, HostState.PromptFunc);

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
            InvokeProvider.Item.Set(HostState.PromptFuncName, HostState._originalPromptFunc);
            HostState._originalPromptFunc = null;

            HostState.SetExternalHost(Host, HostState.OriginalHost);
            HostState.OriginalHost = null;
        }
    }
}
