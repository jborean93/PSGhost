using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSGhost;

public class SpectreHost : PSHost, IHostSupportsInteractiveSession
{
    private readonly PSHost _pwshHost;
    internal readonly SpectreHostUserInterface? _userInterface;
    private readonly IHostSupportsInteractiveSession _pwshInteractive;

    public SpectreHost(PSHost innerHost)
    {
        if (innerHost is IHostSupportsInteractiveSession interactiveHost)
        {
            _pwshInteractive = interactiveHost;
        }
        else
        {
            throw new ArgumentException("Provided host does not implement IHostSupportsInteractiveSession");
        }

        _pwshHost = innerHost;
        if (_pwshHost.UI != null)
        {
            _userInterface = new SpectreHostUserInterface(_pwshHost.UI);
        }
    }

    public override CultureInfo CurrentCulture => _pwshHost.CurrentCulture;

    public override CultureInfo CurrentUICulture => _pwshHost.CurrentUICulture;

    public override Guid InstanceId => _pwshHost.InstanceId;

    public override string Name => _pwshHost.Name;

    public override PSHostUserInterface? UI => _userInterface;

    public override Version Version => _pwshHost.Version;

    public bool IsRunspacePushed => throw new NotImplementedException();

    public Runspace Runspace => throw new NotImplementedException();

    public override void EnterNestedPrompt() => _pwshHost.EnterNestedPrompt();

    public override void ExitNestedPrompt() => _pwshHost.ExitNestedPrompt();

    public override void NotifyBeginApplication() => _pwshHost.NotifyBeginApplication();

    public override void NotifyEndApplication() => _pwshHost.NotifyEndApplication();

    public void PopRunspace() => _pwshInteractive.PopRunspace();

    public void PushRunspace(Runspace runspace) => _pwshInteractive.PushRunspace(runspace);

    public override void SetShouldExit(int exitCode) => _pwshHost.SetShouldExit(exitCode);
}


public class SpectreHostUserInterface : PSHostUserInterface, IHostUISupportsMultipleChoiceSelection
{
    private readonly IHostUISupportsMultipleChoiceSelection _choiceHost;
    private readonly PSHostUserInterface _pwshUI;
    private readonly IAnsiConsole _console;

    private SpectreProgress? _progressHandler;

    private class SpectreChoice
    {
        public int Index { get; set; }
        public ChoiceDescription Description { get; set; }

        public SpectreChoice(int idx, ChoiceDescription description)
        {
            Index = idx;
            Description = description;
        }

        public string ChoiceString()
        {
            // PowerShell uses & before a char to denote a shorthand option,
            // that isn't used for this setup.
            StringBuilder str = new(Description.Label.Replace("&", null));
            if (!string.IsNullOrWhiteSpace(Description.HelpMessage))
            {
                str.Append($" - {Description.HelpMessage}");
            }

            return str.ToString();
        }
    }

