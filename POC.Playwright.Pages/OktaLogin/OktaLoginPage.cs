using Microsoft.Playwright;
using System.Diagnostics;

namespace POC.Playwright.Pages.OktaLogin
{
    public class OktaLoginPage
    {
        protected readonly IPage _page;
        private string _usernameInputLocator = "input[autocomplete='username']";
        private string _passwordInputLocator = "input[autocomplete='current-password']";
        private string _signInButtonLocator = "input[value='Sign in']";
        private string _selectSendPushButtonLocator = "a[aria-label='Select to get a push notification to the Okta Verify app.']";

        public OktaLoginPage(IPage page)
        {
            _page = page;
        }

        public async Task SignInAsync(string username, string password)
        {
            await _page.FillAsync(_usernameInputLocator, username);
            await _page.FillAsync(_passwordInputLocator, password);
            await _page.ClickAsync(_signInButtonLocator);

            // click Select to send push
            if (await _page.Locator(_selectSendPushButtonLocator).IsVisibleAsync())
            {
                await _page.ClickAsync(_selectSendPushButtonLocator);
            }

            Thread.Sleep(5000);
            //do
            //{
                RunCmdCommandToTapButton();
            //}
            //while (await _page.Locator("span .caret").IsHiddenAsync());
        }

        public async Task<bool> UsernameIsDisplayedAsync()
            => await _page.Locator(_usernameInputLocator).IsVisibleAsync();

        public static void RunCmdCommandToTapButton()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Android\Sdk\platform-tools");
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = false;
            try
            {
                process.Start();
                process.StandardInput.WriteLine($"cd /d {path}");
                process.StandardInput.WriteLine("IF EXIST \"adb.exe\" ( ECHO \"File exists!\" ) ELSE ( ECHO \"File does not exist.\" )");
                process.StandardInput.WriteLine("adb shell input tap 240 2240");
                process.StandardInput.WriteLine("exit");
            }
            catch { }
        }
    }
}
