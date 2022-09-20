# Orleans.StorageProviderInterceptors

[![Orleans.StorageProviderInterceptors NuGet Package](https://img.shields.io/nuget/v/Orleans.StorageProviderInterceptors.svg)](https://www.nuget.org/packages/Orleans.StorageProviderInterceptors/) [![Orleans.StorageProviderInterceptors NuGet Package Downloads](https://img.shields.io/nuget/dt/Orleans.StorageProviderInterceptors)](https://www.nuget.org/packages/Orleans.StorageProviderInterceptors) [![GitHub Actions Status](https://github.com/ElanHasson/Orleans.StorageProviderInterceptors/workflows/Build/badge.svg?branch=main)](https://github.com/ElanHasson/Orleans.StorageProviderInterceptors/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/ElanHasson/Orleans.StorageProviderInterceptors?branch=main&includeBuildsFromPullRequest=false)](https://github.com/ElanHasson/Orleans.StorageProviderInterceptors/actions)


An interceptor library for Orleans Storage Providers allowing interception of grain storage provider operations for transparent encryption, model state validation, or whatever else you can dream up!


The current interceptor implementation requires using `StorageInterceptor` instead of Orleans' `PersistentState` attribute on injected grain states.

Check out the sample app: https://github.com/ElanHasson/Orleans.StorageProviderInterceptors/blob/main/Source/Sample/
