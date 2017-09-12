# Sample for Server Side File Check-in
The sample provides a web page that can be used to create a new file Item from a file on the server

## History
This project and the following release notes have been migrated from the old Aras Projects page.

|Release|Notes|
|-------|---------------------------------------------|
|v1     |Server Side File CheckIn project|
|v2     |Updated for 9.2.0|

### Support Aras Versions
|Project|Aras|
|-------|-----|
|v1     |9.1.0|
|v2     |9.2.0|

## Installation
**Important!
Always back up your code tree and database before applying an import package or code tree path!**

### Pre-requisites
1. Aras Innovator installed
2. Visual Studio 2005 or above installed

### Install Steps
##### Creating the external web application
1. Back up your code tree and store the archive in a safe place
2. Open up the SSFileCheckIn.sln Visual Studio 20005 project
3. Modify the string variable filePath to point to some file you want to check into Innovator
4. Publish the project. Choose a folder like `C:\inetpub\wwwroot\TestServerSideCheckIn`
5. Add a new Web Application in IIS that maps to the TestServerSideCheckIn folder
6. Verify that you can navigate to the URL you just created from the server: i.e. <http://localhost/TestServerSideCheckIn>
+ This page will error out since no post data exists (Root element is missing)

##### Innovator Method Creation
1. Create a Server-side Method that will connect to this webpage similar to the example below
``` csharp
Innovator inn = this.getInnovator();
WebRequest req = null;
WebResponse rsp = null;

//get logged on users login credentials

System.Collections.Specialized.NameValueCollection myHeaders = HttpContext.Current.Request.Headers;

string userName = myHeaders.Get("AUTHUSER").ToString();
string passMD5 = myHeaders.Get("AUTHPASSWORD").ToString();

//URL to your Innovator Sever
string innURL = "http://localhost/InnovatorServer/Server/InnovatorServer.aspx"; 
//URL to your External App
string postURL = "http://localhost/testserversidecheckin/default.aspx";  
//This is your Solutions Database
string db = "InnovatorSolutions";  

// XML Post Data
string xml = "<data>"+
                "<user>"+userName+"</user>"+
                "<password>"+passMD5+"</password>"+
                "<url>"+innURL+"</url>"+
                "<database>"+db+"</database>"+
              "</data>";
                  

XmlDocument postXML = new XmlDocument();
postXML.LoadXml(xml);
//  Create a web request using an HTTP Post to post XMLdata
req = WebRequest.Create(postURL);
req.Method = "POST";        // Post method
req.ContentType = "text/xml";     // content type
// Write http XML Post to Web App
StreamWriter writer = new StreamWriter(req.GetRequestStream());
writer.WriteLine(postXML.InnerXml);
writer.Close();
// Read http responsefrom Web App
rsp = req.GetResponse();
StreamReader reader = new StreamReader(rsp.GetResponseStream());

return inn.newResult(reader.ReadToEnd());//result from web app
```

# Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request

# Credits
Created by Aras Corporation Support

# License
Published to Github under the MIT license. See the [LICENSE File](../blob/master/LICENSE.md) for license rights and limitations.
