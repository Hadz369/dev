using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Reporting
{
    /// <summary>
    /// Generic report builder class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReportBuilder
    {
        /// <summary>
        /// Constructor accepting an XML configuration element
        /// </summary>
        /// <param name="config">XML configuration element</param>
        public ReportBuilder(XmlElement config) { }


        public IRenderer Report { get; }

        public void Execute() { }
    }

    public interface IRenderer
    {
        Stream Report { get; }

        void Render();
    }

    public abstract class RendererBase : IRenderer
    {
        Stream _report = null;

        public Stream Report { get { return _report; } }

        public abstract void Render() { }
    }

    public class CrystalRenderer : RendererBase
    {
        public override void Render()
        {
            // Create the stream here
            throw new NotImplementedException();
        }
    }
}


