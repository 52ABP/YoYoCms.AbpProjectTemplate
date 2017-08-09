import Vue from 'vue'
import Router from 'vue-router'
// import authUtils from '../common/utils/authUtils'
import store from '../store'

Vue.use(Router)

import abpScriptService from '../services/abpScriptService'
import dashboard from '../router/dashboard'
import loginRegister from '../router/loginregister'
import administration from '../router/administration/index'
import common from '../router/common'

let router = new Router({
    routes: [
        {path: '/', redirect: '/login'},
        {
            path: '/test',
            component: resolve => {
                require.ensure([],
                    () => {
                        resolve(require('../views/Test.vue'))
                    })
            }
        },
        //  =================================登录注册=====================================
        ...loginRegister,
        //  =================================内容部分=====================================
        {
            path: '/',
            name: 'index',
            component: resolve => {
                require.ensure([],
                    () => {
                        resolve(require('../views/Index.vue'))
                    })
            },
            children: [
                //  =================================dashboard=====================================
                dashboard,
                //  =================================管理=====================================
                administration,
                //  =================================公共页面=====================================
                common
            ]
        },
    ],
    base: '/view/',
    mode: 'history',
})

// let loginouted = false // 是否已经登录超时过
// 10秒后设为已经超时过
// setTimeout(() => {
//     loginouted = true
// }, 1e4)
router.beforeEach(async (to, from, next) => {
    abp.view.setContentLoading && abp.view.setContentLoading(true)
    // if (!to.matched.some(record => record.meta.notAuth) && !authUtils.getToken()) {
    //     // 第一次进来不提示超时
    //     loginouted && abp.notify.error('未登录或登录已超时, 请重新登录!', '未登录')
    //     loginouted = true
    //     next({name: 'login'})
    //     return
    // }

    let menu = []
    for (let i = 0; i < to.matched.length; i++) {
        let item = to.matched[i]
        // 如果要开始加载正式内容
        if (item.name === 'index' && abpScriptService.isNeedLoad) {
            abpScriptService.isNeedLoad = false
            // 获取菜单,语言包等信息
            await abpScriptService.getScripts()
        }
        // debugger
        // item.meta.displayName = item.meta.displayName
        menu.push({name: item.name, displayName: item.meta.displayName})
    }

    store.dispatch('setIndexMenuActive', {menu})

    next()
})

router.afterEach(route => {
    Vue.nextTick(() => {
        store.dispatch('addPageTab', {
            item: {
                name: route.name,
                displayName: route.meta.displayName,
                query: route.query,
                params: route.params
            }
        })
    })
})

export default router