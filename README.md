# WebApi项目的搭建

#### 1.[使用Entity Framework Code First模式构建数据库对象](http://www.cnblogs.com/fzrain/p/3491804.html)

#### 2.[配置JSON的返回驼峰式命名格式](http://www.cnblogs.com/fzrain/p/3520442.html)

#### 3.[设置WebApi返回类型为json格式](https://www.cnblogs.com/sky-net/p/5956538.html)
* 清除所有的返回的xml格式：
```csharp  
   // 找到Global.asax文件，在Application_Start()方法中添加一句：
   GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
   // 找到App_Start中的WebApiConfig.cs文件，打开找到Register(HttpConfiguration config)方法，添加以下代码:
   config.Formatters.Clear();
   config.Formatters.Add(new JsonMediaTypeFormatter());
```
* 自定义返回类型(返回类型为HttpResponseMessage)：`JsonContentNegotiator.cs`
```csharp
   // 找到App_Start中的WebApiConfig.cs文件，打开找到Register(HttpConfiguration config)方法，添加以下代码:
   var jsonFormatter = new JsonMediaTypeFormatter(); 
   config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter)); 
```
##### 3.1 不希望Json接口暴露的属性，这里我们用 JsonIgnore 标记
```csharp
  [JsonIgnore]
  public DateTime Birthday { get; set; }
```
##### 3.2 设置Json数据接口属性的排序，可以使用 JsonProperty(Order = 1) 
```csharp
public class MoStudent
{
    //[DataMember(Order = 1)]
    [JsonProperty(Order = 1)]
    public DateTime Birthday { get; set; }

    //[DataMember(Order = 0)]
    [JsonProperty(Order = 0)]
    public int Id { get; set; }

    //[DataMember(Order = 1)]
    [JsonProperty(Order = 1)]
    public string Name { get; set; }

    //[DataMember(Order = 2)]
    [JsonProperty(Order = 2)]
    public bool Sex { get; set; }

}
```
#### 4.[用Ninject实现依赖注入](http://www.cnblogs.com/fzrain/p/3520442.html)

#### 5.[Web Api的安全性](https://www.cnblogs.com/fzrain/p/3552423.html)
* 在Web Api中强制使用Https
* 使用Basic Authentication验证用户

#### 6.[使用CacheCow和ETag实现本机内存缓存资源](http://www.cnblogs.com/fzrain/p/3618887.html)

#### 7.[使用CacheCow在SQL Server端实现缓存](http://www.cnblogs.com/fzrain/p/3618887.html)

#### 8.[基于特性路由（Attribute Routing）](http://www.cnblogs.com/fzrain/p/3591040.html)