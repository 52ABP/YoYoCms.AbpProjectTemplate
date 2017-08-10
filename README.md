# 演示网站
首先说下这个项目吧。
如标题一样是基于VUE+.NET开发的框架，也是群友一直吼吼吼要一个vue版本的ABP框架。
我们先来看看首页吧：

![1.gif](http://upload-images.jianshu.io/upload_images/1979022-bf6904d8759f82b6.gif?imageMogr2/auto-orient/strip)
还比较酷炫，提供下演示账号
> 演示地址：http://vue.yoyocms.com/
账号：demo
密码：bb123456
当然你也可以自己注册一个账号，来进行验证。

# 介绍
 首先对不知道ABP框架的同学说明下啥是ABP框架：
>  **ABP是“ASP.NET Boilerplate Project (ASP.NET样板项目)”的简称。**
ASP.NET Boilerplate是一个用最佳实践和流行技术开发现代WEB应用程序的新起点，它旨在成为一个通用的WEB应用程序框架和项目模板。
**框架**
ABP是基于最新的ASP.NET CORE，ASP.NET MVC和Web API技术的应用程序框架。并使用流行的框架和库，它提供了便于使用的授权，依赖注入，验证，异常处理，本地化，日志记录，缓存等常用功能。
**架构**
ABP实现了多层架构（领域层，应用层，基础设施层和表示层），以及领域驱动设计（实体，存储库，领域服务，应用程序服务，DTO等）。还实现和提供了良好的基础设施来实现最佳实践，如依赖注入。
**模板**
ABP轻松地为您的项目创建启动模板。它默认包括最常用的框架和库。还允许您选择单页（Angularjs）或多页架构，EntityFramework或NHibernate作为ORM。访问[官网](http://www.aspnetboilerplate.com/)，了解更多。


# 结构
本次选择的项目的结构为:
- .net framework 4.6 
- VUE + VUEX+ VUE-ROUTER
这样的前后端分离的技术，但是分离的不是很纯粹，因为ABP设计的原因，如果要纯粹的前后端分离，在做授权的时候有点麻烦，也就没有采用token的方式进行授权。

在之前的文章 [[ABP框架]动态web Api的拦截用法](http://www.jianshu.com/p/dca3bdbd9e14) 中有说明如何使用token进行授权验证，目前使用的依然是cookie的方式进行验证。
但是不影响前后端开发方式。
看下登录页面：

![login.gif](http://upload-images.jianshu.io/upload_images/1979022-6ac9f7fd6f128783.gif?imageMogr2/auto-orient/strip)

# 功能实现

![image.png](http://upload-images.jianshu.io/upload_images/1979022-e1c1226d0b2399d7.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 登录注册
- 多语言切换
- 消息管理 
- 租户管理（多租户）
- 角色管理
- Session 
- 授权（权限管理）
- 设置管理
- 多语言管理
- 审计日志 
- 动态WebApi
等功能，希望你自己去体验。

### 如果你的.NET技能不扎实，或者想有提高，ABP是最好的学习框架。

# 前端技术栈说明：
####前端使用的框架
1.[vue](https://vuefe.cn)   
2.[vuex](https://vuex.vuejs.org/zh-cn/)   
3.[vue-router](https://router.vuejs.org/zh-cn/)   
4.[jquery](https://jquery.com/)   

#### UI库
1.[element-ui](https://element.eleme.io)   
2.[waves](http://fian.my.id/Waves/)   
3.[bootstrap](http://www.bootcss.com/)   
4.[BSBADMIN](https://github.com/gurayyarar/AdminBSBMaterialDesign) 


### 项目结构
- build  webpack构建的脚本
- config  webpack构建的配置
- dist build之后的文件
- src  源代码目录
    - assets  全局的资源文件
    - common  公共样式 公共的方法
        - language  语言包(暂时无用)
        - utils  工具类
    - components  全局的组件
    - filters  过滤器
    - mixins  存放sass的各类样式
    - router  路由配置
    - service  接口请求层
    - store  vuex
    - vendor  存放第三方的库
    - views  视图文件
    - vuePlugin 自定义的vue插件
- static  静态文件,编译后的目录不变

以下是前端人员的原话，感谢 [慧鑫666](https://github.com/huanghuixin1) 抽出时间来完善vue的界面。
 
 ## 开发步骤
>进行开发前, 我们假定你有 `es6`,`sass`,`vue`,`vue-router`,`vuex` 的技能基础。    
建议读一读 [尤玉溪大神的建议](https://zhuanlan.zhihu.com/p/23134551)
>### 安装前端依赖
>进入`Web项目`中的Assets目录
```
$ npm i
```

>### 运行项目
>记得先启动后台
```
 $ npm run dev
```
>webpack会用 [express](http://expressjs.com/zh-cn/) 启动一个8986端口的web服务器

### 部署
```
 $ npm run build
```
>该命令会将所有文件编译到 `dist` 目录下, 参考上面的项目结构图

#### 1.添加页面
- 先到 `src/views` 创建一个模块的目录。   
比如当前我添加了一个叫 `administration` 的目录, 其中包含了所有系统管理的页面
每个模块里面也可能会包含 `components` 和 `assets`目录, 表示其中的组件和资源都只属于当前模块   
- 接下来添加一个 `Index.vue`, 作为父路由的页面, 用来控制该模块下的所有页面。   
需要注意 [keep-alive](https://cn.vuejs.org/v2/api/#keep-alive) 的作用
- 然后添加你需要的页面 比如叫 `User.vue`   
如果需要获取数据, 请在 `methods` 中添加名为 `fetchData` 的方法, 在该方法中, 需要在获取完数据后调用 `abp.view.setContentLoading(false)`关闭内容区域的loading遮罩层。（可以参照User.vue)
   
#### 2.添加路由
- 进入`src/router`目录，添加路由的模块文件夹，在该文件夹中添加名为 `index.js`的文件   
- 然后向`src/router/index.js` 中注册当前添加的路由信息   

#### 3.添加service   
- 进入`src/services`目录, 添加对应接口请求的模块, 比如role相关的都放到`roleService.js`中   
该`roleService.js`文件导出的对象和`abp.services.yoyocms.role`是对应的。这样使用的好处是可以统一管理和扩展

以上就是vue版本的ABP 的基本情况了。



# 下载地址：
 vue版本开源地址：https://github.com/yoyocms/YoYoCms.AbpProjectTemplate
 vue版本演示地址：http://vue.yoyocms.com/
angularJS版本开源地址：https://github.com/ltm0203/YoYoCms
angularJS版本演示地址：http://www.yoyocms.com

如果你有好的建议或者bug反馈，请到github上提issue 。

# 同时我们也做了一个项目生成器，功能类似ABP官方的模板。功能会在中旬的时候放出，大家请敬请期待。
