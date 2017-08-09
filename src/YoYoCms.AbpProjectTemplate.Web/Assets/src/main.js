// 初始化abp的对象
window.abp = window.abp || {}
window.abp.view = window.abp.view || {}

import loadFile from './common/utils/loadFile'
import config from './common/config'
window.appConfig = config
// elementui
import ElementUI from 'element-ui'
import locale from 'element-ui/lib/locale/lang/zh-CN'
import 'element-ui/lib/theme-default/index.css'
Vue.use(ElementUI, {locale})

// abp的一系列框架封装
import './vendor/abp/scripts/abp.js'
import './common/utils/toastrImp'
import './vendor/jquery.singlR.min'
import './vendor/abp/scripts/libs/abp.jquery.js'
import './vendor/abp/scripts/libs/sweetalert/sweetalert.min'
import './vendor/abp/scripts/libs/abp.sweet-alert'
import './vendor/abp/scripts/libs/abp.toastr'
import './vendor/bsb/plugin/node-waves/waves'

import Vue from 'vue'
import './store'
import App from './App.vue'
import vueLangPlugin from './vuePlugins/langPlugin' // 语言插件
import hooksPlugin from './vuePlugins/hooksPlugin' // 生命周期插件
import permissionPlugin from './vuePlugins/permissionPlugin' // 权限插件
import abpScriptService from './services/abpScriptService'

// window.abp.appPath = config.apiHost
Vue.config.productionTip = config.isDebug

Vue.use(vueLangPlugin)
Vue.use(hooksPlugin)
Vue.use(permissionPlugin)

// 加载apb的ajax库 和 singlr
Promise.all([loadFile.loadJs(config.apiHost + '/signalr/hubs'), loadFile.loadJs('/api/AbpServiceProxies/GetAll?type=jquery')]).then(async () => {
    require('./vendor/abp/scripts/libs/abp.signalr.js')
    await setElementUiLang()
    /* eslint-disable no-new */
    let router = require('./router').default
    abp.router = router
    new Vue({
        el: '#app',
        router,
        template: '<App/>',
        components: {App},
        // i18n
    })
}).catch((e) => {
    // //todo:黄总这里有个问题，就是我的后端崩溃了。。你这个怎么玩。。这里的错误提示就GG了。。 ==============答: 刷新页面================
    abp.message.error('加载失败,请刷新后重试')
    alert('加载失败,请刷新后重试')
    console.log(e)
})

// 设置elementui的语言包
async function setElementUiLang() {
    // 获取菜单,语言包等信息
    await abpScriptService.getScripts()

    if (abp.localization.currentCulture.name === 'en') {
        let langElement = require('element-ui/lib/locale/lang/en').default
        let localeElement = require('element-ui/lib/locale').default
        localeElement.use(langElement)
    }
}