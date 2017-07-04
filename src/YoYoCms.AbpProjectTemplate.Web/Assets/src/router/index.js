import Vue from 'vue'
import Router from 'vue-router'
// import authUtils from '../common/utils/authUtils'
import store from '../store'

Vue.use(Router)

import dashboard from '../router/dashboard'
import loginRegister from '../router/loginregister'
import administration from '../router/administration/index'

let router = new Router({
    routes: [
        {path: '/', redirect: '/login'},
        //  =================================登录注册=====================================
        ...loginRegister,
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
                administration
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
router.beforeEach((to, from, next) => {
    // if (!to.matched.some(record => record.meta.notAuth) && !authUtils.getToken()) {
    //     // 第一次进来不提示超时
    //     loginouted && abp.notify.error('未登录或登录已超时, 请重新登录!', '未登录')
    //     loginouted = true
    //     next({name: 'login'})
    //     return
    // }

    let menu = []
    to.matched.forEach((item) => {
        item.meta.displayName = item.meta.displayName
        menu.push({name: item.name})
    })
    store.dispatch('setIndexMenuActive', {menu})

    next()
})

export default router