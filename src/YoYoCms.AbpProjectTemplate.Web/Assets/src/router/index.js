import Vue from 'vue'
import Router from 'vue-router'
import authUtils from '../common/utils/authUtils'

Vue.use(Router)

import administration from '../router/administration/index'

let router = new Router({
    routes: [
        {
            path: '/login',
            name: 'login',
            component: resolve => {
                require.ensure([], () => {
                    resolve(require('../views/loginregist/Login.vue'))
                })
            },
            meta: {
                notAuth: true // 不需要权限验证
            }
        },
        {
            path: '/',
            name: 'index',
            component: resolve => {
                require.ensure([], () => {
                    resolve(require('../views/Index.vue'))
                })
            },
            children: [{
                path: '',
                name: 'dashboard',
                component: resolve => {
                    require.ensure([], () => {
                        resolve(require('../views/dashboard/Dashboard.vue'))
                    })
                }
            },
            ]
        },
        administration
    ],
    base: '/view/',
    mode: 'history',
})

let loginouted = false // 是否已经登录超时过
// 10秒后设为已经超时过
setTimeout(() => {
    loginouted = true
}, 1e4)
router.beforeEach((to, from, next) => {
    if (!to.matched.some(record => record.meta.notAuth) && !authUtils.getToken()) {
        // 第一次进来不提示超时
        loginouted && abp.notify.error('未登录或登录已超时, 请重新登录!', '未登录')
        loginouted = true
        next({name: 'login'})
        return
    }
    next()
})

export default router