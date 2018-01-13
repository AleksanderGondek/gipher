using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

using gipher.Giphy;

namespace gipher.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var botHandle = ConfigurationManager.AppSettings["BotId"];
            var giphyApi = new GiphyApi(ConfigurationManager.AppSettings["GiphyApiToken"]);

            var requesterName = activity.From.Name;
            var queryText = activity.RemoveMentionText(botHandle);
            var imageUrl = await giphyApi.GetImageUrlFromText(queryText);

            var responseText = $"{requesterName} searched for '{queryText}' \n\n {imageUrl}";
            await context.PostAsync(responseText);

            context.Wait(MessageReceivedAsync);
        }
    }
}