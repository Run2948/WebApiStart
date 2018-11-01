# WebApi��Ŀ�Ĵ

## һ��Learning.WebApi

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


## ����WebApi.Jwt
Authentication for ASP.NET Web Api using simple JWT

#### 1.[WebApiʹ��JWT��֤](https://www.cnblogs.com/wangyulong/p/8727683.html)

#### 2.[ʲô��JWT ? -- JSON WEB TOKEN](https://www.jianshu.com/p/576dbf44b2ae)
###### ע�⣺secret�Ǳ����ڷ������˵ģ�jwt��ǩ������Ҳ���ڷ������˵ģ�secret������������jwt��ǩ����jwt����֤�����ԣ������������˵�˽Կ�����κγ�������Ӧ����¶��ȥ��һ���ͻ��˵�֪���secret, �Ǿ���ζ�ſͻ����ǿ�������ǩ��jwt�ˡ�
##### ���Ӧ��
һ����������ͷ�����`Authorization`��������`Bearer`��ע��
```js
fetch('api/user/1', {
  headers: {
    'Authorization': 'Bearer ' + token
  }
})
```
����˻���֤token�������֤ͨ���ͻ᷵����Ӧ����Դ���������̾���������:
![](./1.png)
##### �ŵ�
* ��Ϊjson��ͨ���ԣ�����JWT�ǿ��Խ��п�����֧�ֵģ���JAVA,JavaScript,NodeJS,PHP�Ⱥܶ����Զ�����ʹ�á�
* ��Ϊ����payload���֣�����JWT����������洢һЩ����ҵ���߼�����Ҫ�ķ�������Ϣ��
* ���ڴ��䣬jwt�Ĺ��ɷǳ��򵥣��ֽ�ռ�ú�С���������Ƿǳ����ڴ���ġ�
* ������Ҫ�ڷ���˱���Ự��Ϣ, ����������Ӧ�õ���չ

## ����JwtDemo
#### [����JWT��web api�����֤���������ʵ��](https://www.cnblogs.com/lwhkdash/p/6686999.html)
#### [WebApi ����������������CORS](https://www.cnblogs.com/landeanfen/p/5177176.html)
* IE8��9��������ǲ���֧��CORS�Ľ��������
* �ڵ��ô�ָ��`jQuery.support.cors = true; `��һ����ܽ��
```js
jQuery.support.cors = true;
var ApiUrl = "http://localhost:27221/";
$(function () {
    $.ajax({
        type: "get",
        url: ApiUrl + "api/Charging/GetAllChargingData",
        data: {},
        success: function (data, status) {
            if (status == "success") {
                $("#div_test").html(data);
            }
        },
        error: function (e) {
            $("#div_test").html("Error");
        },
        complete: function () {

        }
    });
});
```
* �����Ҫ��������������֤
```js
jQuery.support.cors = true;
var ApiUrl = "http://localhost:27221/";
$(function () {
    $.ajax({
        type: "get",
        url: ApiUrl + "api/Charging/GetAllChargingData",
        data: {},
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        success: function (data, status) {
            if (status == "success") {
                $("#div_test").html(data);
            }
        },
        error: function (e) {
            $("#div_test").html("Error");
        },
        complete: function () {

        }
    });
});
```
* ע��������仰��crossDomain: true,xhrFields: {withCredentials: true}
