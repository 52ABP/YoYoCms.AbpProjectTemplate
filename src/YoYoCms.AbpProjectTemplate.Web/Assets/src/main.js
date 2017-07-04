// elementui
import ElementUI from 'element-ui'
import locale from 'element-ui/lib/locale/lang/zh-CN'
import 'element-ui/lib/theme-default/index.css'
Vue.use(ElementUI, {locale})

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
// import VueI18n from 'vue-i18n'
// Vue.use(VueI18n)
// window.abp.appPath = config.apiHost
Vue.config.productionTip = config.isDebug

// 加载apb的ajax库
loadFile.loadJs('/api/AbpServiceProxies/GetAll?type=jquery').then(() => {
    /* eslint-disable no-new */
    new Vue({
        el: '#app',
        router,
        template: '<App/>',
        components: {App},
        // i18n
    })
}).catch(() => {
    // //todo:黄总这里有个问题，就是我的后端崩溃了。。你这个怎么玩。。这里的错误提示就GG了。。
    abp.message.error('加载失败,请刷新后重试')
})
