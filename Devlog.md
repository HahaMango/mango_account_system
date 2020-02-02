# 记录开发过程中出现的问题

1. 刚开始出现了循环登陆的问题，原因是在`identityServer`中添加了`Identity`的支持，但是登陆方式还是用的cookie的方式进行登陆所以登陆完成后向`IdentityServer`发起登陆验证的时候又验证到没有登陆所以又调整到登陆的`Action`。后来改用`Identity`的`SignInManager`就解决问题了。

2. 想要更自由的拓展`Identity`的数据库表结构，需要实现`IUserStore`接口并且注入到容器中。但是由于官方也并没有很详细的接口的实现帮助文档，所以得看`UserManager`的源码知道在源码里面如何使用`IUserStore`。

3. 其中`FindByNameAsync()`这个方法输入的是格式化后的名称，从`Identity`的源码中可以看到格式化是把用户名都转换成大写来实现。所以在该项目中用户表有两个名称字段，一个就是用户输入的名称，一个是格式化后用作搜索的名称。

4. 其中以Set开头的方法不需要直接访问数据库，在传递进来的实参上进行操作就好了。`UserManager`会在之后进行`Update`更新数据库。还有一点就是当数据库中没有找到用户时，返回值为null，不能够抛出异常。

5. 必须在访问https上的Endpoint

6. 如果需要在`access token`中包含用户的角色名称或者一些自定义的claims，可以在定义API资源`ApiResource`的时候设置`UserClaims`属性，就可以包含自定义的claims了。而如果在`IdentityResource`资源的定义中添加自定义的claims则自定义的claims只会在`id token`中出现。

## 生成数字证书

对于`identityserver`和`asp.net core`应用本身在生产环境中必须要使用数字证书，在开发环境中的数字证书都是临时的并不能够用在生产环境中。

在该项目中总共有两个数字证书，分别是`identityserver`使用和供`asp.net core`的HTTPS使用。

证书的生成使用`openssl`来生成自签名的证书：

```shell
openssl req -x509 -newkey rsa:4096 -keyout myapp.key -out myapp.crt -days 3650 -nodes -subj "/CN=myapp" 

openssl pkcs12 -export -out myapp.pfx -inkey myapp.key -in myapp.crt -name "Some friendly name"
```

`identityserver`的证书在`setup.cs`中配置。`asp.net`的证书可以添加如下配置到`appsettings.json`中：
```
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:6003"
      },

      "HttpsInlineCertFile": {
        "Url": "https://localhost:6004",
        "Certificate": {
          "Path": "localhost.pfx",
          "Password": "228887",
          "AllowInvalid": "true"
        }
      }
    }
  }
}
```

## 部署前的准备

- `identityserver`中的Client定义中的返回地址等要检查好，一般开发都是localhost等的地址，但是这地址在生产环境的话会无法工作的。

- `nginx`配置中通过`server_name`可以匹配三级域名，可以用三级域名分别对应同一个服务器的不同服务。

> 阿里云的免费SSL证书只能保护一个域名，所以每个子域名都需要一个SSL证书保护。通配符证书可以保护所有的子域名但是需要收费。
