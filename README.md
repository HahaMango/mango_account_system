# mango_account_system

SSO System Base on IdentityServer and AspDotNetCore Identity

> English version coming soon. I'm not good at English,I'm working hard at it,Thank.

[![Build Status](https://dev.azure.com/q932104843/mango_user_system/_apis/build/status/HahaMango.mango_account_system?branchName=master)](https://dev.azure.com/q932104843/mango_user_system/_build/latest?definitionId=4&branchName=master)

## 简介

该项目是一个支持OAuth2.0的单点登陆系统，支持多种OAuth2.0的第三方登陆，例如`Github`，`Gitee`，`weibo`等。该系统差不多是一个最小化的可运行的`IdentityServer4`，`Identity`的系统。用户信息等都只包含了必须的字段，如果需要包含其他的用户字段的话可以给该库贡献代码，谢谢。

基于`IdentityServer4`，用`Identity`来进行用户信息管理，所有的用户信息表经过重新设计（自定义），`Identity`中的默认EF储存也被重写过以适配新的用户信息表。

主页：

![HomePage](https://s2.ax1x.com/2020/02/16/3pK9H0.png)

登陆页面：
![LoginPage](https://s2.ax1x.com/2020/02/16/3pKpBq.png)

用户信息页面：
![UserInfo Page](https://s2.ax1x.com/2020/02/16/3puxjs.png)

## 环境

- `.Asp.Net Core 2.2`
- `IdentityServer4`
- `VS 2017（非必要）`

## 构建

CD 到项目根目录

执行：

```powershell
dotnet build
```

就会执行包还原和构建了。

## 调试和部署

在调试（非生产）的时候，需要对`Setup.cs`作一些修改。

```csharp
services.AddIdentityServer()
    .AddAspNetIdentity<MangoUser>()
    //删除证书
    //.AddSigningCredential(new X509Certificate2(Configuration["IdentityServerPfx:Path"], Configuration["IdentityServerPfx:Password"]))
    //添加调试临时证书
    .AddDeveloperSigningCredential();
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryClients(Config.GetClients(Configuration))
    .AddInMemoryIdentityResources(Config.GetIdentityResources());
```

关于部署，建议在部署的时候生成两个自签证书，一个证书是给`IdentityServer4`使用，而另一个证书是给该`WebApp`本身使用。

> 注意：这两个证书跟在nginx中配置的证书是不一样的。如果加上nginx上的证书就是三个证书了。

`Asp.Net Core`在生产环境的话是需要有证书才会开启`https`端口的监听，而对于`IdentityServer4`最好是都采用`https`的请求。

生产环境中可以在json配置文件中加入

```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:6000"
      },

      "HttpsInlineCertFile": {
        "Url": "https://localhost:6001",
        "Certificate": {
          "Path": "pfxname.pfx",
          "Password": "password",
          "AllowInvalid": "true"
        }
      }
    }
  }
}
```

从而开启`https`监听。

## 贡献

首先欢迎所有人对该项目贡献代码。

如果想为该库添加一些特性或者功能，修复bug的话可以发issues，或者直接PR。
