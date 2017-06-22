// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import $ from 'jquery'
window.$ = $
import './vendor/abp/scripts/abp.js'
import './vendor/abp/scripts/libs/abp.jquery.js'
import './vendor/abp/scripts/libs/sweetalert/sweetalert.min'
import './vendor/abp/scripts/libs/abp.sweet-alert'

import Vue from 'vue'
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
