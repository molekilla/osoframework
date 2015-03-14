# OsoFramework #

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

Tutorials:
  * [How to create a new robot](NewRobot.md)