## 项目介绍
####前端使用的框架

1.[vue](https://vuefe.cn)   
2.[vuex](https://vuex.vuejs.org/zh-cn/)   
3.[vue-router](https://router.vuejs.org/zh-cn/)   
4.[jquery](https://jquery.com/)   
#### UI库
1.[element-ui](https://element.eleme.io)   
2.[waves](http://fian.my.id/Waves/)   
3.[bootstrap](http://www.bootcss.com/)   
4.[bsb](http://bsb.ddxc.org/pages/ui/breadcrumbs.html)

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

## 开发步骤
进行开发前, 我们假定你有 `es6`,`sass`,`vue`,`vue-router`,`vuex` 的技能基础。    
建议读一读 [尤玉溪大神的建议](https://zhuanlan.zhihu.com/p/23134551)
### 安装前端依赖
进入`Web项目`中的Assets目录
```
$ npm i
```

### 运行项目
记得先启动后台
```
 $ npm run dev
```
webpack会用 [express](http://expressjs.com/zh-cn/) 启动一个8986端口的web服务器

### 部署
```
 $ npm run build
```
该命令会将所有文件编译到 `dist` 目录下, 参考上面的项目结构图

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