    public SpectreHostUserInterface(PSHostUserInterface ui)
    {
        if (ui is IHostUISupportsMultipleChoiceSelection choiceHost)
        {
            _choiceHost = choiceHost;
        }
        else
        {
            throw new ArgumentException("Host UI does not implement IHostUISupportsMultipleChoiceSelection");
        }

        _pwshUI = ui;

        // Probably not right but for whatever reason I need to force Ansi support here.
        _console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.Yes,
            ColorSystem = ColorSystemSupport.Detect,
            Out = new AnsiConsoleOutput(System.Console.Out),
        });
    }

    public override PSHostRawUserInterface? RawUI => _pwshUI.RawUI;

    public override Dictionary<string, PSObject> Prompt(string caption, string message,
        Collection<FieldDescription> descriptions)
        => _pwshUI.Prompt(caption, message, descriptions);

    public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices,
        int defaultChoice)
    {
        List<SpectreChoice> formattedChoices = choices.Select((c, i) => new SpectreChoice(i, c)).ToList();
        if (defaultChoice < 0)
        {
            defaultChoice = 0;
        }

        SpectreChoice defaultFormattedChoice = formattedChoices[defaultChoice];
        formattedChoices.RemoveAt(defaultChoice);

        StringBuilder title = new(caption);
        if (!string.IsNullOrWhiteSpace(message))
        {
            title.Append($" - [grey]{message}[/]");
        }

        SelectionPrompt<SpectreChoice> choicePrompt = new SelectionPrompt<SpectreChoice>()
            .Title(title.ToString())
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .UseConverter(c => c.ChoiceString());
        choicePrompt.AddChoice(defaultFormattedChoice);
        choicePrompt.AddChoices(formattedChoices.ToArray());

        SpectreChoice choice = _console.Prompt(choicePrompt);

        return choice.Index;
    }

    public Collection<int> PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices,
        IEnumerable<int> defaultChoices)
    {
        List<SpectreChoice> formattedChoices = choices.Select((c, i) => new SpectreChoice(i, c)).ToList();

        StringBuilder title = new(caption);
        if (!string.IsNullOrWhiteSpace(message))
        {
            title.Append($" - [grey]{message}[/]");
        }

        MultiSelectionPrompt<SpectreChoice> choicePrompt = new MultiSelectionPrompt<SpectreChoice>()
            .Title(title.ToString())
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .UseConverter(c => c.ChoiceString())
            .AddChoices(formattedChoices);

        foreach (int c in defaultChoices)
        {
            choicePrompt.Select(formattedChoices[c]);
        }

        List<SpectreChoice> choice = _console.Prompt(choicePrompt);

        return new Collection<int>(choice.Select(c => c.Index).ToArray());
    }

    public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
        => PromptForCredential(caption, message, userName, targetName, PSCredentialTypes.Default,
            PSCredentialUIOptions.Default);

    public override PSCredential PromptForCredential(string caption, string message, string userName,
        string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
        => _pwshUI.PromptForCredential(caption, message, userName, targetName, allowedCredentialTypes, options);

    public override string ReadLine() => _pwshUI.ReadLine();

    public override SecureString ReadLineAsSecureString() => _pwshUI.ReadLineAsSecureString();

    public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        => _pwshUI.Write(foregroundColor, backgroundColor, value);

    public override void Write(string value) => _pwshUI.Write(value);

    public override void WriteDebugLine(string message) => _pwshUI.WriteDebugLine(message);

    public override void WriteErrorLine(string value) => _pwshUI.WriteErrorLine(value);

    public override void WriteInformation(InformationRecord record) => _pwshUI.WriteInformation(record);

    public override void WriteLine(string value) => _pwshUI.WriteLine(value);

    public override void WriteProgress(long sourceId, ProgressRecord record)
    {
        if (_progressHandler == null)
        {
            _progressHandler = new(_console);
        }

        if (_progressHandler.UpdateProgress(record))
        {
            _progressHandler.Dispose();
            _progressHandler = null;
        }
    }

    public override void WriteVerboseLine(string message) => _pwshUI.WriteVerboseLine(message);

    public override void WriteWarningLine(string message) => _pwshUI.WriteWarningLine(message);

    internal void CancelProgressOutput()
    {
        _progressHandler?.Cancel();
        _progressHandler?.Dispose();
        _progressHandler = null;
    }
}

internal sealed class SpectreProgress : IDisposable
{
    private readonly ProgressContext _context;
    private readonly SemaphoreSlim _done = new(0, 1);
    private readonly Task _progressTask;
    private readonly Dictionary<Int64, ProgressTask> _runningTasks = new();

    public SpectreProgress(IAnsiConsole console)
    {
        using SemaphoreSlim contextSet = new(0, 1);
        ProgressContext? context = null;

        _progressTask = Task.Run(() => console.Progress()
            .AutoClear(true)
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn(),
                new SpinnerColumn()
            })
            .StartAsync(async ctx =>
            {
                context = ctx;
                contextSet.Release();
                await _done.WaitAsync();
            }));

        contextSet.Wait();
        _context = context!;
    }

    public bool UpdateProgress(ProgressRecord record)
    {
        StringBuilder description = new(record.Activity);
        if (!string.IsNullOrEmpty(record.StatusDescription))
        {
            description.AppendFormat(" - {0}", record.StatusDescription);
        }

        ProgressTask spectreTask;
        if (!_runningTasks.ContainsKey(record.ActivityId))
        {
            spectreTask = _context.AddTask(description.ToString(), autoStart: false);
            _runningTasks[record.ActivityId] = spectreTask;

            if (record.PercentComplete == -1)
            {
                spectreTask.IsIndeterminate = true;
            }

            spectreTask.StartTask();
        }
        else
        {
            spectreTask = _runningTasks[record.ActivityId];
        }

        if (!spectreTask.IsIndeterminate && record.PercentComplete != -1)
        {
            spectreTask.Increment(record.PercentComplete - spectreTask.Value);
        }
        spectreTask.Description(description.ToString());

        if (record.RecordType == ProgressRecordType.Completed)
        {
            spectreTask.StopTask();
            _runningTasks.Remove(record.ActivityId);
        }

        if (_runningTasks.Count == 0)
        {
            _done.Release();
            _progressTask.GetAwaiter().GetResult();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Cancel()
    {
        foreach (ProgressTask spectreTask in _runningTasks.Values)
        {
            spectreTask.StopTask();
        }
        _done.Release();
        _progressTask.GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        _done.Dispose();
        GC.SuppressFinalize(this);
    }
    ~SpectreProgress() => Dispose();
}
