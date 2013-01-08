using System;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PyTestMon.Core
{
    public class BrowserWriter
    {
        public WebBrowser Browser { get; set; }

        public void Navigate(string url)
        {
            Browser.Navigate(url);

            if (Browser.Document == null)
                throw DocumentNotLoaded();

            //Browser.Document.ContextMenuShowing += (o, e) => { e.ReturnValue = false; };
        }

        public void Write(XElement element)
        {
            if (Browser == null)
                throw NoBrowserSet();

            Browser.Invoke(
                new MethodInvoker(
                    () =>
                    {
                        if (Browser.Document == null)
                            throw DocumentNotLoaded();

                        if (Browser.Document.Body == null)
                            throw NoDocumentBody();

                        Browser.Document.Body.InnerHtml += element.ToString();
                    }));
        }

        public void ClearBody()
        {
            if (Browser == null)
                throw NoBrowserSet();

            Browser.Invoke(
                new MethodInvoker(
                    () =>
                    {
                        if (Browser.Document == null)
                            throw DocumentNotLoaded();

                        if (Browser.Document.Body == null)
                            throw NoDocumentBody();

                        Browser.Document.Body.InnerText = "";
                    }));
        }

        public void AppendToElementId(string id, XElement detailsElement)
        {
            if (Browser == null)
                throw NoBrowserSet();

            Browser.Invoke(
                new MethodInvoker(
                    () =>
                    {
                        if (Browser.Document == null)
                            throw DocumentNotLoaded();

                        if (Browser.Document.Body == null)
                            throw NoDocumentBody();

                        HtmlElement parent = Browser.Document.GetElementById(id);

                        if (parent == null)
                            throw ElementNotFound();

                        parent.InnerHtml += detailsElement.ToString();
                    }));
        }

        public void PrependToElementId(string id, XElement detailsElement)
        {
            if (Browser == null)
                throw NoBrowserSet();

            Browser.Invoke(
                new MethodInvoker(
                    () =>
                    {
                        if (Browser.Document == null)
                            throw DocumentNotLoaded();

                        if (Browser.Document.Body == null)
                            throw NoDocumentBody();

                        HtmlElement parent = Browser.Document.GetElementById(id);

                        if (parent == null)
                            throw ElementNotFound();

                        var text = new StringBuilder(detailsElement.ToString());
                        text.Append(parent.InnerHtml);

                        parent.InnerHtml = text.ToString();
                    }));
        }

        private static readonly Func<Exception> NoBrowserSet = () => new InvalidOperationException("Browser object must be set before writing");
        private static readonly Func<Exception> DocumentNotLoaded = () => new InvalidOperationException("The browser must have a document loaded");
        private static readonly Func<Exception> ElementNotFound = () => new InvalidOperationException("Element not found in document");
        private static readonly Func<Exception> NoDocumentBody = () => new InvalidOperationException("The document must have a body before it can be written to");
    }
}