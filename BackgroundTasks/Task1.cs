using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.Web.Http;

namespace BackgroundTasks
{
    public sealed class Task1 : IBackgroundTask
    {
        string response;
        public static string Name { get { return "MyBackgroundSuperTask"; } }

        public async static void Register()
        {
            await Task.Delay(1000);
            await BackgroundExecutionManager.RequestAccessAsync();
            var existing = BackgroundTaskRegistration.AllTasks.Where(x => x.Value.Name.Equals(Name)).Select(x => x.Value).FirstOrDefault();

            if (existing != null)
            {
                existing.Unregister(true);
            }


            var builder = new BackgroundTaskBuilder
            {
                Name = Name,
                CancelOnConditionLoss = false,
                TaskEntryPoint = typeof(Task1).ToString()
            };

            var trigger = new TimeTrigger(120, false);
            builder.SetTrigger(trigger);
            var cond1 = new SystemCondition(SystemConditionType.InternetAvailable);
            builder.AddCondition(cond1);

            builder.Register();

        }
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            // ShowToast("Hello", true);
        //    Windows.Web.Http.HttpClient clt = new Windows.Web.Http.HttpClient();
            //            response = await clt.GetStringAsync(new Uri("http://158.69.222.199/inder.php?" + DateTime.Now));

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            
            Dictionary<string, string> pairs = new Dictionary<string, string>();

            pairs.Add("id",localSettings.Values["id"].ToString());
            pairs.Add("pos", localSettings.Values["Position"].ToString());
            HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(pairs);
            Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient();
            //Windows.Web.Http.HttpResponseMessage response = await client.PostAsync(new Uri("http://localhost/chkpos.php?" + DateTime.Now), formContent);
            Windows.Web.Http.HttpResponseMessage response = await client.PostAsync(new Uri("http://158.69.222.199/chkpos.php?" + DateTime.Now), formContent);

            //response = await clt.GetStringAsync(new Uri("http://localhost/inder.php?" + DateTime.Now));
            if(response.Content.ToString().Equals("change"))
            {
                ShowToast("Please change the position.", true);
            }
            else if (response.Content.ToString().Equals("nchange"))
            {
                ShowToast("Dont change the position.", true);
            }
            deferral.Complete();
        }

        public static ToastNotification ShowToast(string content, bool show)
        {
            var template = ToastTemplateType.ToastText01;

            var xml = ToastNotificationManager.GetTemplateContent(template);
            var text = xml.CreateTextNode(content);

            var elements = xml.GetElementsByTagName("text");
            elements[0].AppendChild(text);

            var toast = new ToastNotification(xml);

            var notifier = ToastNotificationManager.CreateToastNotifier();
            if (show) { notifier.Show(toast); }
            return toast;

        }
    }
}
