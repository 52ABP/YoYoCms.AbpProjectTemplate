## 项目介绍
前端使用的框架

1.vue   
2.vuex   
3.vue-router   
element-ui jquery

### 运行项目
```
 npm i
 npm run dev
```


### 项目结构
- build  webpack构建的脚本
- config  webpack构建的配置
- src  源代码目录
    - assets  全局的资源文件
    - common  公共样式 公共的方法
        - language  语言包(还未实现)
        - utils  工具类
    - components  全局的组件
    - filters  过滤器
    - mixins  存放sass的各类样式
    - router  路由配置
    - service  接口请求层
    - store  vuex
    - vendor  存放第三方的库
    - views  视图文件
- static  静态文件,编译后的目录不变
