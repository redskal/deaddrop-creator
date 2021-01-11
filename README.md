# DeaddropCreator
###### _An open-source, programmable .NET binder_

Deaddrop Creator (DDC)  is a tool that will produce some C# source code for a Windows executable that, when run, will drop any amount of given files to the local filesystem.  With this you can create one file that you can load onto a remote system in order to setup your toolbase.  (Or just use ZIP  compression and 'expand' command?)

Think of it as an open-source binder program (think circa 1998). The reason for doing it this way is simple: more control for the user.  You can adjust, recompile and redistribute a module for each host you're attacking.

Requires you to use Microsoft's C# compiler which can be found: %windir%\Microsoft.NET\Framework\<version>\csc.exe