
namespace HealthSdkPrototype
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Serilog;

    public class OauthWebServer : IDisposable
    {
        private Task _listenerTask;
        private HttpListener _listener;
        private string _listenerUri { get; set; }

        public ChannelWriter<Message> Writer { get; set; }

        public void Start(string uri)
        {
            if (_listenerTask is null)
            {
                _listenerUri = uri;
                _listenerTask = Task.Factory.StartNew(StartListener);
            }
        }

        public void Stop()
        {
            if (_listenerTask != null)
            {
                if (_listener != null && _listener.IsListening)
                {
                    _listener.Stop();
                }
            }
        }

        public void StartListener()
        {
            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add(_listenerUri);
                _listener.Start();

                bool haveCode = false;

                while (_listener != null && _listener.IsListening && haveCode == false)
                {
                    var context = _listener.GetContext();

                    Log.Information("OauthWebServer/RequestUrl:\"{RequestUrl}\"", context.Request.Url);

                    string code = context.Request.QueryString["code"];
                    if (String.IsNullOrWhiteSpace(code) == false)
                    {
                        Log.Information($"OauthWebServer/Code:\"{code}\"");

                        Writer.TryWrite(new Message { Code = code });
                        haveCode = true;
                    }
                    WriteString(context, CloseWindowResponse);

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.Close();
                }
            }
            catch (HttpListenerException ex)
            {
                Log.Error(ex, "OauthWebServer");
                Writer.TryWrite(new Message { Exception = ex });
            }
        }

        public void Dispose()
        {
            if (_listener != null)
            {
                _listener.Close();
                _listener = null;
            }
        }

        private void WriteString(HttpListenerContext ctx, string s)
        {
            byte[] buf = Encoding.UTF8.GetBytes(s);
            ctx.Response.ContentLength64 = buf.Length;
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
        }

        private const string CloseWindowResponse = "<!DOCTYPE html><html><head></head><body onload=\"closeThis();\"><h1>Authorization Successfull</h1><p>You can now close this window</p><script type=\"text/javascript\">function closeMe() { window.close(); } function closeThis() { window.close(); }</script></body></html>";
    }
}
