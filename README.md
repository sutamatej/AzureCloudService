AzureCloudService
=================

This is a project that demostrates different use cases for Windows Azure cloud.

Sending emails with worker role and Razor engine
------------------------------------------------

[ActionMailer.Standalone](http://nuget.org/packages/ActionMailer.Standalone/) package is used in order
to send email messages from worker project which is a non-MVC project. This package enables usage of
Razor templating engine for email content. Unfortunately, at the moment ActionMailer.Standalone
specifically requires Razor engine of version 3.0.8, which makes it a bit messy when trying to install
ActionMailer through NuGet (NuGet installs 3.2.0 by default).

How does it work?
-----------------

Web role project inserts messages into a job queue which is periodically checked by worker role.
The role picks up a message, parses it and executes an applicable job. In this case an email job
renders an email body from a template and sends it, but it's possible to create other types of jobs
depending on your needs.

[Role content folders](http://blogs.msdn.com/b/philliphoff/archive/2012/06/08/add-files-to-your-windows-azure-package-using-role-content-folders.aspx)
are leveraged to ensure that correct template paths are used. All the email templates are copied to BackgroundWorker
role content folder during solution build. This is achieved with `AfterBuild` target in BackgroundWorker
project file:

```xml
<Target Name="AfterBuild">
  <MakeDir Directories="$(SolutionDir)CloudService\BackgroundWorkerContent\EmailTemplates" Condition="!Exists('$(SolutionDir)CloudService\BackgroundWorkerContent\EmailTemplates')" />
  <ItemGroup>
    <Templates Include="$(ProjectDir)EmailTemplates\*.*" />
  </ItemGroup>
  <Copy SourceFiles="@(Templates)" DestinationFolder="$(SolutionDir)CloudService\BackgroundWorkerContent\EmailTemplates" />
</Target>
```

Emails are rendered into `C:\Temp` folder.

TODO
----

* Investigate usage of (stringly typed) Html helpers in email templates
* Remove the dependency on the specific Razor Engine version (contribute to ActionMailer project)

Resources
---------

[Action Mailer Grok Talk Samples](https://github.com/philjones88/DNDN-ActionMailer)

[ActionMailer.Net source code](https://bitbucket.org/swaj/actionmailer.net/wiki/Home)

[Windows Azure Multi-tier Application series](http://www.windowsazure.com/en-us/develop/net/tutorials/multi-tier-web-site/1-overview/)
