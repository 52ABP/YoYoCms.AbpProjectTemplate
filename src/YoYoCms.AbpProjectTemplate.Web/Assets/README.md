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
进行开发前, 需要你有 `es6` , `sass`和 `vue` 的技能基础
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

