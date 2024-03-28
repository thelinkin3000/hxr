# HXR
## _I'm sorry vimv, I'm just not used to bash_

HXR is a .net core console application that let's you use Helix to rename the files within your working folder.

- Go to your folder
- Launch hxr
- Use helix to rename the files
- :wq, and enjoy your renamed files!

## Features

- It launches hxr in another window (okay, this is a bug).
- It checks that the filenames you provided are allowed based on your filesystem via the .net core api.
- It checks that you didn't accidentally remove a line and thus mixing up your filenames.
- If it finds any errors it lets you start all over again. No hard feelings, we all make mistakes.
- It's EXTREMELY coupled and NOT AT ALL configurable. I'm sorry, I'll improve it.
