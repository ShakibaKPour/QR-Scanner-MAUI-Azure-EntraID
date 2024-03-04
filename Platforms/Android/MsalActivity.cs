using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace RepRepair.Platforms.Android
{
        [Activity(Exported = true)]
        [IntentFilter(new[] { Intent.ActionView },
            Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
            DataHost = "auth",
            DataScheme = "msaldff3c905-f0d7-4071-99cf-9cb059eb6fcd")]
        public class MsalActivity : BrowserTabActivity
        {
        }
}
