# Data Page

implemented in both examples/Node.Net.AspNet.Host and examples/Node.Net.Wasm

## Files

The data page has a Files tab with the component Node.Net.Components.Files

The Node.Net.Component.Files component has a FluentToolbar that allows a user to select a specific IFileSystem
for IFileSystemRegistry by name, if there are no registered IFileSystem entries, a message indicating such is displayed.

The Node.Net.Component.FileSystem component display a ui for a specific IFileSystem.
It lists all directory and file items in the IFileSystem in a Tree View, (leverage the Fluent NavMenuTree for this purpose)