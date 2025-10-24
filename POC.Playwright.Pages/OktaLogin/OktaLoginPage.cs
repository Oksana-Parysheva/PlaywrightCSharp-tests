using Microsoft.Playwright;
using POC.Playwright.Core.Controls;
using System.Diagnostics;

namespace POC.Playwright.Pages.OktaLogin
{
    public class OktaLoginPage
    {
        protected readonly IPage _page;
        public Textbox UsernameTextbox;
        public Textbox PasswordTextbox;
        public Button LoginButton;
        public Link SelectSendPushLink;

        public OktaLoginPage(IPage page)
        {
            _page = page;
            UsernameTextbox = new Textbox(_page.GetByRole(AriaRole.Textbox, new() { Name = "Enter 521 ID" }));
            PasswordTextbox = new Textbox(_page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }));
            LoginButton = new Button(_page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }));
            SelectSendPushLink = new Link(_page.GetByRole(AriaRole.Link, new() { Name = "Select to get a push" }));
        }

        public async Task SignInAsync(string username, string password)
        {
            await UsernameTextbox.EnterTextAsync(username);
            await PasswordTextbox.EnterTextAsync(password);
            await LoginButton.ClickAsync();
            await Task.Delay(2000);

            // click Select to send push
            if (await SelectSendPushLink.IsVisibleAsync())
            {
                await SelectSendPushLink.ClickAsync();
            }

            await Task.Delay(5000);
            //do
            //{
                RunCmdCommandToTapButton();
            //}
            //while (await _page.Locator("span .caret").IsHiddenAsync());
        }

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
