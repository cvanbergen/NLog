#if !SILVERLIGHT

namespace NLog.LayoutRenderers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using NLog.Config;

    /// <summary>
    /// The xml-serialized object (message)
    /// </summary>
    [LayoutRenderer("xmlserialized")]
    [ThreadAgnostic]
    public class SerializeXmlLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// Renders the specified object as xml via XML-serialization and appends it to the specified <see cref="StringBuilder" />.
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
                    var serializerObj = new XmlSerializer(n.GetType());
                    using (var ms = new MemoryStream())
                    {
                        serializerObj.Serialize(ms, n);
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