using Allure.Net.Commons;

namespace POC.Playwright.Core.Logging
{
    public class AllureReport
    {
        private readonly Stack<string> _stepStack = new();

        public async Task AddStepAsync(string stepName, Func<Task> action)
            => await ExecuteStepAsync(stepName, action);

        public void AddStep(string stepName, Action action = null)
            => ExecuteStep(stepName, action ?? (() => { }));

        public async Task AddSubStepAsync(string stepName, Func<Task> action)
            => await ExecuteStepAsync(stepName, action, isSubStep: true);

        public void AddSubStep(string stepName, Action action = null)
            => ExecuteStep(stepName, action ?? (() => { }), isSubStep: true);

        private void ExecuteStep(string stepName, Action action, bool isSubStep = false)
        {
            if (_stepStack.Count == 0 || !isSubStep)
            {
                // New top-level step
                AllureApi.Step(stepName, () =>
                {
                    _stepStack.Push(stepName);
                    action();
                    _stepStack.Pop();
                });
            }
            else
            {
                // Nested inside currently running parent step
                AllureApi.Step(stepName, action);
            }
        }

        private async Task ExecuteStepAsync(string stepName, Func<Task> action, bool isSubStep = false)
        {
            if (_stepStack.Count == 0 || !isSubStep)
            {
                await AllureApi.Step(stepName, async () =>
                {
                    _stepStack.Push(stepName);
                    await action();
                    _stepStack.Pop();
                });
            }
            else
            {
                await AllureApi.Step(stepName, async () => await action());
            }
        }
    }
}
