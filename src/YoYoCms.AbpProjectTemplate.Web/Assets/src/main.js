// elementui
import ElementUI from 'element-ui'
import locale from 'element-ui/lib/locale/lang/zh-CN'
import 'element-ui/lib/theme-default/index.css'
Vue.use(ElementUI, {locale})

import $ from 'jquery' // jq
window.$ = window.jquery = window.jQuery = $

// abp的一系列框架封装
import './vendor/abp/scripts/abp.js'
import './common/utils/toastrImp'
import './vendor/abp/scripts/libs/abp.jquery.js'
import './vendor/abp/scripts/libs/sweetalert/sweetalert.min'
import './vendor/abp/scripts/libs/abp.sweet-alert'
import './vendor/abp/scripts/libs/abp.toastr'
import './vendor/bsb/plugin/node-waves/waves'

import Vue from 'vue'
import './store'
import App from './App.vue'
import router from './router'
import config from './common/config'
import loadFile from './common/utils/loadFile'
// window.abp.appPath = config.apiHost
console.log(config.apiHost, process.env)
Vue.config.productionTip = true

// 加载apb的ajax库
loadFile.loadJs('/api/AbpServiceProxies/GetAll?type=jquery').then(() => {
    /* eslint-disable no-new */
    new Vue({
        el: '#app',
        router,
        template: '<App/>',
        components: {App}
    })
}).catch(() => {
    abp.message.error('加载失败,请刷新后重试')
})
