# osoframework
Web robot framework, works with .NET 3.5 and Mono 2.3.

Example code:

```
// using read
string goog1 = Read(new HttpSettings { Query = "http://www.google.com" });

// using get XDocument for url
// pre parsing is for manually parsing non conforming XHTML
var goog2 = ReadXml(new HttpSettings { Query = "http://www.google.com" }, x => x);

byte[] a = ReadWebBinaryResource("http://subsonicproject.com/content/images/SubSonicSMall.png");
            
// Example: Parse site using registed navigation steps
// Get states
states = Navigation.First.Read().ParseXElement
         (data =>
               {
                   return from lnk in GetTagElements(data, "<body", "</body>").HtmlAnchors()
                         where lnk.Attribute("HREF") != null
                         && lnk.Attribute("HREF").Value.StartsWith("/sv/buscar.html")
                         select lnk;
                    }
                );
```

### Why OsoFramework
Because my dog looks like a bear. And also it rhymes with Mono and Awesome.

### What OsoFramework offers
* Easy to parse XHTML pages
* Fast way to save extracted data to databases (SQLite, SQL Server, MySql?)
* LINQ extensions for XDocument allows easy to parse HTML tags
* Text search tag converter when HTML is hard to parse.
* Extensive logging support

### Introduction
This tutorial explains how to create a new web robot using OsoFramework.

### Adding references to a new project
Create a new solution and project from Visual Studio.NET or Mono Develop. Add the following references:

log4net (found in Third Party folder)
System.Configuration
System.Linq
System.Xml
OsoFramework
OsoFramework.Http
SubSonic (found in Third Party folder)
IronPython 2.6 (found in Third Party folder)
Microsoft Unity Block (found in Third Party folder)
Following the OsoExamples project organization, add a DatabaseSchema folder, which stores the database schemas and Robot folder, storing the robots.

### Creating the database schema
In the DatabaseSchema folder, create a new class named SearchResult.cs. Inherit from IParseData and add the following properties described below.
```
        public class SearchResult: IParseData 
        {
                public SearchResult()
                {
                     Title = string.Empty;
                     Url = string.Empty;
                     Description = string.Empty;
                     LastUpdated = DateTime.Now;
                }
                
        /// <summary>
        /// by default we have an autonumber
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// KeyIndex is the column we want to index, useful for querying existing items.
        /// </summary>
        public string KeyIndex
        {
            get
            {
                return Url;
            }
            set
            {
                Url= value;
            }
        }

        public string Url { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime LastUpdated { get; set; }
        }
```
Create another class called SearchResultDataRepository, which is the repository class for the new search results schema.
```
 public class SearchResultDataRepository: DataRepositoryBase 
// Use IDataRepositoryBase if a custom data access API is required
  {                               
                public SearchResultDataRepository()
                {
                }       
    }
```
### Creating the robot class
Add a new class in Robot folder and inherit from WebRobotBase and IWebRobot. Name the class GoogleSearch.

In the Start method, add the following:
```
       public void Start()
        {
            Main();
        }

        private void Main()
        {
            // using read
            string goog1 = Read(new HttpSettings { Query = "http://www.google.com" });
            
            // info prints to log and enterprise manager console
            Info(goog1);

            // using get XDocument for url
            // pre parsing is for manually parsing non conforming XHTML, not used here
            var goog2 = ReadXml(new HttpSettings { Query = "http://www.google.com" },
                                response => response);

            byte[] a = ReadWebBinaryResource("http://subsonicproject.com/content/images/SubSonicSMall.png");
            Info("google ended");

            
        }
        ```
### Configuring Dependency Injection Configuration (Microsoft Unity)
Here you will add the name of the robot, which is required for it to work correctly.

Additionally, add a connection string in the connection strings section and set the correct data repository type.
```
   <!-- 
Google Robot Object
SetConnectionString sets the connection string
Name sets the name of the robot
DatabaseRepository sets the IDataRepository use by the robot
 -->
          <type type="OsoFramework.IWebRobot, OsoFramework"
                    mapTo="OsoExamples.Robot.GoogleExample, OsoExamples"
                    name="googleSearchBot">
            <typeConfig
            extensionType="Microsoft.Practices.Unity.Configuration.TypeInjectionElement, Microsoft.Practices.Unity.Configuration">
              <method name="SetConnectionString">
                <param name="connectionString" parameterType="System.String">
                  <value value="GoogleLinkRepository"/>
                </param>
              </method>
              <property name="Name" propertyType="System.String">
                <value value="Google Robot that grabs links" />
              </property>
              <property
                name="DatabaseRepository"
                propertyType="OsoExamples.DatabaseSchema.GoogleLinkRepository, OsoExamples"  />
            </typeConfig>
          </type>
          
          ```
