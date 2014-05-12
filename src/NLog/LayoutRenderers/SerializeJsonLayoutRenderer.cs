#if !SILVERLIGHT

namespace NLog.LayoutRenderers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using NLog.Config;

    /// <summary>
    /// The serialized object (message) in JSON
    /// </summary>
    [LayoutRenderer("jsonserialized")]
    [ThreadAgnostic]
    public class SerializeJsonLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="logEvent"></param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            try
            {
                logEvent.Parameters.ToList().ForEach(n =>
                {
                    if (n.GetType().IsClass) return;
                    var serializerObj = new DataContractJsonSerializer(n.GetType());
                    using (var ms = new MemoryStream())
                    {
                        serializerObj.WriteObject(ms, n);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var sr = new StreamReader(ms))
                        {
                            builder.Append(sr.ReadToEnd());
                        }
                    }
                });

            }
            catch (Exception)
            {
                builder.Append("Failed to serialize object:");
                builder.Append(logEvent.FormattedMessage);
            }
        }
    }
}
#endif