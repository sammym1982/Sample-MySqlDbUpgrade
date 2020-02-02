> Intro

This project uses the DBUp package to create and upgrade our mysql databases. For each new micro service which has the database and needs it
to be upgraded. Do following.

1. Add a class library which will contain all the scripts for database. **IMPORTANT**: Add all the scripts as EmbeddedResource.
2. Recommended: Follow StepXXX_{SomeName}.sql as the naming convention.
3. In DatabaseSetup project add reference to the newly created class library. This will allow database upgrade to load all scripts.


> sample commands

```
DatabaseSetup.exe -c "{connection_string}" -a "{assemblyname}"
DatabaseSetup.exe --help
```

> Reference

[https://github.com/DbUp/DbUp](https://github.com/DbUp/DbUp)