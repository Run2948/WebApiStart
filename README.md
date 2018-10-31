# WebApi��Ŀ�Ĵ

#### 1.[ʹ��Entity Framework Code Firstģʽ�������ݿ����](http://www.cnblogs.com/fzrain/p/3491804.html)

#### 2.[����JSON�ķ����շ�ʽ������ʽ](http://www.cnblogs.com/fzrain/p/3520442.html)

#### 3.[����WebApi��������Ϊjson��ʽ](https://www.cnblogs.com/sky-net/p/5956538.html)
* ������еķ��ص�xml��ʽ��
```csharp  
   // �ҵ�Global.asax�ļ�����Application_Start()���������һ�䣺
   GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
   // �ҵ�App_Start�е�WebApiConfig.cs�ļ������ҵ�Register(HttpConfiguration config)������������´���:
   config.Formatters.Clear();
   config.Formatters.Add(new JsonMediaTypeFormatter());
```
* �Զ��巵������(��������ΪHttpResponseMessage)��`JsonContentNegotiator.cs`
```csharp
   // �ҵ�App_Start�е�WebApiConfig.cs�ļ������ҵ�Register(HttpConfiguration config)������������´���:
   var jsonFormatter = new JsonMediaTypeFormatter(); 
   config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter)); 
```
##### 3.1 ��ϣ��Json�ӿڱ�¶�����ԣ����������� JsonIgnore ���
```csharp
  [JsonIgnore]
  public DateTime Birthday { get; set; }
```
##### 3.2 ����Json���ݽӿ����Ե����򣬿���ʹ�� JsonProperty(Order = 1) 
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
#### 4.[��Ninjectʵ������ע��](http://www.cnblogs.com/fzrain/p/3520442.html)

#### 5.[Web Api�İ�ȫ��](https://www.cnblogs.com/fzrain/p/3552423.html)
* ��Web Api��ǿ��ʹ��Https
* ʹ��Basic Authentication��֤�û�

#### 6.[ʹ��CacheCow��ETagʵ�ֱ����ڴ滺����Դ](http://www.cnblogs.com/fzrain/p/3618887.html)

#### 7.[ʹ��CacheCow��SQL Server��ʵ�ֻ���](http://www.cnblogs.com/fzrain/p/3618887.html)

#### 8.[��������·�ɣ�Attribute Routing��](http://www.cnblogs.com/fzrain/p/3591040.html)