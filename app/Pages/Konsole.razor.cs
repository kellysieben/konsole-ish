using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Web;

namespace app.Pages
{
    public class Command
    {
        public string Text {get; set;}
    }

    public partial class Konsole
    {
        private string cmd = "";
        private string Output { get; set; } = "";
        private string Input { get; set; } = "";
        private List<string> _history = new List<string>();
        private int _historyIndex = 0;
        private void OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "ArrowUp" && _historyIndex > 0)
            {
                _historyIndex--;
                Input = _history[_historyIndex];
            }
            else if (e.Key == "ArrowDown" && _historyIndex + 1 < _history.Count)
            {
                _historyIndex++;
                Input = _history[_historyIndex];
            }
            // todo: doesn't work right when typing new command. Requires Enter to be pressed first sometimes
            // has to do with DOM focus I think
            // currently handling this with javascript as well
            else if (e.Key == "Escape")
            {
                Input = "";
                _historyIndex = _history.Count;
            }
        }

        private async Task CmdEnter()
        {
            Console.WriteLine($"jbf : CMD=[${cmd}]");
        }
        
        private async Task Run(KeyboardEventArgs e)
        {
            if (e.Key != "Enter")
            {
                return;
            }

            var code = Input;
            if (!string.IsNullOrEmpty(code))
            {
                _history.Add(code);
            }

            _historyIndex = _history.Count;
            Input = "";
            await RunSubmission(code);
        }

        private async Task RunSubmission(string code)
        {
            Output += $@"<br /><span class=""info"">{HttpUtility.HtmlEncode(code)}</span>";
            var previousOut = Console.Out;
            try
            {
                // if (TryCompile(code, out var script, out var errorDiagnostics))
                // {
                var writer = new StringWriter();
                Console.SetOut(writer);
                Console.WriteLine($"At some point, this will be meaningful.  Until then, stay jolly bunfiller!");
                // var entryPoint = _previousCompilation.GetEntryPoint(CancellationToken.None);
                // var type = script.GetType($"{entryPoint.ContainingNamespace.MetadataName}.{entryPoint.ContainingType.MetadataName}");
                // var entryPointMethod = type.GetMethod(entryPoint.MetadataName);
                // var submission = (Func<object[], Task>)entryPointMethod.CreateDelegate(typeof(Func<object[], Task>));
                // if (_submissionIndex >= _submissionStates.Length)
                // {
                // Array.Resize(ref _submissionStates, Math.Max(_submissionIndex, _submissionStates.Length * 2));
                // }
                // var returnValue = await ((Task<object>)submission(_submissionStates));
                // if (returnValue != null)
                // {
                // Console.WriteLine(CSharpObjectFormatter.Instance.FormatObject(returnValue));
                // }
                var output = HttpUtility.HtmlEncode(writer.ToString());
                if (!string.IsNullOrWhiteSpace(output))
                {
                    Output += $"<br />{output}";
                }
            // }
            // else
            // {
            // foreach (var diag in errorDiagnostics)
            // {
            // Output += $@"<br / ><span class=""error"">{HttpUtility.HtmlEncode(diag)}</span>";
            // }
            // }
            }
            catch (Exception ex)
            {
                Output += $@"<br /><span class=""error"">{HttpUtility.HtmlEncode(ex)}</span>";
            }
            finally
            {
                Console.SetOut(previousOut);
            }
        }
    }
}