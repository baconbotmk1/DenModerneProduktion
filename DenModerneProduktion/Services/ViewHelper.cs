using Microsoft.JSInterop;

namespace DenModerneProduktion.Services
{
    public class ViewHelper
    {
        private readonly IJSRuntime _JS;

        public ViewHelper(IJSRuntime JS)
        {
            _JS = JS;
        }

        public void ShowLoader( string text = "")
        {
            _JS.InvokeVoidAsync("showLoader", text);
        }

        public void HideLoader()
        {
            _JS.InvokeVoidAsync("hideLoader");
        }

        public void Alert(string message)
        {
            _JS.InvokeVoidAsync("alert", message);
        }
    }
}
