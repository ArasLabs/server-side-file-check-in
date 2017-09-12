using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Xml;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Aras.IOM;

namespace SSFileCheckIn
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //System.Diagnostics.Debugger.Break();
            Page.Response.ContentType = "text/xml";
            StreamReader reader = new StreamReader(Page.Request.InputStream);
            String xmlData = reader.ReadToEnd();

            XmlDocument reqData = new XmlDocument();
            reqData.LoadXml(xmlData);
            XmlNode myData = reqData.SelectSingleNode("data");
            string url = myData.SelectSingleNode("url").InnerXml;
            string database = myData.SelectSingleNode("database").InnerText;
            string userName = myData.SelectSingleNode("user").InnerText;
            string password = myData.SelectSingleNode("password").InnerText;
            Response.Clear();
            Response.Write(CreateFileItem(url, database, userName, password));
            Response.End();
        }
        string CreateFileItem(string URL, string database, string username, string password)
        {
            string filePath = "C:\\temp\\TesTDocument1.doc";
            HttpServerConnection conn = IomFactory.CreateHttpServerConnection(URL, database, username, password);
            Item login_result = conn.Login();
            if (login_result.isError())
            {
                throw new Exception("Login failed");
            }
            Innovator inn = IomFactory.CreateInnovator(conn);

            Item fileItem = inn.newItem("File", "add");
            fileItem.setProperty("filename", new System.IO.FileInfo(filePath).Name);
            fileItem.attachPhysicalFile(filePath);
            Item result = fileItem.apply();
            conn.Logout();
            if (result.isError())
            {
                return result.getErrorString();
            }
            else
            {
                return "Success!";
            }

        }
    }
}
