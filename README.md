
### .NET applications referencing vulnerable NuGet-packages LiteDB and Newtonsoft.Json

    OblivionAppPackageConfigNet48
    OblivionAppPackageReferenceNet48
    OblivionAppPackageReferenceDotNet60


### node.js application referencing vulnerable NPM-packages matrix-react-sdk 

    OblivionAppPackageReferenceNodeJs
    OblivionAngularWebAppJavaScriptReference

### .NET applications having SQL Injections etc.

    CyberSecurityWebApplication

### .NET applications referencing NuGetPackages which references to other packages

    
<pre>    
PinguinAppPackageReferenceNet48
    
snyk test --print-deps

PinguinAppPackageReferenceNet48 @ 1.0.0
   ├─ AngularJS.Route @ 1.8.2
   │  └─ AngularJS.Core @ 1.8.2
   └─ Microsoft.AspNet.Mvc @ 5.2.9
      ├─ Microsoft.AspNet.Razor @ 3.2.9
      └─ Microsoft.AspNet.WebPages @ 3.2.9
         ├─ Microsoft.AspNet.Razor @ 3.2.9
         └─ Microsoft.Web.Infrastructure @ 1.0.0
</pre>    


## HTML-file directly referencing vulnerable jquery 1.7.1

     laps_compare
