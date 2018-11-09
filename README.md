# README

This is a minimal example of the bug described in [Fulma Issue 169](https://github.com/MangelMaxime/Fulma/issues/169).
To encounter the bug, run the following:

```
fake build --target Bundle
cd deploy
dotnet Server.dll
```

Then, open your web browser to `localhost:8085/contact`.

For some reason the bug *only* occurs on the contact page, rendering it from other routes works fine (which is highly confusing and probably indicats that there's something very weird going on.)
This code is extracted from a larger project which has more routes, and has been modified to be as minimal of an example as possible.