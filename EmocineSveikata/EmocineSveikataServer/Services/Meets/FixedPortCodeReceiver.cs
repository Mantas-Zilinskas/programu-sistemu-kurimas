using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using System.Net;

namespace EmocineSveikataServer.Services.Meets
{
    public class FixedPortCodeReceiver : ICodeReceiver
    {
        public string RedirectUri => "http://localhost:5005/authorize/";

        public async Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl url, CancellationToken taskCancellationToken)
        {
            var authUrl = url.Build().ToString();
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = authUrl,
                UseShellExecute = true
            });

            var listener = new HttpListener();
            listener.Prefixes.Add(RedirectUri);
            listener.Start();

            var context = await listener.GetContextAsync();
            var response = context.Response;

            var query = context.Request.QueryString;

            var html = "<html><body>You may now close this window.</body></html>";
            var buffer = System.Text.Encoding.UTF8.GetBytes(html);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
            listener.Stop();

            if (query["error"] != null)
            {
                return new AuthorizationCodeResponseUrl { Error = query["error"] };
            }

            return new AuthorizationCodeResponseUrl { Code = query["code"], State = query["state"] };
        }
    }
}
