import Vue from 'vue'
import Router from 'vue-router'

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
                path: 'tenant.dashboard',
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

router.beforeEach((to, from, next) => {
    console.log(to, from, next)
    next()
})

export default router